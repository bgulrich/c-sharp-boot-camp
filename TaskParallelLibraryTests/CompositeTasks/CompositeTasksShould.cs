using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace TPL.CompositeTaskTests
{
    public class CompositeTasksShould
    {
        #region When All Final Status
        [Fact]
        public void RunToCompletionIfNoTasksCanceledWhenUsingWhenAll()
        {
            var tcs1 = new TaskCompletionSource<object>();
            var tcs2 = new TaskCompletionSource<object>();
            var tcs3 = new TaskCompletionSource<object>();

            tcs1.SetResult(null);
            tcs2.SetResult(null);
            tcs3.SetResult(null);

            var comp = Task.WhenAll(tcs1.Task, tcs2.Task, tcs3.Task);
            Assert.Equal(TaskStatus.RanToCompletion, comp.Status);
        }

        [Fact]
        public void EnterCanceledStateIfOneTaskCanceledWhenUsingWhenAll()
        {
            var tcs1 = new TaskCompletionSource<object>();
            var tcs2 = new TaskCompletionSource<object>();
            var tcs3 = new TaskCompletionSource<object>();

            tcs1.SetResult(null);
            tcs2.SetCanceled();
            tcs3.SetResult(null);

            var comp = Task.WhenAll(tcs1.Task, tcs2.Task, tcs3.Task);
            Assert.Equal(TaskStatus.Canceled, comp.Status);
        }

        [Fact]
        public void EnterFaultedStateIfOneTaskCanceledAndOtherFaultedWhenUsingWhenAll()
        {
            var tcs1 = new TaskCompletionSource<object>();
            var tcs2 = new TaskCompletionSource<object>();
            var tcs3 = new TaskCompletionSource<object>();

            tcs1.SetResult(null);
            tcs2.SetCanceled();
            tcs3.SetException(new Exception());

            var comp = Task.WhenAll(tcs1.Task, tcs2.Task, tcs3.Task);
            Assert.Equal(TaskStatus.Faulted, comp.Status);
        }
        #endregion

        #region When Any Final Status
        [Theory]
        [InlineData(TaskStatus.RanToCompletion)]
        [InlineData(TaskStatus.Canceled)]
        [InlineData(TaskStatus.Faulted)]
        public void EnterStatusOfFirstCompletedTaskWhenUsingWhenAny(TaskStatus firstTaskStatus)
        {
            var tcs1 = new TaskCompletionSource<object>();
            var tcs2 = new TaskCompletionSource<object>();
            var tcs3 = new TaskCompletionSource<object>();

            switch (firstTaskStatus)
            {
                case TaskStatus.RanToCompletion: tcs1.SetResult(null); break;
                case TaskStatus.Canceled: tcs1.SetCanceled(); break;
                case TaskStatus.Faulted: tcs1.SetException(new Exception()); break;
            }

            // works with or without these
            //tcs2.SetResult(null);
            //tcs3.SetResult(null);

            var comp = Task.WhenAny(tcs1.Task, tcs2.Task, tcs3.Task).Unwrap();
            Assert.Equal(firstTaskStatus, comp.Status);
        }
        #endregion
    }
}
