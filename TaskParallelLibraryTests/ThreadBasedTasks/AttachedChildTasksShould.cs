using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPLTests.ThreadBasedTasks
{
    public class AttachedChildTasksShould
    {
        [Fact]
        public async Task CauseParentTaskToWaitForCompletion()
        {
            var startTime = DateTime.Now;

            var parent = Task.Factory.StartNew(() =>
            {
                var attachedChild = Task.Factory.StartNew(() => Task.Delay(500).Wait(), TaskCreationOptions.AttachedToParent);
                var detachedChild = Task.Factory.StartNew(() => Task.Delay(1000).Wait());
                Task.Delay(100).Wait();
            });

            await parent;

            var duration_ms = (DateTime.Now - startTime).TotalMilliseconds;

            Assert.True(duration_ms > 400 && duration_ms < 600);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DriveParentStatusAndExceptions(bool childSuccess)
        {
            var childException = new Exception("test");

            var parent = Task.Factory.StartNew(() =>
            {
                var attachedChild = Task.Factory.StartNew(() =>
                {
                    Task.Delay(200).Wait();
                    if(!childSuccess)
                        throw childException;

                }, TaskCreationOptions.AttachedToParent);

                var detachedChild = Task.Factory.StartNew(() => Task.Delay(100).Wait());
                Task.Delay(100).Wait();
            });

            while (!parent.IsCompleted)
            {
                await Task.Delay(100);
            }

            Assert.Equal(childSuccess ? TaskStatus.RanToCompletion : TaskStatus.Faulted, parent.Status);

            if (!childSuccess)
            {
                var ex = await Assert.ThrowsAsync<AggregateException>(() => parent);

                Assert.Same(childException, ex.InnerException);
            }
        }
    }
}
