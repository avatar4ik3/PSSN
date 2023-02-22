using AutoMapper;
using PSSN.Common.Model;
using PSSN.Common.Requests;
using PSSN.Common.Responses;
using PSSN.Core;
using PSSN.Core.Generators;
using PSSN.Core.Operators;
using PSSN.Core.Round;
using PSSN.Core.Strategies;

namespace PSSN.Benchmarks;

public static class HardEndpointImps
{
    //|  629.0 ms |  14.88 ms |  22.28 ms |    1 | 54000.0000 | 16000.0000 | 3000.0000 | 310.29 MB |

    public static GenerationResponse Solution_Default(GenerationRequest request, Mapper mapper, IGameRunner gameRunner,
        Random random)
    {
        var gen = new FilledStrategiesGenerator(
            request.Population,
            new SingleFilledStrategyGenerator(request.GenCount, random));

        var strats = gen
            .Generate()
            .ToList();

        var result = new GenerationResponse();

        foreach (var _ in ..request.Population)
        {
            //C_Count_2_Population * GenCount ::::: <- GenCount это GPR 
            var tree = gameRunner.Play(strats, request.Ro!, request.GenCount);

            result.Items.Add(new(
                mapper.Map<List<FilledStrategyModel>>(strats.Copy().ToList()),
                mapper.Map<ResultTree>(tree)));

            var newPopulation = new List<FilledStrategy>();

            var selectionOperator = new SelectionOperator<FilledStrategy>(request.SelectionGroupSize, tree, random);
            var crossingOverOperator =
                new BestScorePickerCrossingOverOperator(
                    request.CrossingCount, strats, tree);
            var mutationOperator = new MutationOperator(request.SwapChance, random);
            //INNER_C = Population / 2 + 1 если Populations нечетное
            foreach (var __ in ..(strats.Count / 2 + strats.Count % 2))
            {
                //INNER_C 
                var s1 = selectionOperator.Operate(strats);
                var s2 = selectionOperator.Operate(strats);

                var crossovers = crossingOverOperator.Operate(s1, s2);

                //INNER_C * Population
                var mutated = mutationOperator.Operate(crossovers);

                newPopulation.AddRange(mutated);
            }

            strats = newPopulation.Copy()
                .Zip(Enumerable.Range(0, newPopulation.Count()))
                .Select(x => new FilledStrategy(x.First.behaviours, x.Second.ToString()))
                .ToList();
        }

        return result;
    }

    //| 825.4 ms | 149.13 ms | 218.59 ms |    2 | 54000.0000 | 16000.0000 | 3000.0000 | 310.08 MB |

    public static GenerationResponse Solution_NoCopy_NoToList_NoBadAlloc(GenerationRequest request, Mapper mapper, IGameRunner gameRunner,
        Random random)
    {
        var gen = new FilledStrategiesGenerator(
            request.Population,
            new SingleFilledStrategyGenerator(request.GenCount, random));

        var strats = gen
            .Generate()
            .ToList();

        var result = new GenerationResponse();

        foreach (var _ in ..request.Population)
        {
            //C_Count_2_Population * GenCount ::::: <- GenCount это GPR 
            var tree = gameRunner.Play(strats, request.Ro!, request.GenCount);

            result.Items.Add(new(
                mapper.Map<List<FilledStrategyModel>>(strats),
                mapper.Map<ResultTree>(tree)));

            var newPopulation = new List<FilledStrategy>(strats.Count());

            var selectionOperator = new SelectionOperator<FilledStrategy>(request.SelectionGroupSize, tree, random);
            var crossingOverOperator =
                new BestScorePickerCrossingOverOperator(
                    request.CrossingCount, strats, tree);
            var mutationOperator = new MutationOperator(request.SwapChance, random);
            //INNER_C = Population / 2 + 1 если Populations нечетное
            foreach (var __ in ..(strats.Count() / 2 + strats.Count() % 2))
            {
                //INNER_C 
                var s1 = selectionOperator.Operate(strats);
                var s2 = selectionOperator.Operate(strats);

                var crossovers = crossingOverOperator.Operate(s1, s2);

                //INNER_C * Population
                var mutated = mutationOperator.Operate(crossovers);

                newPopulation.AddRange(mutated);
            }

            strats = newPopulation.Copy()
                .Zip(Enumerable.Range(0, newPopulation.Count()))
                .Select(x =>
                {
                    x.First.Name = x.Second.ToString();
                    return x.First;
                }).ToList();
        }

        return result;
    }

    public static GenerationResponse Solution_NoMapping(GenerationRequest request, IGameRunner gameRunner,
        Random random)
    {
        var gen = new FilledStrategiesGenerator(
            request.Population,
            new SingleFilledStrategyGenerator(request.GenCount, random));

        var strats = gen
            .Generate()
            .ToList();

        var result = new GenerationResponse();

        foreach (var _ in ..request.Population)
        {
            //C_Count_2_Population * GenCount ::::: <- GenCount это GPR 
            var tree = gameRunner.Play(strats, request.Ro!, request.GenCount);

            var newPopulation = new List<FilledStrategy>(strats.Count());

            var selectionOperator = new SelectionOperator<FilledStrategy>(request.SelectionGroupSize, tree, random);
            var crossingOverOperator =
                new BestScorePickerCrossingOverOperator(
                    request.CrossingCount, strats, tree);
            var mutationOperator = new MutationOperator(request.SwapChance, random);
            //INNER_C = Population / 2 + 1 если Populations нечетное
            foreach (var __ in ..(strats.Count() / 2 + strats.Count() % 2))
            {
                //INNER_C 
                var s1 = selectionOperator.Operate(strats);
                var s2 = selectionOperator.Operate(strats);

                var crossovers = crossingOverOperator.Operate(s1, s2);

                //INNER_C * Population
                var mutated = mutationOperator.Operate(crossovers);

                newPopulation.AddRange(mutated);
            }

            strats = newPopulation.Copy()
                .Zip(Enumerable.Range(0, newPopulation.Count()))
                .Select(x =>
                {
                    x.First.Name = x.Second.ToString();
                    return x.First;
                }).ToList();
        }

        return result;
    }

}