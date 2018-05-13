using System.Collections.Generic;
using System.Threading.Tasks;

namespace PgSqlVsMsSql
{
    public abstract class SqlTesterBase
    {
        protected static IReadOnlyCollection<Person> FiftyKPerson = TestData.GenerateTestData(50000);
        protected static IReadOnlyCollection<Person> FiftyHundredKPerson = TestData.GenerateTestData(500000);

        public abstract void SetUp();
        public abstract Task RunTestAsync();
    }
}
