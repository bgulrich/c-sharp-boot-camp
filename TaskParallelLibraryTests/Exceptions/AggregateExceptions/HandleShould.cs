using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPLTests.Exceptions.AggregateExceptions
{
    public class HandleShould
    {
        [Fact]
        public void ThrowNewAggregateExceptionContainingUnhandledInnerExceptions()
        {
            var exception1 = new Exception("1");
            var exception2 = new Exception("2");

            var task1 = Task.Factory.StartNew<bool>(() => throw exception1);
            var task2 = Task.Factory.StartNew<bool>(() => throw exception2);

            var compositeTask = Task.WhenAll(task1, task2);

            while (!compositeTask.IsCompleted) { }

            var aggException = Assert.Throws<AggregateException>(() =>
            {
                try
                {
                    var result = compositeTask.Result;
                }
                catch(AggregateException aggEx)
                {
                    // add each to our list for verification and mark handled
                    aggEx.Handle(ex =>
                    {
                        var handled = ex.Message == "1";
                        return handled;
                    });
                }
            });

            Assert.Equal(1, aggException.InnerExceptions.Count);
            // only expect unhandled inner exception (2) here
            Assert.Equal("2", aggException.InnerExceptions.Single().Message);
        }

        [Fact]
        public void NotThrowExceptionIfAllInnerExceptionsAreHandled()
        {
            var exception1 = new Exception("1");
            var exception2 = new Exception("2");

            var task1 = Task.Factory.StartNew<bool>(() => throw exception1);
            var task2 = Task.Factory.StartNew<bool>(() => throw exception2);

            var compositeTask = Task.WhenAll(task1, task2);

            while (!compositeTask.IsCompleted) { }

            var handled = new List<Exception>();

            try
            {
                var result = compositeTask.Result;
            }
            catch (AggregateException aggEx)
            {
                // only 
                aggEx.Handle(ex =>
                {
                    handled.Add(ex);
                    return true;
                });
            }

            Assert.Equal(2, handled.Count);
            // same exceptions (ignore order)?
            Assert.Equal(2, handled.Union(new[] { exception1, exception2 }).Count());
        }
    }
}
