using jieba.NET;
using JiebaNet.Segmenter;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class LuceneHelper
    {
        public static IndexWriter GetIndexWriter()
        {
            var dir = FSDirectory.Open("Index_Data");
            //Analyzer analyzer = new SmartChineseAnalyzer(LuceneVersion.LUCENE_48);
            Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Search);

            var indexConfig = new IndexWriterConfig(LuceneVersion.LUCENE_48, analyzer);
            IndexWriter writer = new IndexWriter(dir, indexConfig);
            return writer;
        }

        public static void WriteDocument(string url, string title, string keywords, string description)
        {
            using (var writer = GetIndexWriter())
            {
                writer.DeleteDocuments(new Term("url", url));

                Document doc = new Document();
                doc.Add(new StringField("url", url, Field.Store.YES));

                TextField titleField = new TextField("title", title, Field.Store.YES);
                titleField.Boost = 3F;

                TextField keywordField = new TextField("keyword", keywords, Field.Store.YES);
                keywordField.Boost = 2F;

                TextField descriptionField = new TextField("description", description, Field.Store.YES);
                descriptionField.Boost = 1F;

                doc.Add(titleField);
                doc.Add(keywordField);
                doc.Add(descriptionField);
                writer.AddDocument(doc);
                writer.Flush(triggerMerge: true, applyAllDeletes: true);
                writer.Commit();
            }
        }

        public static List<string> GetKeyWords(string q)
        {
            List<string> keyworkds = new List<string>();
            Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Search);
            using (var ts = analyzer.GetTokenStream(null, q))
            {
                ts.Reset();
                var ct = ts.GetAttribute<Lucene.Net.Analysis.TokenAttributes.ICharTermAttribute>();

                while (ts.IncrementToken())
                {
                    StringBuilder keyword = new StringBuilder();
                    for (int i = 0; i < ct.Length; i++)
                    {
                        keyword.Append(ct.Buffer[i]);
                    }
                    string item = keyword.ToString();
                    if (!keyworkds.Contains(item))
                    {
                        keyworkds.Add(item);
                    }
                }
            }
            return keyworkds;
        }
        public static void Search(string q)
        {
            IndexReader reader = DirectoryReader.Open(FSDirectory.Open("Index_Data"));

            var searcher = new IndexSearcher(reader);

            var keyWordQuery = new BooleanQuery();
            foreach (var item in GetKeyWords(q))
            {
                keyWordQuery.Add(new TermQuery(new Term("title", item)), Occur.SHOULD);
                keyWordQuery.Add(new TermQuery(new Term("keyword", item)), Occur.SHOULD);
                keyWordQuery.Add(new TermQuery(new Term("description", item)), Occur.SHOULD);
            }
            var hits = searcher.Search(keyWordQuery, 200).ScoreDocs;

            foreach (var hit in hits)
            {
                var document = searcher.Doc(hit.Doc);
                Console.WriteLine("Url:{0}", document.Get("url"));
                Console.WriteLine("Title:{0}", document.Get("title"));
                Console.WriteLine("Keyword:{0}", document.Get("keyword"));
                Console.WriteLine("Description:{0}", document.Get("description"));
            }
        }
    }
}
