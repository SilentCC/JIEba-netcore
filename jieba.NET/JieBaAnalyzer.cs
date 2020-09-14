using System;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis.Util;
using System.IO;
using JiebaNet.Segmenter;


namespace jieba.NET
{
    public class JieBaAnalyzer : Analyzer
    {
        TokenizerMode _mode;
        bool _defaultUserDict;
        public JieBaAnalyzer(TokenizerMode Mode, bool defaultUserDict = false) : base()
        {
            _mode = Mode;
            _defaultUserDict = defaultUserDict;
        }

        protected override TokenStreamComponents CreateComponents(string filedName, TextReader reader)
        {
            var tokenizer = new JieBaTokenizer(reader, _mode, _defaultUserDict);

            var tokenstream = (TokenStream)new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, tokenizer);

            tokenstream.AddAttribute<ICharTermAttribute>();
            tokenstream.AddAttribute<IOffsetAttribute>();

            return new TokenStreamComponents(tokenizer, tokenstream);
        }
    }
}
