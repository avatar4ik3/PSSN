using PSSN.Api.Model;

namespace PSSN.Api.ServiceInterfaces;

public interface IParserService
{ 
    Task<IEnumerable<ResultVector>> GetVectors();
    Task<string[]> ReadAllLines(string file);
}