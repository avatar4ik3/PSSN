using PSSN.Api.Model;
using PSSN.Api.ServiceInterfaces;
using PSSN.Core.Strategies;

namespace PSSN.Api.Services;

public sealed class ParserService : IParserService
{
    private readonly ILogger<ParserService> _logger;

    public ParserService(ILogger<ParserService> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<ResultVector>> GetVectors()
    {
        var lines = await ReadAllLines("vectors.txt");

        var strategies = lines[0].Split()
            .Where(str => string.IsNullOrWhiteSpace(str) is false && string.IsNullOrWhiteSpace(str) is false)
            .Select(x => new ConditionalStrategy() { Name = x }).ToList();

        _logger.LogInformation("Method {MethodName} started", nameof(GetVectors));

        var result = new List<ResultVector>();

        try
        {
            for (var i = 1; i < lines.Length - 1; i++)
            {
                var vector = new ResultVector
                {
                    Stage = i,
                    Vector = new Dictionary<IStrategy, double>()
                };

                var values = lines[i]
                    .Split()
                    .Skip(1)
                    .Select(double.Parse)
                    .ToList();

                for (var column = 0; column < strategies.Count; ++column)
                    vector.Vector.Add(strategies[column], values[column]);

                result.Add(vector);
            }
        }
        catch (Exception e)
        {
            _logger.LogError("An error was occured {Message}", e.Message);
        }

        return result;
    }

    public async Task<string[]> ReadAllLines(string file)
    {
        using var reader = File.OpenText(file);
        var fileText = await reader.ReadToEndAsync();
        return fileText.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
    }
}