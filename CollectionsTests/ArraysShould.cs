using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CollectionTests
{
    public class ArraysShould
    {
        #region Helpers

        private interface ISomeInterface { }
        private class SomeClass : ISomeInterface { }
        private class SomeDerivedClass : SomeClass { }

        private struct SomeStruct : ISomeInterface { }

        #endregion

        #region Reference Type

        [Fact]
        public void BehaveAsAReferenceType()
        {
            var array1 = new int[] { 0, 1, 2, 3, 4, 5 };
            var array2 = array1;

            // same instance?
            Assert.Same(array2, array1);

            // modify array 2
            array2[4] = 12;

            // change affects array 1 (same instance)
            Assert.Equal(12, array1[4]);
        }

        #endregion

        #region Length Properties
        [Fact]
        public void ReportTheCorrectLength()
        {
            var length = 100;

            Assert.Equal(length, new int[length].Length);
        }

        [Fact]
        public void ReportTheCorrectLongLength()
        {
            var length = 100;

            Assert.Equal(length, new int[length].LongLength);
        }
        #endregion

        #region Indexing
        [Fact]
        public void ReturnElementsInZeroBasedIndexOrder()
        {
            var array = new int[] { 0, 1, 2, 3, 4 };

            for (int i = 0; i < array.Length; ++i)
                Assert.Equal(i, array[i]);
        }

        [Fact]
        public void ThrowOutOfRangeExceptionIfIndexedBeyondSize()
        {
            var array = new int[10];
            Assert.Throws<IndexOutOfRangeException>(() => { var y = array[10]; });
        }

        #endregion

        #region Enumerable

        [Fact]
        public void BeEnumerable()
        {
            var array = new int[] { 1, 2, 3 };

            #region enumerator implementation
            var enumerator = array.GetEnumerator();

            enumerator.MoveNext();
            var value = (int)enumerator.Current;

            Assert.Equal(array[0], value);

            int index = 1;

            while (enumerator.MoveNext())
            {
                Assert.Equal(array[index], enumerator.Current);
                ++index;
            }
            #endregion

            #region foreach implementation
            value = 1;

            foreach(var arrayElement in array)
            {
                Assert.Equal(value++, arrayElement);
            }

            #endregion
        }

        #endregion

        #region Replacing Elements
        [Fact]
        public void ReplaceElementsWithSquareBracketSyntax()
        {
            var array = new [] { 1, 2, 3 };

            Assert.Equal(2, array[1]);

            array[1] = 5;

            Assert.Equal(5, array[1]);
        }

        #endregion

        #region Initialization

        [Fact]
        public void InitializeElementsToDefaultValueOfType()
        {
            var intArray = new int[10];

            foreach (var integer in intArray)
                Assert.Equal(default(int), integer);

            var objectArray = new object[10];

            foreach (var obj in objectArray)
                Assert.Equal(default(object), obj);
        }

        #endregion

        #region Acceptable Values

        [Fact]
        public void AcceptDerivedValues()
        {
            var array = new SomeClass[5];
            array[3] = new SomeDerivedClass();
        }

        [Fact]
        public void AcceptImplementingValuesWhenArrayIsInterfaceType()
        {
            var array = new ISomeInterface[5];

            array[3] = new SomeDerivedClass();
            array[2] = new SomeClass();
            array[0] = new SomeStruct();
        }

        #endregion

        #region Type

        [Fact]
        public void BeDerivedDirectlyFromSystemArray()
        {
            var someClassArray = new SomeClass[0];

            var type = someClassArray.GetType();

            Assert.Equal(typeof(System.Array), type.BaseType);
            // interestingly name is just the class name + square brackets
            Assert.Contains("SomeClass[]", type.FullName);
        }

        #endregion

        #region Array Covariance

        [Fact]
        public void BeCastableToAnArrayOfBaseType()
        {
            var someDerivedTypeArray = new SomeDerivedClass[10];
            SomeClass[] someBaseTypeArray = someDerivedTypeArray;
        }

        [Fact]
        public void AllowMeToGetIntoTroubleWithArrayCovariance()
        {
            var someDerivedTypeArray = new SomeDerivedClass[10];
            object[] someBaseTypeArray = someDerivedTypeArray;

            Assert.ThrowsAny<ArrayTypeMismatchException>(() => { someBaseTypeArray[5] = 3; });
        }

        #endregion

        #region Instance Functionality

        [Fact]
        public void CopyValuesWithCopyToMethod()
        {
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var copy = new int[array.Length + 5];

            array.CopyTo(copy, 5);

            // equal content
            Assert.True(array.SequenceEqual(copy.Skip(5)));
        }

        [Fact]
        public void CopyValuesWithCloneMethod()
        {
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var clone = (int[])array.Clone();

            // different instances
            Assert.NotSame(array, clone);
            // but equal content
            Assert.True(array.SequenceEqual(clone));
        }

        #endregion

        #region Static Functionality
        [Fact]
        public void ResetToDefaultValuesWithClearMethod()
        {
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Array.Clear(array, 0, array.Length);

            Assert.DoesNotContain(array, i => i != default(int));
        }

        [Fact]
        public void ReverseArrayUsingReverseMethod()
        {
            var array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            Array.Reverse(array);

            for (int i = 0; i < array.Length; ++i)
                Assert.Equal(10 - i, array[i]);
        }
        #endregion

        #region ICollection

        [Fact]
        public void AllowMeToGetIntoTroubleWithICollection()
        {
            var array = new int[10];
            ICollection<int> collection = array;
            Assert.ThrowsAny<NotSupportedException>(() => { collection.Add(5); });

            // notice ICollection<T>.Add(T item) is implemented explicitly on the array, the below line of code won't work
            // array.Add(5);
        }

        [Fact]
        public void HelpMeAvoidTroubleWithICollection()
        {
            ICollection<int> collection = new int[10];

            if (collection.IsReadOnly)
                Console.WriteLine("Can't add to a read-only collection!");
            else
                collection.Add(5);
        }
        #endregion
    }
}
