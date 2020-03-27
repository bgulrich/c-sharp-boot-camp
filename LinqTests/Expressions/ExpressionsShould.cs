using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace LinqTests.IQueryable
{
    public class ExpressionsShould
    {
        [Fact]
        public void EmitExecutableLogicWhenCompiled()
        {
            Func<int, int, int> addFunc = (x, y) => x + y;
            Expression<Func<int, int, int>> addExpression = (x, y) => x + y;

            // Func is executable
            Assert.Equal(5, addFunc(2, 3));

            // but Expression is not
            //Assert.Equal(5, addExpression(2, 3)); // illegal

            // unless it's compiled
            var compiledExpression = addExpression.Compile();
            Assert.Equal(5, compiledExpression(2, 3));
        }

        [Fact]
        public void ExposeTraversableDataStructure()
        {
            Expression<Func<int, int, int, int>> addAndMultiplyExpression = (x, y, z) => (x + y) * z;

            // check parameters
            Assert.Equal(3, addAndMultiplyExpression.Parameters.Count);
            Assert.Equal("x", addAndMultiplyExpression.Parameters[0].Name);
            Assert.Equal("y", addAndMultiplyExpression.Parameters[1].Name);
            Assert.Equal("z", addAndMultiplyExpression.Parameters[2].Name);

            // check logic
            // root is multiply
            var bodyExpression = Assert.IsAssignableFrom<BinaryExpression>(addAndMultiplyExpression.Body);
            Assert.Equal(ExpressionType.Multiply, bodyExpression.NodeType);

            // right side of multiply is z
            var rightExpression = Assert.IsAssignableFrom<ParameterExpression>(bodyExpression.Right);
            Assert.Equal("z", rightExpression.Name);

            // left side of multiple is (x + y)
            var leftExpression = Assert.IsAssignableFrom<BinaryExpression>(bodyExpression.Left);
            Assert.Equal(ExpressionType.Add, leftExpression.NodeType);
            var leftLeftParameter = Assert.IsAssignableFrom<ParameterExpression>(leftExpression.Left);
            Assert.Equal("x", leftLeftParameter.Name);
            var leftRightParameter = Assert.IsAssignableFrom<ParameterExpression>(leftExpression.Right);
            Assert.Equal("y", leftRightParameter.Name);
        }
    }
}
