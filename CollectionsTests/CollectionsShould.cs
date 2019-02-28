using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class CollectionsShould
    {
        #region Dervied

        /// <summary>
        /// An IList of guids that will reject the addition of empty guids
        /// </summary>
        private class GuidCollection : Collection<Guid>
        {
            // called by Add() and Insert()
            protected override void InsertItem(int index, Guid item)
            {
                // reject empty guids
                if (item == Guid.Empty)
                    throw new ArgumentException();

                base.InsertItem(index, item);
            }

            // called by indexer
            protected override void SetItem(int index, Guid item)
            {
                // reject empty guids
                if (item == Guid.Empty)
                    throw new ArgumentException();

                base.SetItem(index, item);
            }
        }

        #endregion

        [Fact]
        public void SupportCustomizations()
        {
            var gc = new GuidCollection();

            var g = Guid.NewGuid();

            // add a guid
            gc.Add(g);
            Assert.Contains(g, gc);

            // reject addition of an empty guid
            Assert.Throws<ArgumentException>(() => gc.Add(Guid.Empty));

            // reject setting an empty guid
            Assert.Throws<ArgumentException>(() => gc[0] = Guid.Empty);
        }
    }
}
