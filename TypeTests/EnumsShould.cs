using System;
using Xunit;

namespace TypeTests
{
    public class EnumsShould
    {
        #region Types

        private enum State
        {
            Unknown,
            Initializing,
            Running,
            Complete
        }

        [Flags]
        private enum ColorsSupported : byte
        {
            None   = 0x00,
            Red    = 0x01,
            Orange = 0x02,
            Yellow = 0x04,
            // ToString will prefer combination values and use them alone if possible
            // YellowOrange = 0x6,
            Green  = 0x08,
            Blue   = 0xA0,
            Indigo = 0xB0,
            Violet = 0xC0
        }

        #endregion

        #region General Use

        [Fact]
        public void SupportZeroBasedIntegerCastByDefault()
        {
            Assert.Equal(0, (int)State.Unknown);
            Assert.Equal(1, (int)State.Initializing);
            Assert.Equal(2, (int)State.Running);
            Assert.Equal(3, (int)State.Complete);
        }

        [Fact]
        public void SupportFlagsUsage()
        {
            var colors = (ColorsSupported)0x6;

            Assert.Equal(ColorsSupported.Orange, colors & ColorsSupported.Orange);
            Assert.Equal(ColorsSupported.Yellow, colors & ColorsSupported.Yellow);

            // to string when flags attribute present is a comma separated list of active names
            Assert.Equal($"{ColorsSupported.Orange}, {ColorsSupported.Yellow}", colors.ToString());
        }

        #endregion

        #region Static Methods
        [Fact]
        public void SupportRetrievalOfNames()
        {
            int i = 0;

            foreach(var name in Enum.GetNames(typeof(State)))
            {
                switch(i)
                {
                    case 0: Assert.Equal("Unknown", name); break;
                    case 1: Assert.Equal("Initializing", name); break;
                    case 2: Assert.Equal("Running", name); break;
                    case 3: Assert.Equal("Complete", name); break;
                    default: throw new Exception();
                }

                if(++i >= 4)
                    break;
            }
        }

        [Fact]
        public void SupportRetrievalOfValues()
        {
            int i = 0;

            foreach (var value in Enum.GetValues(typeof(State)))
            {
                Assert.Equal(i++, (int)value);
            }
        }

        #endregion
    }
}
