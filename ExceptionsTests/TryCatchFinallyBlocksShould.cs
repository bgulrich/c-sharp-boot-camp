using System;
using Xunit;

namespace ExceptionTests
{
    public class TryCatchFinallyBlocksShould
    {
        [Fact]
        public void ExecuteOnlyTheFirstMatchingCatchClause()
        {
            bool caught = false;

            try
            {
                throw new ArgumentException("something went wrong");
            }
            catch (ArgumentNullException) { Assert.False(true); }
            catch (ArgumentException ex) when (ex.Message.Contains("kaput")) { Assert.False(true); }
            catch (ArgumentException ex) when (ex.Message.Contains("wrong")) { caught = true; }
            catch (Exception) { Assert.False(true); }

            Assert.True(caught);
        }

        [Fact]
        public void ExecuteFinallyBlockIfNoExceptionThrown()
        {
            bool finallyExecuted = false;

            try
            {
                Console.WriteLine("do some exception-free work");
            }
            finally
            {
                finallyExecuted = true;
            }

            Assert.True(finallyExecuted);
        }

        [Fact]
        public void ExecuteFinallyBlockIfExceptionCaught()
        {
            bool exceptionCaught = false;
            bool finallyExecuted = false;

            try
            {
                int[] array = new int[10];
                Console.WriteLine($"do some exception-full work: {array[50]}");
            }
            catch { exceptionCaught = true; }
            finally { finallyExecuted = true; }

            Assert.True(exceptionCaught);
            Assert.True(finallyExecuted);
        }

        [Fact]
        public void ExecuteFinallyBlockIfExceptionUncaught()
        {
            bool exceptionCaught = false;
            bool finallyExecuted = false;

            try
            {
                try
                {
                    int[] array = new int[10];
                    Console.WriteLine($"do some exception-full work: {array[50]}");
                }               
                finally { finallyExecuted = true; }
            }
            catch { exceptionCaught = true; }

            Assert.True(exceptionCaught);
            Assert.True(finallyExecuted);
        }
    }
}
