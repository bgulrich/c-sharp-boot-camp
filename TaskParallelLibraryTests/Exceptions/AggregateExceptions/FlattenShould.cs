using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPLTests.Exceptions.AggregateExceptions
{
    public class FlattenShould
    {
        [Fact]
        public void FlattenNestedAggregateExceptions()
        {
            var exception1 = new Exception("1");
            var exception2 = new Exception("2");
            var exception3 = new Exception("3");

            var task1 = Task.Factory.StartNew<bool>(() => throw exception1);
            var task2 = Task.Factory.StartNew<bool>(() => throw exception2);
            var task3 = Task.Factory.StartNew<bool>(() => throw exception3);

            // task with child tasks
            var compositeTask23 = Task.Run(() =>
            {
                Task.WaitAll(task2, task3);
            });

            var flattenedAggException = Assert.Throws<AggregateException>(() =>
            {
                try
                {
                    Task.WaitAll(compositeTask23, task1);
                }
                catch (AggregateException aggEx)
                {
                    Assert.Equal(2, aggEx.InnerExceptions.Count);
                    // expecting an inner aggregate exception for the 2,3 tasks
                    var innerAggEx = aggEx.InnerExceptions.Single(ex => ex is AggregateException) as AggregateException;
                    Assert.Equal(2, innerAggEx.InnerExceptions.Count);

                    throw aggEx.Flatten();
                }
            });

            Assert.Equal(3, flattenedAggException.InnerExceptions.Count);
            Assert.Equal(3, flattenedAggException.InnerExceptions.Union(new[] { exception1, exception2, exception3 }).Count());
        }
    }
}
