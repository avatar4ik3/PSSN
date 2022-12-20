// wrong way


// using Moq;
//
// using PSSN.Api.Model;
// using PSSN.Api.ServiceInterfaces;
// using PSSN.Api.Services;
// using PSSN.Core.Strategies;
//
// namespace PSSN.Api.Tests;
//
// public class TestRepositoryTest
// {
//     private readonly Mock<IFileLineProvider> _provider = new();
//
//     public TestRepositoryTest()
//     {
//         _provider.Setup(x => x.ReadAllLines(It.IsAny<string>())).Returns(Task.FromResult(new[]
//         {
//             "C	CTT	CRTT	CD	DC	DTT	DRTT	DD",
//             "0	0,125	0,125	0,125	0,125	0,125	0,125	0,125	0,125",
//             "1	0,12406015	0,124530075	0,124530075	0,125	0,125	0,125469925	0,125469925	0,12593985"
//         }));
//     }
//
//     [Fact]
//     public async Task Repo_Count_ShouldBe2()
//     {
//         var repo = new ParserService(string.Empty, _provider.Object);
//
//         var actual = await repo.GetVectors();
//
//         Assert.Equal(actual.Count(), 2);
//     }
//
//     [Fact]
//     public async Task Repo_Values_AsExpected()
//     {
//         var repo = new ParserService(string.Empty, _provider.Object);
//
//         var actual = (await repo.GetVectors()).ToList();
//
//         var expected = new List<ResultVector>();
//
//         expected.Add(new ResultVector
//         {
//             Stage = 0,
//             Vector = new Dictionary<IStrategy, double>
//             {
//                 {new EmptyStrategy {Name = "C"}, 0.125},
//                 {new EmptyStrategy {Name = "CTT"}, 0.125},
//                 {new EmptyStrategy {Name = "CRTT"}, 0.125},
//                 {new EmptyStrategy {Name = "CD"}, 0.125},
//                 {new EmptyStrategy {Name = "DC"}, 0.125},
//                 {new EmptyStrategy {Name = "DTT"}, 0.125},
//                 {new EmptyStrategy {Name = "DRTT"}, 0.125},
//                 {new EmptyStrategy {Name = "DD"}, 0.125}
//             }
//         });
//
//         expected.Add(new ResultVector
//         {
//             Stage = 1,
//             Vector = new Dictionary<IStrategy, double>
//             {
//                 {new EmptyStrategy {Name = "C"}, 0.12406015},
//                 {new EmptyStrategy {Name = "CTT"}, 0.124530075},
//                 {new EmptyStrategy {Name = "CRTT"}, 0.124530075},
//                 {new EmptyStrategy {Name = "CD"}, 0.125},
//                 {new EmptyStrategy {Name = "DC"}, 0.125},
//                 {new EmptyStrategy {Name = "DTT"}, 0.125469925},
//                 {new EmptyStrategy {Name = "DRTT"}, 0.125469925},
//                 {new EmptyStrategy {Name = "DD"}, 0.12593985}
//             }
//         });
//         for (var i = 0; i < 2; ++i) Assert.Equal(expected[i].Vector, actual[i].Vector);
//     }
//
//     [Fact]
//     public async Task Repository_ReadsFile_Succesfull()
//     {
//         // Given
//         var repo = new ParserService("vectors.txt", new FileLineProvider());
//         // When
//         var actual = await repo.GetVectors();
//         // Then
//     }
// }