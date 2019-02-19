using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BuiltInTypesTests
{
    public class ObjectsShould
    {
        [Fact]
        public void AcceptAnyValuesOfAnyDataType() //object a)
        {
            object a;

            // Boolean
            a = true;
            bool boolObject = true;
            Assert.Equal(boolObject, a);

            // Byte
            a = (byte)12;
            byte byteObject = 12;
            Assert.Equal(byteObject, a);

            //SByte
            a = (sbyte)-12;
            sbyte sbyteObject = -12;
            Assert.Equal(sbyteObject, a);

            // Char
            a = '\u00A5';
            char charObject = '\u00A5';
            Assert.Equal(charObject, a);

            // Decimal
            a = 123.456789m;
            decimal decimalObject = 123.456789m;
            Assert.Equal(decimalObject, a);

            // Double
            a = 1.2E+3;
            double doubleObject = 1.2E+3;
            Assert.Equal(doubleObject, a);

            // Float
            a = 1.23F;
            float floatObject = 1.23F;
            Assert.Equal(floatObject, a);

            // Int
            a = 123;
            int intObject = 123;
            Assert.Equal(intObject, a);

            // String
            a = "Objects are dope.";
            string stringObject = "Objects are dope.";
            Assert.Equal(stringObject, a);
        }
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public Person Clone()
            {
                return (Person)MemberwiseClone();
            }
        }

        [Fact]
        public void AllowCreatingShallowCopyofCurrentObject()
        {
            Person p1 = new Person();
            p1.Name = "Sam";
            p1.Age = 74;

            Person p2 = p1.Clone();

            Assert.Equal(p1.Age, p2.Age);
            Assert.Equal(p1.Name, p2.Name);

            // Changing details of p1 should not affect p2
            p1.Name = "Walton";
            p1.Age = 56;

            Assert.NotEqual(p1.Age, p2.Age);
            Assert.NotEqual(p1.Name, p2.Name);
        }

    }
}
