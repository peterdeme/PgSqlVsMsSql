using Bogus;
using System.Collections.Generic;

namespace PgSqlVsMsSql
{
    public class TestData
    {
        public static IReadOnlyCollection<Person> GenerateTestData(int numberOfTestData = 1000000)
        {
            var faker = new Faker<Person>()
                .RuleFor(x => x.PersonID, f => null)
                .RuleFor(x => x.FirstName, f => f.Name.FirstName())
                .RuleFor(x => x.LastName, f => f.Name.LastName())
                .RuleFor(x => x.AddressLine1, f => f.Address.StreetAddress())
                .RuleFor(x => x.AddressLine2, f => f.Address.SecondaryAddress())
                .RuleFor(x => x.City, f => f.Address.City())
                .RuleFor(x => x.ZipCode, f => f.Address.ZipCode());

            return faker.Generate(numberOfTestData);
        }
    }

    public class Person
    {
        public string PersonID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}
