using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LambdaTests
{
    public class LambdasShould
    {
        #region Basics
        // "by reference" -> conceptual, not literal
        [Fact]
        public void UseClosureVariablesByReference()
        {
            // initialize int to 10
            int someClosureInt = 10;

            // create action using lambda with closure on int
            Action x = () =>
            {
                Assert.Equal(50, someClosureInt);
                someClosureInt = 100;
            };

            // set closure int to 50 before calling action
            someClosureInt = 50;

            x();

            Assert.Equal(100, someClosureInt);
        }

        /// --------------------------------------------------------------------------------------
        ///  Below code illustrates what compiler does with lambdas/closures behind the scences
        /// --------------------------------------------------------------------------------------
        [Fact]
        public void ActLikeThis()
        {
            var sgc = new SomeGeneratedClass();

            // initialize int to 10
            sgc.someGeneratedClosureInt = 10;

            // set closure int to 50 before calling action
            sgc.someGeneratedClosureInt = 50;

            sgc.GeneratedLambdaMethod();

            Assert.Equal(100, sgc.someGeneratedClosureInt);
        }

        // Compiler generated
        public class SomeGeneratedClass
        {
            // generated field
            public int someGeneratedClosureInt;

            // generated method to wrap code
            public void GeneratedLambdaMethod()
            {
                Assert.Equal(50, someGeneratedClosureInt);
                someGeneratedClosureInt = 100;
            }
        }

        #region Task Example

        // "by reference" -> conceptual, not literal
        [Fact]
        public async Task UseClosureVariablesByReferenceWithTask()
        {
            // initialize int to 10
            int someClosureInt = 10;

            // create action using lambda with closure on int
            var t = new Task(() =>
            {
                Assert.Equal(50, someClosureInt);
                someClosureInt = 100;
            });

            // set closure int to 50 before calling action in task
            someClosureInt = 50;

            t.Start();
            await t;

            Assert.Equal(100, someClosureInt);
        }

        /// --------------------------------------------------------------------------------------
        ///  Below code illustrates what compiler does with lambdas/closures behind the scences
        /// --------------------------------------------------------------------------------------
        [Fact]
        public async Task ActLikeThisWithTask()
        {
            var sgc = new SomeGeneratedClass();

            // initialize int to 10
            sgc.someGeneratedClosureInt = 10;

            // set closure int to 50 before calling action in task
            sgc.someGeneratedClosureInt = 50;

            // generate delegate that "points" to generated method
            var generatedDelegate = new Action(sgc.GeneratedLambdaMethod);

            var t = new Task(generatedDelegate);
            t.Start();
            await t;

            Assert.Equal(100, sgc.someGeneratedClosureInt);
        }

        #endregion

        #endregion

        #region Race Conditions
        [Fact]
        public async Task CreateRaceConditions()
        {
            int i = 0, s = 0;
            var o = new object();

            Action incrementor = () =>
            {
                for (int j = 0; j < 1000000; ++j)
                {
                    ++i;
                }
            };

            Action synchronizedIncrementor = () =>
            {
                for (int j = 0; j < 1000000; ++j)
                {
                    lock (o) { ++s; }
                }
            };

            await Task.WhenAll(Task.Factory.StartNew(incrementor), Task.Factory.StartNew(incrementor),
                               Task.Factory.StartNew(synchronizedIncrementor), Task.Factory.StartNew(synchronizedIncrementor));

            Assert.NotEqual(2000000, i);
            Assert.Equal(2000000, s);
        }

        #endregion
    }
}
