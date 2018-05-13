using BenchmarkDotNet.Attributes;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace PgSqlVsMsSql.MsSql
{
    public class MsSql : SqlTesterBase
    {
        private string SchemaScript => File.ReadAllText(@".\MsSql\MsSql.Schema.sql");
        private string DbCreationScript => File.ReadAllText(@".\MsSql\MsSql.Creation.sql");

        [GlobalSetup]
        public override void SetUp()
        {
            CreateDb();
            CreateSchema();
            InsertTestData();
        }

        private void CreateDb()
        {
            using (var conn = GetSqlConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = DbCreationScript;
                cmd.ExecuteNonQuery();
            }
        }

        private void CreateSchema()
        {
            using (var conn = GetSqlConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = SchemaScript;
                cmd.ExecuteNonQuery();
            }
        }

        private void InsertTestData()
        {
            var dt = new DataTable { Columns = { "PersonID", "LastName", "FirstName", "AddressLine1", "AddressLine2", "City", "ZipCode" } };

            foreach (var p in FiftyHundredKPerson)
                dt.Rows.Add(p.PersonID, p.LastName, p.FirstName, p.AddressLine1, p.AddressLine2, p.City, p.ZipCode);

            using (var conn = GetSqlConnection())
            using (var bulkCopy = new SqlBulkCopy(conn) { BulkCopyTimeout = 60000 })
            {
                conn.Open();
                bulkCopy.DestinationTableName = "Persons";
                bulkCopy.WriteToServer(dt);
            }
        }

        [Benchmark]
        public override async Task RunTestAsync()
        {
            using (var conn = GetSqlConnection())
            using (var cmd = conn.CreateCommand())
            {
                await conn.OpenAsync();
                cmd.CommandText = "SELECT * FROM Persons WHERE FirstName = @p1";
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
            using (var conn = GetSqlConnection())
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    ALTER DATABASE [RaceDb] set single_user with rollback immediate;
                    DROP DATABASE [RaceDb]";
                cmd.ExecuteNonQuery();
            }
        }

        private SqlConnection GetSqlConnection() => new SqlConnection($"Server=DESKTOP-T0L53AP\\LOCAL_INSTANCE;Initial Catalog=RaceDb;Integrated Security=True;");
    }
}
