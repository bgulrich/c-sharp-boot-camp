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
        #region Final Status
        [Fact]
        public void RunToCompletionIfNoTasksCanceled()
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
        public void EnterCanceledStateIfOneTaskCanceled()
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
        public void EnterFaultedStateIfOneTaskCanceledAndOtherFaulted()
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
    }
}
