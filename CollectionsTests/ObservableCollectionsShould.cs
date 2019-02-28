using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class ObservableCollectionsShould
    {
        [Fact]
        public void RaiseEventsWhenCollectionUpdated()
        {
            var people = new ObservableCollection<string>();

            int addCount = 0;
            int removeCount = 0;
            int moveCount = 0;
            int replaceCount = 0;
            int resetCount = 0;

            // hookup changed event handler
            people.CollectionChanged += (s, a) =>
            {
                switch (a.Action)
                {
                    case NotifyCollectionChangedAction.Add: ++addCount; break;
                    case NotifyCollectionChangedAction.Remove: ++removeCount; break;
                    case NotifyCollectionChangedAction.Move: ++moveCount; break;
                    case NotifyCollectionChangedAction.Replace: ++replaceCount; break;
                    case NotifyCollectionChangedAction.Reset: ++resetCount; break;
                }
            };

            // add
            people.Add("Billy");
            people.Add("Susan");
            people.Add("Tommy");
            people.Insert(0, "Amanda");
            Assert.Equal(4, addCount);

            // remove
            people.RemoveAt(2);
            Assert.Equal(1, removeCount);

            // replace
            people[0] = "Jane";
            Assert.Equal(1, replaceCount);

            // move
            people.Move(0, 1);
            Assert.Equal(1, moveCount);

            // clear
            people.Clear();
            Assert.Equal(1, resetCount);
        }
    }
}
