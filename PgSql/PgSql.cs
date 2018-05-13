using BenchmarkDotNet.Attributes;
using Npgsql;
using System.IO;
using System.Threading.Tasks;

namespace PgSqlVsMsSql.PgSql
{
    public class PgSql : SqlTesterBase
    {
        private string SchemaScript => File.ReadAllText(@".\PgSql\PgSql.Schema.sql");
        private string DbCreationScript => File.ReadAllText(@".\PgSql\PgSql.Creation.sql");

        [GlobalSetup]
        public override void SetUp()
        {
            CreateDb();
            CreateSchema();
            InsertTestData();
        }

        private void CreateDb()
        {
            using (var conn = GetDbConnection("postgres"))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = DbCreationScript;
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateSchema()
        {
            using (var conn = GetDbConnection("RaceDb"))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = SchemaScript;
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertTestData()
        {
            using (var conn = GetDbConnection("RaceDb"))
            {
                conn.Open();
                using (var bulkCopy = conn.BeginBinaryImport("COPY persons (lastname, firstname, addressline1, addressline2, city, zipcode) FROM STDIN BINARY"))
                {
                    foreach (var p in FiftyHundredKPerson)
                    {
                        bulkCopy.WriteRow(p.LastName, p.FirstName, p.AddressLine1, p.AddressLine2, p.City, p.ZipCode);
                    }
                }
            }
        }

        [Benchmark]
        public override async Task RunTestAsync()
        {
            using (var conn = GetDbConnection("RaceDb"))
            using (var cmd = conn.CreateCommand())
            {
                await conn.OpenAsync();
                cmd.CommandText = "SELECT * FROM persons WHERE FirstName = @p1";
                cmd.Parameters.AddWithValue("@p1", "Michael");
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        reader.GetInt32(0);
                    }
                }
            }
        }

        public void TearDown()
        {
            using (var conn = GetDbConnection("postgres"))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
SELECT pid, pg_terminate_backend(pid) 
FROM pg_stat_activity 
WHERE datname = @p0 AND pid <> pg_backend_pid();

DROP DATABASE IF EXISTS @p0";
                cmd.Parameters.AddWithValue("@p0", "RaceDb");
                cmd.ExecuteNonQuery();
            }
        }

        private NpgsqlConnection GetDbConnection(string db) => new NpgsqlConnection($"Server=localhost;Database={db};Port=5432;Uid=postgres;Pwd=sorrygithub");
    }
}
