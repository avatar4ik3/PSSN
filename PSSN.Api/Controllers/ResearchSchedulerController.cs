using System;
using System.Threading.Channels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PSSN.Api.DAL;
using PSSN.Api.DAL.Entities;
using PSSN.Common.Model;
using PSSN.Common.Requests;
using PSSN.Common.Responses;
using PSSN.Core;
using PSSN.Core.Containers;
using PSSN.Core.Generators;
using PSSN.Core.Operators;
using PSSN.Core.Operators.MemeOperators;
using PSSN.Core.Round;
using Serilog;
using ConditionalStrategy = PSSN.Core.Strategies.ConditionalStrategy;

namespace PSSN.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
[ProducesResponseType(StatusCodes.Status504GatewayTimeout)]
public class ResearchSchedulerController(ScheduleResearchRunner _scheduleResearchRunner) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleGenerationResponse))]
    public async Task<ActionResult<SchedulerResearchResponseModel>> ScheduleResearch(ScheduleResearchRequestModel request)
    {
        var scheduledResearch = await _scheduleResearchRunner.ScheduleResearch(request);
        return Ok(scheduledResearch);
    }

    [HttpGet("{researchGuid:Guid}")]
    public async Task<ActionResult> GetScheduledResearch([FromRoute] Guid researchGuid)
    {
        var research = await _scheduleResearchRunner.GetResearch(researchGuid);
        if (research is null) return NotFound();

        return Ok(research);
    }
}


public class ScheduleResearchRequestModel
{
    public int StrategiesCount { get; set; }
    public decimal DistributionStep { get; set; }
    public double[][]? Ro { get; set; }
    public int CountOfExperiments { get; set; }
    public int GenerationCount { get; set; }
    public int GenCount { get; set; }
    public double SwapChance { get; set; }
    public int CrossingCount { get; set; }
    public int SelectionGroupSize { get; set; }
    public bool UseCrossingOver { get; set; }
}

public class SchedulerResearchResponseModel
{
    public Guid ResearchGuid { get; set; }
    public int CurrentCountOfGames { get; set; }
    public int TotalCountOfGames { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDropped { get; set; }
}

public class CompletedResearchResponseModel
{
    public Guid ResearchGuid { get; set; }
    public int CurrentCountOfGames { get; set; }
    public int TotalCountOfGames { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsDropped { get; set; }
    public List<KeyValuePair<decimal, MemeSingleGenerationResponseModel>>? Results { get; set; }
}

public class ScheduleResearchRunner(IGameRunner _gameRunner, IServiceProvider _serviceProvider, ApplicationContext _context)
{
    public async Task<SchedulerResearchResponseModel> ScheduleResearch(ScheduleResearchRequestModel request)
    {
        var countOfDistributions = (int)Math.Ceiling(1 / request.DistributionStep);
        var totalGamesCount = countOfDistributions * request.CountOfExperiments;

        var createdResearch = await _context.Researches.AddAsync(new()
        {
            CountOfExperiments = request.CountOfExperiments,
            CrossingCount = request.CrossingCount,
            DistributionStep = request.DistributionStep,
            GenCount = request.GenCount,
            Ro = request.Ro!.SelectMany(x => x).ToList(),
            SelectionGroupSize = request.SelectionGroupSize,
            StrategiesCount = request.StrategiesCount,
            SwapChance = request.SwapChance,
            CountOfDistributions = countOfDistributions,
            TotalGamesCount = totalGamesCount,
            UseCrossingOver = request.UseCrossingOver,
        });
        await _context.SaveChangesAsync();

        var researchGuid = createdResearch.Entity.Guid;

        var channel = Channel.CreateUnbounded<WorkerResults>(new()
        {
            SingleReader = true,
            SingleWriter = false
        });

        var writer = channel.Writer;
        var reader = channel.Reader;

        Task.Factory.StartNew(async () =>
        {
            try
            {
                await using var scope = _serviceProvider.CreateAsyncScope();
                var container = scope.ServiceProvider.GetRequiredService<PatternsContainer>();
                //var tasks = Enumerable.Range(0, countOfDistributions + 1)
                //    .Select(x => x * request.DistributionStep)
                //    .SelectMany(x =>
                //        Enumerable.Range(1, request.CountOfExperiments)
                //            .Select(xx => RunSingleExperiment(x, xx, container, writer)).ToArray())
                //    .ToArray();

                await Parallel.ForEachAsync(
                    Enumerable.Range(0, countOfDistributions + 1).Select(x => x * request.DistributionStep), new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount
                    }, async (distr, ct) =>
                    {
                        await Parallel.ForEachAsync(
                            Enumerable.Range(1, request.CountOfExperiments), new ParallelOptions()
                            {
                                MaxDegreeOfParallelism = Environment.ProcessorCount
                            }, async (expNo, ct) =>
                            {
                                await RunSingleExperiment(distr, expNo, container, writer);
                            });
                    });



                async Task RunSingleExperiment(decimal currentDistribution, int currentExperiment, PatternsContainer patternsContainer, ChannelWriter<WorkerResults> channelWriter)
                {
                    try
                    {
                        Log.Information("Started experiment {experiment} for distribution {distribution}", currentExperiment,
                            currentDistribution);
                        var strats = new List<ConditionalStrategy>(request.StrategiesCount);
                        var newPopulation = ConditionalStrategyBuilder
                            .RandomMemes(Random.Shared, (double)currentDistribution, request.StrategiesCount, request.GenCount, patternsContainer)
                            .Select((x, i) =>
                            {
                                x.Id = i;
                                return x;
                            }).ToList();

                        TreeGameRunnerResult tree = null!;

                        foreach (var generation in Enumerable.Range(1, request.GenerationCount))
                        {
                            strats = newPopulation.Select((x, i) =>
                            {
                                x.Id = i;
                                return x;
                            }).ToList();

                            newPopulation = new List<ConditionalStrategy>();
                            tree = _gameRunner.Play(strats, request.Ro!, request.GenCount);

                            var selectionOperator =
                                new SelectionOperator<ConditionalStrategy>(request.SelectionGroupSize, tree, Random.Shared);
                            var crossingOverOperator = new MemeCrossingOverOperator(strats, tree);
                            var mutationOperator = new MemeMutationOperator(request.SwapChance, Random.Shared);

                            foreach (var _ in ..(strats.Count() / 2 + strats.Count() % 2))
                            {
                                var s1 = selectionOperator.Operate(strats);
                                var s2 = selectionOperator.Operate(strats);
                                var toMutate = new[] { s1, s2 };


                                if (request.UseCrossingOver)
                                {
                                    toMutate = crossingOverOperator.Operate(s1, s2).ToArray();
                                }

                                var mutated = mutationOperator.Operate(toMutate);

                                newPopulation.AddRange(mutated);
                                Log.Information(
                                    "Calculated game {gameGeneration} for distribution {distribution} and experiment # {experiment}",
                                    generation, currentDistribution, currentExperiment);
                            }

                        }

                        var results = new WorkerResults()
                        {
                            Strategies = strats,
                            Tree = tree,
                            CurrentDistribution = currentDistribution,
                            CurrentExperiment = currentExperiment,
                            Generation = request.GenCount,
                        };
                        await channelWriter.WriteAsync(results);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Exception {ex}", ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error("Unable to WRITE execution of research {researchGuid}. Exception {ex}", researchGuid, ex);
            }
            finally
            {
                Log.Information("WRITE completed of research {researchGuid}", researchGuid);
                writer.Complete();
            }
        });

        Task.Factory.StartNew(async () =>
        {

            try
            {
                await using var asyncScope = _serviceProvider.CreateAsyncScope();
                await using var context = asyncScope.ServiceProvider.GetRequiredService<ApplicationContext>();
                var research = await context.Researches.FirstAsync(x => x.Guid == researchGuid);

                await foreach (var record in reader.ReadAllAsync())
                {
                    Log.Information("Begining reading of experiment {experiment} of distribution {distribution}",
                        record.CurrentExperiment, record.CurrentDistribution);

                    var generationResults = new GenerationResults()
                    {
                        Strategies = record.Strategies.Select(x => new DAL.Entities.ConditionalStrategy()
                        {
                            PatternName = x.Pattern.GetType().Name,
                            PatternCoefs = x.Pattern.Coeffs.Select(x => x.Value).ToArray(),
                            Id = x.Id,
                            Name = x.Name
                        }).ToList()
                    };

                    research.GameResults.Add(new()
                    {
                        CurrentDistribution = record.CurrentDistribution,
                        CurrentExperiment = record.CurrentExperiment,
                        CurrentGeneration = record.Generation,
                        GenerationResults = generationResults
                    });


                    generationResults.Tree = record.Tree.RawMap.Select(x => x.Value.Select(xx =>
                        new GenerationTreeNode()
                        {
                            Results = xx.Value.Select(xxx => xxx.Value).ToList(),
                            Strategy1 = generationResults.Strategies.First(xxx => xxx.Id == x.Key.Id),
                            Strategy2 = generationResults.Strategies.First(xxx => xxx.Id == xx.Key.Id),
                        })).SelectMany(x => x).ToList();

                    context.Update(research);
                    await context.SaveChangesAsync();
                    Log.Information("Done reading of experiment {experiment} of distribution {distribution}",
                        record.CurrentExperiment, record.CurrentDistribution);
                }

                research.Status = ResearchCompletionStatus.Completed;
                context.Update(research);
                await context.SaveChangesAsync();
            }
            catch (OperationCanceledException oce)
            {

            }
            catch (Exception ex)
            {
                Serilog.Log.Error("Unable to READ execution of research {researchGuid}. Exception {ex}",
                    researchGuid, ex);
            }

        });

        return new()
        {
            ResearchGuid = researchGuid,
            CurrentCountOfGames = 0,
            TotalCountOfGames = totalGamesCount,
            IsCompleted = false,
            IsDropped = false
        };
    }

    public async Task<CompletedResearchResponseModel?> GetResearch(Guid guid)
    {
        var research = await _context.Researches.AsNoTracking()
            .Include(research => research.GameResults)
            .ThenInclude(x => x.GenerationResults)
            .ThenInclude(x => x.Strategies)
            .Include(x => x.GameResults)
            .ThenInclude(x => x.GenerationResults)
            .ThenInclude(x => x.Tree)
            .ThenInclude(x => x.Strategy1)
            .Include(x => x.GameResults)
            .ThenInclude(x => x.GenerationResults)
            .ThenInclude(x => x.Tree)
            .ThenInclude(x => x.Strategy2)
            .FirstOrDefaultAsync(x => x.Guid == guid);

        if (research is null) return null;

        var result = new CompletedResearchResponseModel()
        {
            CurrentCountOfGames = await _context.Researches.AsNoTracking().Where(x => x.Guid == guid).Select(x => x.GameResults).CountAsync(),
            IsCompleted = research.Status == ResearchCompletionStatus.Completed,
            IsDropped = research.Status == ResearchCompletionStatus.Dropped,
            TotalCountOfGames = research.TotalGamesCount,
            ResearchGuid = research.Guid
        };

        if (result is { IsCompleted: true })
        {
            result.Results = research.GameResults.GroupBy(x => x.CurrentDistribution).Select(x =>
            {
                return (new MemeSingleGenerationResponseModel()
                {
                    GameResult = new()
                    {
                        Strats = x.Select(xx => xx.GenerationResults.Strategies.Select(xxx =>
                            new ConditionalStrategyModel()
                            {
                                Pattern = new PatternModel()
                                {
                                    Coeffs = xxx.PatternCoefs,
                                    Name = xxx.PatternName
                                },
                                Id = xxx.Id,
                                Name = xxx.Name
                            })).SelectMany(x => x).ToList(),
                        Result = new()
                        {
                            Map = x.Select(xx => xx.GenerationResults.Tree
                                    .Select(xxx => (
                                        s1: new TreeId()
                                        {
                                            Id = xxx.Strategy1.Id,
                                            Name = xxx.Strategy1.Name
                                        },
                                        s2: new TreeId()
                                        {
                                            Id = xxx.Strategy2.Id,
                                            Name = xxx.Strategy2.Name
                                        },
                                        results: xxx.Results.Select((r, i) => (i, r)).ToDictionary(x => x.i, x => x.r)
                                        )
                                    ))
                                 .SelectMany(xx => xx)
                                 .DistinctBy(x => new { x.s1, x.s2 })
                                 .GroupBy(xx => xx.s1)
                                 .Select(xx => (xx, xx.ToDictionary(xxx => xxx.s2, xxx => xxx.results).Select(x => x).ToList()))
                                 .ToDictionary(xx => xx.xx.Key, xx => xx.Item2).Select(xx => xx).ToList()
                        }

                    }
                }, x.Key);
            }).ToDictionary(x => x.Key, x => x.Item1).Select(x => x).ToList();
        }

        return result;
    }
}

public class WorkerResults
{
    public List<ConditionalStrategy> Strategies { get; set; }
    public TreeGameRunnerResult Tree { get; set; }

    public decimal CurrentDistribution { get; set; }
    public int CurrentExperiment { get; set; }
    public int Generation { get; set; }
}