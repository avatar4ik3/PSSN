using PSSN.Api.Model;

namespace PSSN.Api.Data;

public sealed class TestRepository : ITestRepository
{
    private readonly string _file;
    private readonly IFileLineProvider _provider;

    public TestRepository(String file, IFileLineProvider provider)
    {
        this._file = file;
        this._provider = provider;
        // this._logger = logger;
    }
    ///Test Method. Takes values from provided test file in .txt extension
    ///Values must be classified in columns by strateg's names and lines must also be ordered by k-assending 
    public async Task<IEnumerable<ResultVector>> GetVectors()
    {

        var lines = await _provider.ReadAllLines(_file);
        var columnNames = lines[0].Split().Where(str => String.IsNullOrWhiteSpace(str) is false && String.IsNullOrWhiteSpace(str) is false).Select(x => new Strategy { Name = x }).ToList();

        Console.WriteLine("Started");
        var result = new List<ResultVector>();
        for (int i = 1; i < lines.Length; ++i)
        {
            Console.WriteLine($"Line {i}");
            //  _logger.LogDebug("Current Line {line}", i);
            var vector = new ResultVector() { Stage = i - 1, Vector = new() };
            var values = lines[i].Split().Skip(1).Select(x => Double.Parse(x)).ToList();
            for (int column = 0; column < columnNames.Count; ++column)
            {
                vector.Vector.Add(columnNames[column], values[column]);
            }
            //  _logger.LogDebug("Added Vector {vector}", vector);
            result.Add(vector);
        }

        return result;
    }
}