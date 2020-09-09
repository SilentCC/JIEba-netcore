using System;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis;
using JiebaNet.Segmenter;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.FileProviders;
using System.Text;
using JiebaNet.Segmenter.Common;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace jieba.NET
{
    public class JieBaTokenizer : Tokenizer
    {
        private string _inputText;
        private readonly string _stropWordsPath = "Resources/stopwords.txt";

        private readonly JiebaSegmenter _segmenter;
        private TokenizerMode _mode;
        private ICharTermAttribute _termAtt;
        private IOffsetAttribute _offsetAtt;
        //private IPositionIncrementAttribute _posIncrAtt;
        private ITypeAttribute _typeAtt;
        private readonly List<JiebaNet.Segmenter.Token> _wordList = new List<JiebaNet.Segmenter.Token>();

        private IEnumerator<JiebaNet.Segmenter.Token> _iter;

        public Dictionary<string, int> StopWords { get; } = new Dictionary<string, int>();

        public JieBaTokenizer(TextReader input, TokenizerMode Mode, bool defaultIgnore = false, bool defaultUserDict = false)
            : base(AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, input)
        {

            _segmenter = new JiebaSegmenter();
            _mode = Mode;
            if (defaultIgnore)
            {
                var list = FileExtension.ReadEmbeddedAllLines(Assembly.GetExecutingAssembly(), _stropWordsPath);
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    if (StopWords.ContainsKey(item))
                        continue;
                    StopWords.Add(item, 1);
                }

            }
            if (defaultUserDict)
            {
                _segmenter.LoadUserDictForEmbedded(Assembly.GetCallingAssembly(), "dict.txt");
            }

            if(!string.IsNullOrEmpty(Settings.IgnoreDictFile))
            {
                var list = FileExtension.ReadAllLines(Settings.IgnoreDictFile);
                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item))
                        continue;
                    if (StopWords.ContainsKey(item))
                        continue;
                    StopWords.Add(item, 1);
                }
            }

            if (!string.IsNullOrEmpty(Settings.UserDictFile))
            {
                _segmenter.LoadUserDict(Settings.UserDictFile);
            }

            Init();
        }

        #region private func
        private void Init()
        {
            _termAtt = AddAttribute<ICharTermAttribute>();
            _offsetAtt = AddAttribute<IOffsetAttribute>();
            //_posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
            _typeAtt = AddAttribute<ITypeAttribute>();
            AddAttribute<IPositionIncrementAttribute>();
        }

        private string ReadToEnd(TextReader input)
        {
            return input.ReadToEnd();
        }


        private Lucene.Net.Analysis.Token Next()
        {
            var res = _iter.MoveNext();
            if (res)
            {
                var word = _iter.Current;
                var token = new Lucene.Net.Analysis.Token(word.Word, word.StartIndex, word.EndIndex);
                if (Settings.Log)
                {
                    //chinese char
                    var zh = new Regex(@"[\u4e00-\u9fa5]|[^\x00-\xff]");
                    var offset = zh.Matches(word.Word).Count;
                    offset = offset > 20 ? 0 : offset;
                    Console.WriteLine($"==分词：{ word.Word.PadRight(10 - offset, '=') }==起始位置：{ word.StartIndex.ToString().PadLeft(3, '=') }==结束位置{ word.EndIndex.ToString().PadLeft(3, '=') }");
                }
                return token;
            }
            return null;
        }
        #endregion

        public sealed override bool IncrementToken()
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


        public override void Reset()
        {
            base.Reset();

            _inputText = ReadToEnd(base.m_input);
            RemoveStopWords(_segmenter.Tokenize(_inputText, _mode));

            _iter = _wordList.GetEnumerator();
        }

        private void RemoveStopWords(IEnumerable<JiebaNet.Segmenter.Token> words)
        {
            _wordList.Clear();

            foreach (var x in words)
            {
                if (!StopWords.ContainsKey(x.Word))
                {
                    _wordList.Add(x);
                }
            }
        }
    }
}