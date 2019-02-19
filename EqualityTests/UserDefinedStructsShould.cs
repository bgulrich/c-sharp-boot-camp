using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace EqualityTests
{
    public class UserDefinedStructsShould
    {
        #region Structs

        private struct Point_NoEquality
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        private struct Point : IEquatable<Point>, IComparable<Point>
        {
            public int X { get; set; }
            public int Y { get; set; }

            #region Equality guidelines

            // 1. Override virtual Object.Equals(Object)
            public override bool Equals(object obj)
            {
                if (obj is Point p)
                {
                    return this.Equals(p);
                }

                return false;
            }

            // 2. Implement IEquatable<T>
            public bool Equals(Point other)
            {
                return (X == other.X) && (Y == other.Y);
            }

            // 3. Optional - overload == and != operators
            public static bool operator ==(Point left, Point right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(Point left, Point right)
            {
                return !(left == right);
            }

            // 4. Override GetHashCode
            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            // 5. Optional - implement IComparable<T> and override >=/<= operators
            // Below is an example only and probably wouldn't really be implemented on this struct

            /// <summary>
            /// Returns an integer value that indicates the relative order of two values
            /// </summary>
            /// <param name="other"></param>
            /// <returns>
            /// Less than zero if this precedes other
            /// Zero if this is same as other
            /// More than zero if this follows other
            /// </returns>
            public int CompareTo(Point other)
            {
                // distance from origin ??
                return (this.X * this.X + this.Y * this.Y).CompareTo(other.X * other.X + other.Y * other.Y);
            }

            public static bool operator >=(Point left, Point right)
            {
                return left.CompareTo(right) >= 0;
            }

            public static bool operator <=(Point left, Point right)
            {
                return left.CompareTo(right) <= 0;
            }
            #endregion
        }

        #endregion

        [Fact]
        public void HaveNoEqualityOperatorDefinedByDefault()
        {
            var p1 = new Point_NoEquality {X = 4, Y = 5};
            var p2 = new Point_NoEquality {X = 4, Y = 5};

            // cant' do this
            // Assert.True(p1 == p2);
        }

    }
}
