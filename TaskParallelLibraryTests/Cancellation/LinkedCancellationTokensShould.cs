using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace TPL.CancellationTests
{
    public class LinkedCancellationTokensShould
    {
        [Fact]
        public void LinkedCancellationTokenSourcesShouldRequestCancellationIfAnyConstituentRequestsCancellation()
        {
            var cts1 = new CancellationTokenSource();
            var cts2 = new CancellationTokenSource();
            var cts3 = new CancellationTokenSource();

            var linkedCts12 = CancellationTokenSource.CreateLinkedTokenSource(cts1.Token, cts2.Token);
            var linkedCts23 = CancellationTokenSource.CreateLinkedTokenSource(cts2.Token, cts3.Token);

            cts1.Cancel();

            Assert.True(linkedCts12.IsCancellationRequested);
            Assert.True(linkedCts12.Token.IsCancellationRequested);

            Assert.False(linkedCts23.IsCancellationRequested);
            Assert.False(linkedCts23.Token.IsCancellationRequested);

            cts2.Cancel();

            Assert.True(linkedCts23.IsCancellationRequested);
            Assert.True(linkedCts23.Token.IsCancellationRequested);
        }
    }
}
