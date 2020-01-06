using System;
using System.Collections.Generic;
using Xunit;
using Reactive.Bindings;
using IssueHub.Utils;

namespace IssueHub.Tests
{
    public class ReactiveCollectionExtensionsTest
    {
        [Fact]
        public void Merge_test_when_empty()
        {
            var collection = new ReactiveCollection<int>();

            collection.Merge(new List<int>
            {
                4,
                5,
                6,
            });

            Assert.Equal(3, collection.Count);
            Assert.Equal(4, collection[0]);
            Assert.Equal(5, collection[1]);
            Assert.Equal(6, collection[2]);
        }

        [Fact]
        public void Merge_test_when_new_collection_is_shorter()
        {
            var collection = new ReactiveCollection<int>();
            collection.Add(1);
            collection.Add(2);
            collection.Add(3);

            collection.Merge(new List<int>
            {
                4,
                5,
            });

            Assert.Equal(2, collection.Count);
            Assert.Equal(4, collection[0]);
            Assert.Equal(5, collection[1]);
        }

        [Fact]
        public void Merge_test_when_new_collection_is_longer()
        {
            var collection = new ReactiveCollection<int>();
            collection.Add(1);

            collection.Merge(new List<int>
            {
                4,
                5,
                6,
            });

            Assert.Equal(3, collection.Count);
            Assert.Equal(4, collection[0]);
            Assert.Equal(5, collection[1]);
            Assert.Equal(6, collection[2]);
        }

        [Fact]
        public void Merge_test_when_same_length()
        {
            var collection = new ReactiveCollection<int>();
            collection.Add(1);
            collection.Add(2);
            collection.Add(3);

            collection.Merge(new List<int>
            {
                4,
                5,
                6,
            });

            Assert.Equal(3, collection.Count);
            Assert.Equal(4, collection[0]);
            Assert.Equal(5, collection[1]);
            Assert.Equal(6, collection[2]);
        }
    }
}
