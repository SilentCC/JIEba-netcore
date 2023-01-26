using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using jieba.NET;
using JiebaNet.Segmenter;
using Xunit;

namespace Test
{
    public class StopwordsTests
    {
        [Fact]
        public void Stopwords_loading_test()
        {
            var tokenizer = new JieBaTokenizer(TextReader.Null, TokenizerMode.Default);
            tokenizer.StopWords.Count().Should().BeGreaterThan(10);
        }
    }
}
