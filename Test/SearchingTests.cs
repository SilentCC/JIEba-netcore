using System.Linq;
using FluentAssertions;
using jieba.NET;
using JiebaNet.Segmenter;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers.Classic;
using Lucene.Net.Store;
using Lucene.Net.Util;
using Xunit;

namespace Test
{
    public class SearchingTests
    {
        private const LuceneVersion _appLuceneVersion = LuceneVersion.LUCENE_48;

        [Theory]
        [InlineData("ASP.NET Core", "ASP.NET Core 中使用 dapr：pub/sub 发送与订阅消息")]
        public void Searching_with_jieba(string phrase, string title)
        {
            using var dir = FSDirectory.Open("./index/test");
            var analyzer = new JieBaAnalyzer(TokenizerMode.Default, true);
            var indexConfig = new IndexWriterConfig(_appLuceneVersion, analyzer);
            using var writer = new IndexWriter(dir, indexConfig);

            var source = new { title };
            var doc = new Document
            {
                new TextField(nameof(source.title), source.title, Field.Store.YES)
            };
            writer.AddDocument(doc);
            writer.Flush(triggerMerge: false, applyAllDeletes: false);

            var queryParser = new QueryParser(_appLuceneVersion, nameof(source.title), analyzer);
            var query = queryParser.Parse(phrase);
            using var reader = writer.GetReader(applyAllDeletes: true);
            var searcher = new Lucene.Net.Search.IndexSearcher(reader);
            var hit = searcher.Search(query, 10).ScoreDocs.FirstOrDefault();
            hit.Should().NotBeNull();

            var foundDoc = searcher.Doc(hit!.Doc);
            foundDoc.Should().NotBeNull();
            foundDoc.Get(nameof(source.title)).Should().Be(source.title);

            writer.DeleteAll();
        }
    }
}
