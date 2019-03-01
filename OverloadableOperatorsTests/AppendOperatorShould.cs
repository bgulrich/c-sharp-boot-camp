using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace OverloadableOperatorsTests
{
    public class AppendOperatorShould
    {
        #region Helper

        public IEnumerable<string> _storeGroup;
        public IEnumerable<string> _storesToBeAdded;

        public void Calculator(IEnumerable<string> sg, IEnumerable<string> sta)
        {
            _storeGroup = sg;
            _storesToBeAdded = sta;
        }

        public struct StoreGroup
        {
            public IEnumerable<string> _storeGroup { get; set; }

            public StoreGroup(IEnumerable<string> storeGroup)
            {
                _storeGroup = storeGroup;
            }

            public static StoreGroup operator +(StoreGroup initial, StoreGroup subsequent)
            {
                StoreGroup result = new StoreGroup(initial._storeGroup);
                result._storeGroup = initial._storeGroup ?? Enumerable.Empty<string>().Concat(subsequent._storeGroup ?? Enumerable.Empty<string>());

                return result;
            }
        }
        #endregion

        [Fact]
        public void WorkAsExpected()
        {
            StoreGroup sg_1 = new StoreGroup(new string[] { "1", "2", "3" });
            StoreGroup sg_2 = new StoreGroup(new string[] { "4", "5", "6" });
            StoreGroup sg_3 = new StoreGroup(new string[] { });

            sg_3 = sg_1 + sg_2;

            Assert.True(new string[] { "1", "2", "3", "4", "5", "6" }.SequenceEqual(sg_3._storeGroup));
        }

    }
}
