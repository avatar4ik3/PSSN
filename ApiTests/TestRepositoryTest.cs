using Moq;
using PSSN.Api.Data;
using PSSN.Api.Model;

namespace PSSN.ApiTests;

[TestClass]
public class TestRepositoryTest
{
    private readonly Mock<IFileLineProvider> _provider = new Mock<IFileLineProvider>();

    [TestInitialize()]
    public void Init()
    {
        _provider.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(Task.FromResult<string[]>(new string[]{
                "C	CTT	CRTT	CD	DC	DTT	DRTT	DD",
                "0	0,125	0,125	0,125	0,125	0,125	0,125	0,125	0,125",
                "1	0,12406015	0,124530075	0,124530075	0,125	0,125	0,125469925	0,125469925	0,12593985",
        }));
    }
    [TestMethod]
    public async Task Repo_Count_ShouldBe2()
    {

        var repo = new TestRepository(String.Empty, _provider.Object);

        var actual = await repo.GetVectors();

        Assert.AreEqual(actual.Count(), 2);
    }

    [TestMethod]
    public async Task Repo_Values_AsExpected()
    {

        var repo = new TestRepository(String.Empty, _provider.Object);

        var actual = (await repo.GetVectors()).ToList();

        var expected = new List<ResultVector>();

        expected.Add(new ResultVector
        {
            Stage = 0,
            Vector = new Dictionary<Strategy, double>{
            {new Strategy(){Name = "C"},0.125},
            {new Strategy(){Name = "CTT"},0.125},
            {new Strategy(){Name = "CRTT"},0.125},
            {new Strategy(){Name = "CD"},0.125},
            {new Strategy(){Name = "DC"},0.125},
            {new Strategy(){Name = "DTT"},0.125},
            {new Strategy(){Name = "DRTT"},0.125},
            {new Strategy(){Name = "DD"},0.125}
        }
        });

        expected.Add(new ResultVector
        {
            Stage = 1,
            Vector = new Dictionary<Strategy, double>{
            {new Strategy(){Name = "C"},0.12406015},
            {new Strategy(){Name = "CTT"},0.124530075},
            {new Strategy(){Name = "CRTT"},0.124530075},
            {new Strategy(){Name = "CD"},0.125},
            {new Strategy(){Name = "DC"},0.125},
            {new Strategy(){Name = "DTT"},0.125469925},
            {new Strategy(){Name = "DRTT"}, 0.125469925},
            {new Strategy(){Name = "DD"},0.12593985}
        }
        });
        for (int i = 0; i < 2; ++i)
        {
            CollectionAssert.AreEquivalent(expected[i].Vector, actual[i].Vector);
        }
    }
}