using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JiebaNet.Segmenter.Common;
using Segmenter.Common;
using Xunit;

namespace Test
{
    public class TrieNodeTests
    {
        [Fact]
        public void CharComparer_test()
        {
            var dict = new Dictionary<char, TrieNode>(CharComparer.IgnoreCase);
            dict.Add('a', new TrieNode('c'));
            Assert.True(dict.ContainsKey('A'));
            Assert.False(dict.ContainsKey('b'));
        }
    }
}
