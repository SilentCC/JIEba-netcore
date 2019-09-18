using System;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis;
using JiebaNet.Segmenter;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace jieba.NET
{
    public class JieBaTokenizer
        : Tokenizer
    {
        private static bool _initial = false;
        private string _inputText;
        private bool _originalResult = false;
        private int _start = 0;

        private readonly string _stropWordsPath = "Resources/stopwords.txt";

        private readonly JiebaSegmenter _segmenter;
        private TokenizerMode _mode;
        private ICharTermAttribute _termAtt;
        private IOffsetAttribute _offsetAtt;
        private IPositionIncrementAttribute _posIncrAtt;
        private ITypeAttribute _typeAtt;

        private Dictionary<string, int> _stopWords = new Dictionary<string, int>();
        private List<JiebaNet.Segmenter.Token> _wordList = new List<JiebaNet.Segmenter.Token>();

        private IEnumerator<JiebaNet.Segmenter.Token> _iter;

        public JieBaTokenizer(TextReader input, TokenizerMode Mode)
            : base(AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, input)
        {
            _segmenter = new JiebaSegmenter();
            _mode = Mode;
            LoadStopWords();
            Init();
        }

        private void LoadStopWords()
        {
            var fileProvider = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly);
            var fileInfo = fileProvider.GetFileInfo(_stropWordsPath);

            using (var reader = new StreamReader(fileInfo.CreateReadStream()))
            {
                var s = "";
                while ((s = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(s))
                        continue;
                    if (_stopWords.ContainsKey(s))
                        continue;
                    _stopWords.Add(s, 1);
                }
            }
        }

        private void Init()
        {
            _termAtt = AddAttribute<ICharTermAttribute>();
            _offsetAtt = AddAttribute<IOffsetAttribute>();
            _posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
            _typeAtt = AddAttribute<ITypeAttribute>();
        }

        private string ReadToEnd(TextReader input)
        {
            return input.ReadToEnd();
        }

        public sealed override Boolean IncrementToken()
        {
            ClearAttributes();

            var word = Next();
            if (word != null)
            {
                var buffer = word.ToString();
                _termAtt.SetEmpty().Append(buffer);
                _offsetAtt.SetOffset(CorrectOffset(word.StartOffset), CorrectOffset(word.EndOffset));
                _typeAtt.Type = word.Type;
                return true;
            }

            End();
            Dispose();
            return false;
        }

        private Lucene.Net.Analysis.Token Next()
        {
            var length = 0;
            var res = _iter.MoveNext();
            if (res)
            {
                var word = _iter.Current;
                var token = new Lucene.Net.Analysis.Token(word.Word, word.StartIndex, word.EndIndex);
                _start += length;
                return token;
            }
            return null;
        }

        public override void Reset()
        {
            base.Reset();

            _inputText = ReadToEnd(base.m_input);
            RemoveStopWords(_segmenter.Tokenize(_inputText, _mode));

            _start = 0;
            _iter = _wordList.GetEnumerator();
        }

        private void RemoveStopWords(IEnumerable<JiebaNet.Segmenter.Token> words)
        {
            _wordList.Clear();

            foreach (var x in words)
            {
                if (!_stopWords.ContainsKey(x.Word))
                {
                    _wordList.Add(x);
                }
            }
        }
    }
}