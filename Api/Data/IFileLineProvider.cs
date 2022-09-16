namespace PSSN.Api.Data;

public interface IFileLineProvider
{
    public Task<string[]> ReadAllLines(string file){
        return File.ReadAllLinesAsync(file);
    }
}  