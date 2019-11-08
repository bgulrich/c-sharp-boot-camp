using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TPLTests.ThreadBasedTasks
{
    public class TaskShould
    {
        [Fact]
        public async Task ProgressThroughStatesOfExecution()
        {
            var counter = 0;

            var t = new Task<int>(() =>
            {
                for (var i = 0; i < 20; ++i)
                {
                    Task.Delay(100).Wait();
                    ++counter;
                }

                return counter;
            });

            Assert.Equal(TaskStatus.Created, t.Status);

            t.Start();

            Assert.Equal(TaskStatus.WaitingToRun, t.Status);

            while (t.Status == TaskStatus.WaitingToRun)
            {
                await Task.Delay(100);
            }

            Assert.Equal(TaskStatus.Running, t.Status);

            while (t.Status == TaskStatus.Running)
            {
                await Task.Delay(100);
            }

            Assert.Equal(TaskStatus.RanToCompletion, t.Status);

            Assert.Equal(20, counter);
        }
    }
}
