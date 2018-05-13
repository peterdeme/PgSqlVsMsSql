# PostgresSQL vs MS SQL full table scan perf test on .NET Core 2.0.7

Benchmarked with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library.
Th benchmark basicly issues a simple `SELECT * FROM Persons WHERE LastName = 'Michael'` to the servers.
Tested on 500.000 rows.

### Versions
**PostgresSQL server**: 10.4, compiled by Visual C++ build 1800, 64-bit
**SQL Server**: Microsoft SQL Server 2014 - 12.0.2269.0 (X64)

## MS SQL Result

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.16299.125 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-5500U CPU 2.40GHz (Broadwell), 1 CPU, 4 logical and 2 physical cores
Frequency=2338337 Hz, Resolution=427.6544 ns, Timer=TSC
.NET Core SDK=2.1.200
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT


```
|       Method |     Mean |    Error |   StdDev |
|------------- |---------:|---------:|---------:|
| RunTestAsync | 112.8 ms | 1.694 ms | 1.584 ms |

## PostgreSQL Result

``` ini

BenchmarkDotNet=v0.10.14, OS=Windows 10.0.16299.125 (1709/FallCreatorsUpdate/Redstone3)
Intel Core i7-5500U CPU 2.40GHz (Broadwell), 1 CPU, 4 logical and 2 physical cores
Frequency=2338337 Hz, Resolution=427.6544 ns, Timer=TSC
.NET Core SDK=2.1.200
  [Host]     : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.7 (CoreCLR 4.6.26328.01, CoreFX 4.6.26403.03), 64bit RyuJIT


```
|       Method |     Mean |    Error |   StdDev |
|------------- |---------:|---------:|---------:|
| RunTestAsync | 62.16 ms | 1.236 ms | 1.214 ms |
