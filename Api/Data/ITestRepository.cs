using PSSN.Api.Model;

namespace PSSN.Api.Data;

public interface ITestRepository {
    public Task<IEnumerable<ResultVector>> GetVectors();
}