using System;
using Xunit;

namespace AnonymousTypeTests
{
    public class AnonymousTypesShould
    {
        [Fact]
        public void StoreValuesSuppliedByObjectInitializer()
        {
            var person = new
            {
                Name = "Bob",
                Age = 52,
                IsEmployed = true
            };

            // illegal - read only
            // person.Name = "Susan";

            Assert.Equal("Bob", person.Name);
            Assert.Equal(52, person.Age);
            Assert.True(person.IsEmployed);
        }
    }
}
