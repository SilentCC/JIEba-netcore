using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using JiebaNet.Segmenter;
using Xunit;

namespace Test
{
    public class WordDictionaryTests
    {
        [Fact]
        public void Load_word_dictionary_test()
        {
            var dict = WordDictionary.Instance;
            dict.Total.Should().BeGreaterThan(10);
            dict.ContainsWord(".net").Should().BeTrue();
            dict.ContainsWord("C#").Should().BeTrue();
        }
    }
}
