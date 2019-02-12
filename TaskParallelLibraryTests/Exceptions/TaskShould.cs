using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPL.ExceptionTests
{
    public class TaskShould
    {
        [Fact]
        public void EnterFaultedStatusWhenExceptionSet()
        {
            var exception = new Exception("Some Exception");

            var task = Task.Factory.StartNew(() => throw exception);

            while (!task.IsCompleted) { }

            Assert.Equal(TaskStatus.Faulted, task.Status);

            // observe the exception
            try { task.Wait(); } catch { }
        }

        [Fact]
        public void ThrowWrappingAggregagteExceptionWhenWaited()
        {
            var exception = new Exception("Some Exception");

            var task = Task.Factory.StartNew(() => throw exception);

            var aggException = Assert.Throws<AggregateException>(() => task.Wait());
            Assert.Equal(exception, aggException.InnerException);
        }

        [Fact]
        public void ThrowWrappingAggregagteExceptionWhenResultAccessed()
        {
            var exception = new Exception("Some Exception");

            var task = Task.Factory.StartNew<bool>(() => throw exception);

            while (!task.IsCompleted) { }

            var aggException = Assert.Throws<AggregateException>(() => { return task.Result; });
            Assert.Equal(exception, aggException.InnerException);
        }

        [Fact]
        public async Task RethrowExceptionWhenAwaited()
        {
            var exception = new Exception("Some Exception");

            var task = Task.Factory.StartNew(() => throw exception);

            var thrownException = await Assert.ThrowsAsync<Exception>(() => task);
            Assert.Equal(exception, thrownException);
        }
    }
}
