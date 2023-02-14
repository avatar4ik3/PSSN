``` ini

BenchmarkDotNet=v0.13.4, OS=Windows 11 (10.0.22621.1105)
Intel Core i5-8300H CPU 2.30GHz (Coffee Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=7.0.101
  [Host] : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX2

Job=MediumRun  Toolchain=InProcessNoEmitToolchain  IterationCount=15  
LaunchCount=2  WarmupCount=10  

```
|   Method |    Mean |    Error |   StdDev |  Median | Rank |       Gen0 |       Gen1 |       Gen2 | Allocated |
|--------- |--------:|---------:|---------:|--------:|-----:|-----------:|-----------:|-----------:|----------:|
|  Default | 1.448 s | 0.1204 s | 0.1688 s | 1.434 s |    1 | 99000.0000 | 51000.0000 | 50000.0000 | 310.34 MB |
| NoToList | 1.630 s | 0.1910 s | 0.2739 s | 1.459 s |    1 | 99000.0000 | 51000.0000 | 50000.0000 | 310.13 MB |
