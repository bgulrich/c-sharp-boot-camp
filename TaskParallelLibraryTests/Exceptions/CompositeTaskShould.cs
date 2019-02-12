using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPL.ExceptionTests
{
    public class CompositeTaskShould
    {
        [Fact]
        public void EnterFaultedStatusWhenExceptionSet()
        {
            var exception1 = new Exception("1");
            var exception2 = new Exception("2");

            var task1 = Task.Factory.StartNew(() => throw exception1);
            var task2 = Task.Factory.StartNew(() => throw exception2);

            var compositeTask = Task.WhenAll(task1, task2);

            while (!compositeTask.IsCompleted) { }

            Assert.Equal(TaskStatus.Faulted, compositeTask.Status);

            // observe the exception
            try { compositeTask.Wait(); } catch { }
        }

        [Fact]
        public void ThrowWrappingAggregagteExceptionWhenWaited()
        {
            var exception1 = new Exception("1");
            var exception2 = new Exception("2");

            var task1 = Task.Factory.StartNew(() => throw exception1);
            var task2 = Task.Factory.StartNew(() => throw exception2);

            var compositeTask = Task.WhenAll(task1, task2);

            var aggException = Assert.Throws<AggregateException>(() => compositeTask.Wait());
            Assert.Equal(2, aggException.InnerExceptions.Count);
            // same exceptions (ignore order)?
            Assert.Equal(2, aggException.InnerExceptions.Union(new[] { exception1, exception2 }).Count());
        }

        [Fact]
        public void ThrowWrappingAggregagteExceptionWhenResultAccessed()
        {
            var exception1 = new Exception("1");
            var exception2 = new Exception("2");

            var task1 = Task.Factory.StartNew<bool>(() => throw exception1);
            var task2 = Task.Factory.StartNew<bool>(() => throw exception2);

            var compositeTask = Task.WhenAll(task1, task2);

            while (!compositeTask.IsCompleted) { }

            var aggException = Assert.Throws<AggregateException>(() => { return compositeTask.Result; });
            Assert.Equal(2, aggException.InnerExceptions.Count);
            // same exceptions (ignore order)?
            Assert.Equal(2, aggException.InnerExceptions.Union(new[] { exception1, exception2 }).Count());
        }

        [Fact]
        public async Task RethrowOneOfExceptionWhenAwaited()
        {
            var exception1 = new Exception("1");
            var exception2 = new Exception("2");

            var task1 = Task.Factory.StartNew(() => throw exception1);
            var task2 = Task.Factory.StartNew(() => throw exception2);

            var compositeTask = Task.WhenAll(task1, task2);

            var thrownException = await Assert.ThrowsAsync<Exception>(() => compositeTask);
            Assert.Contains(new[] { exception1, exception2 }, ex => ex == thrownException);
        }
    }
}
