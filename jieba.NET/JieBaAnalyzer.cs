using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.TokenAttributes;
using System.IO;
using JiebaNet.Segmenter;

namespace jieba.NET
{
    public class JieBaAnalyzer : Analyzer
    {
        public TokenizerMode _mode;
        private readonly bool _skipStopwords;

        public JieBaAnalyzer(TokenizerMode Mode, bool skipStopwords = false)
            : base()
        {
            _mode = Mode;
            _skipStopwords = skipStopwords;
        }

        protected override TokenStreamComponents CreateComponents(string filedName, TextReader reader)
        {
            var tokenizer = new JieBaTokenizer(reader, _mode);

            var tokenstream = (TokenStream)new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, tokenizer);

            tokenstream.AddAttribute<ICharTermAttribute>();
            tokenstream.AddAttribute<IOffsetAttribute>();

            return new TokenStreamComponents(tokenizer, tokenstream);
        }
    }
}
