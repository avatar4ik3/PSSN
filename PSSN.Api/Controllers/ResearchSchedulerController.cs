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
    public ActionResult GetScheduledResearch([FromRoute] Guid researchGuid)
    {
        throw new NotImplementedException();
    }
}


public class ScheduleResearchRequestModel
{
    public int StrategiesCount { get; set; }
    public decimal DistributionStep { get; set; }
    public double[][]? Ro { get; set; }
    public int CountOfExperiments { get; set; }
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

public class CompletedResearchResponseModel : List<MemeSingleGenerationResponseModel>;

public class ScheduleResearchRunner(IGameRunner _gameRunner, IServiceProvider _serviceProvider, ApplicationContext _context)
{
    public async Task<SchedulerResearchResponseModel> ScheduleResearch(ScheduleResearchRequestModel request)
    {
        var countOfDistributions = (int)Math.Ceiling(1 / request.DistributionStep);
        var totalGamesCount = countOfDistributions * request.GenCount * request.CountOfExperiments;

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
            TotalGamesCount = totalGamesCount
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
                var tasks = Enumerable.Range(0, countOfDistributions + 1)
                    .Select(x => x * request.DistributionStep)
                    .SelectMany(x =>
                        Enumerable.Range(1, request.CountOfExperiments)
                            .Select(xx => RunSingleExperiment(x, xx, container, writer)))
                    .ToArray();
                await Task.WhenAll(tasks);



                async Task RunSingleExperiment(decimal currentDistribution, int currentExperiment, PatternsContainer patternsContainer,
                    ChannelWriter<WorkerResults> channelWriter)
                {
                    try
                    {

                        Log.Information("Started experiment {experiment} for distribution {distribution}", currentExperiment,
                            currentDistribution);
                        var strats = new List<ConditionalStrategy>(request.StrategiesCount);
                        var newPopulation = ConditionalStrategyBuilder.RandomMemes(Random.Shared, (double)currentDistribution, request.StrategiesCount, request.GenCount, patternsContainer).ToList();
                        TreeGameRunnerResult tree = null!;

                        foreach (var generation in Enumerable.Range(1, request.GenCount))
                        {
                            strats = newPopulation.Select(x => x).ToList();
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
                using var cts = new CancellationTokenSource();
                reader.Completion.ContinueWith(x => cts.Cancel(), TaskScheduler.Current);

                while (await reader.WaitToReadAsync(cts.Token))
                {
                    var record = await reader.ReadAsync(cts.Token);
                    Log.Information("Begining reading of experiment {experiment} of distribution {distribution}", record.CurrentExperiment, record.CurrentDistribution);

                    var generationResults = new GenerationResults()
                    {
                        Strategies = record.Strategies.Select(x => new DAL.Entities.ConditionalStrategy()
                        {
                            PatternName = x.GetType().Name,
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
                    await context.SaveChangesAsync(cts.Token);

                }

                research.Status = ResearchCompletionStatus.Completed;
                context.Update(research);
                await context.SaveChangesAsync(cts.Token);
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
}

public class WorkerResults
{
    public List<ConditionalStrategy> Strategies { get; set; }
    public TreeGameRunnerResult Tree { get; set; }

    public decimal CurrentDistribution { get; set; }
    public int CurrentExperiment { get; set; }
    public int Generation { get; set; }
}