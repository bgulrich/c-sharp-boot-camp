using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CollectionTests
{
    public class LinkedListsShould
    {
        [Fact]
        public void DoLinkedListStuff()
        {
            var presidents = new LinkedList<string>();

            presidents.AddFirst("George H. W. Bush");
            presidents.AddLast("Bill Clinton");
            //presidents.AddLast("George W. Bush");
            presidents.AddLast("Barack Obama");
            presidents.AddLast("Donald Duck");
            presidents.AddLast("Donald Trump");

            // oops, forgot one!
            presidents.AddAfter(presidents.Find("Bill Clinton"), "George W. Bush");

            // oops, typo
            presidents.Remove("Donald Duck");

            // let's go back a little farther
            presidents.AddFirst("Ronald Reagan");

            Assert.Equal("Ronald Reagan", presidents.First.Value);
            Assert.Equal("George W. Bush", presidents.Find("Bill Clinton").Next.Value);
            Assert.DoesNotContain("Donald Duck", presidents);
        }

        [Fact]
        public void BeEnumerable()
        {
            var presidents = new LinkedList<string>();

            presidents.AddLast("Bill Clinton");
            presidents.AddLast("George W. Bush");
            presidents.AddLast("Barack Obama");
            presidents.AddLast("Donald Trump");

            var iterations = 0;

            foreach (var p in presidents)
            {
                ++iterations;
            }

            Assert.Equal(4, iterations);
        }

        [Fact]
        public void BeReverseTraversable()
        {
            var presidents = new LinkedList<string>();

            presidents.AddLast("Bill Clinton");
            presidents.AddLast("George W. Bush");
            presidents.AddLast("Barack Obama");
            presidents.AddLast("Donald Trump");

            var iterations = 0;
            var p = presidents.Last;

            while (p != null)
            {
                p = p.Previous;
                ++iterations;
            }

            Assert.Equal(4, iterations);
        }

    }
}
