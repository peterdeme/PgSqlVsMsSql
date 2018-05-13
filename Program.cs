using BenchmarkDotNet.Running;
using System;

namespace PgSqlVsMsSql
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<MsSql.MsSql>();
            BenchmarkRunner.Run<PgSql.PgSql>();

            Console.WriteLine("Done...");
            Console.ReadKey();
        }
    }
}
