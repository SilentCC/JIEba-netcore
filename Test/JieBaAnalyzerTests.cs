using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jieba.NET;
using JiebaNet.Segmenter;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis;
using Lucene.Net.Util;
using static Lucene.Net.Util.Fst.Util;
using Xunit;
using Lucene.Net.Analysis.Standard;
using FluentAssertions;

namespace Test
{
    public class JieBaAnalyzerTests
    {
        [Theory]
        [InlineData("ASP.NET Core", "asp.net,core")]
        [InlineData("Lucene.Net", "lucene.net")]
        [InlineData("Json.NET", "json.net")]
        [InlineData("C#", "c#")]
        [InlineData("C++", "c++")]
        [InlineData("学习.NET编程", "学习,.net,编程")]
        [InlineData("F#语法概览", "f#,语法,概览")]
        [InlineData("学习云原生开发", "学习,云原生,开发")]
        [InlineData("初步认识低代码", "初步,认识,低代码")]
        [InlineData("阿里云", "阿里云")]
        [InlineData("微服务架构", "微服务,架构")]
        [InlineData("元宇宙探索", "元宇宙,探索")]
        [InlineData("读懂Web3架构", "读懂,web3,架构")]
        public void GetTokenStream_test(string phrase, string expected)
        {
            Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Default, true);
            using TokenStream tokenStream = analyzer.GetTokenStream(null, new StringReader(phrase));
            ICharTermAttribute charTermAttribute = tokenStream.AddAttribute<ICharTermAttribute>();
            tokenStream.Reset();

            var tokeList = new List<string>();
            while (tokenStream.IncrementToken())
            {
                tokeList.Add(charTermAttribute.ToString());
            }
            tokenStream.End();

            var actual = string.Join(",", tokeList);
            actual.Should().Be(expected);
        }
    }
}
