using System;
using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis;
using JiebaNet.Segmenter;
using System.IO;
using System.Collections.Generic;

namespace jieba.NET
{
    public class JieBaTokenizer
        : Tokenizer
    {
        private static object _LockObj = new object();
        private static bool _Inited = false;
        private System.Collections.Generic.List<JiebaNet.Segmenter.Token> _WordList = new List<JiebaNet.Segmenter.Token>();
        private string _InputText;
        private bool _OriginalResult = false;

        private ICharTermAttribute termAtt;
        private IOffsetAttribute offsetAtt;
        private IPositionIncrementAttribute posIncrAtt;
        private ITypeAttribute typeAtt;

        private List<string> stopWords = new List<string>();
        private string stopUrl="./stopwords.txt";
        private JiebaSegmenter segmenter;

        private System.Collections.Generic.IEnumerator<JiebaNet.Segmenter.Token> iter;
        private int start =0;

        private TokenizerMode mode;



        public JieBaTokenizer(TextReader input,TokenizerMode Mode)
            :base(AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY,input)
        {
            segmenter = new JiebaSegmenter();
            mode = Mode;
            StreamReader rd = File.OpenText(stopUrl);
            string s = "";
            while((s=rd.ReadLine())!=null)
            {
                stopWords.Add(s);
            }
           
            Init();
            
        }

        private void Init()
        {
            termAtt = AddAttribute<ICharTermAttribute>();
            offsetAtt = AddAttribute<IOffsetAttribute>();
            posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
            typeAtt = AddAttribute<ITypeAttribute>();
        }

        private string ReadToEnd(TextReader input)
        {
            return input.ReadToEnd();
        }

        public sealed override Boolean IncrementToken()
        {
            ClearAttributes();

            Lucene.Net.Analysis.Token word = Next();
            if(word!=null)
            {
                var buffer = word.ToString();
                termAtt.SetEmpty().Append(buffer);
                offsetAtt.SetOffset(CorrectOffset(word.StartOffset),CorrectOffset(word.EndOffset));
                typeAtt.Type = word.Type;
                return true;
            }
            End();
            this.Dispose();
            return false;
            
        }

        public Lucene.Net.Analysis.Token Next()
        {
           
            int length = 0;
            bool res = iter.MoveNext();
            Lucene.Net.Analysis.Token token;
            if (res)
            {
                JiebaNet.Segmenter.Token word = iter.Current;

                token = new Lucene.Net.Analysis.Token(word.Word, word.StartIndex,word.EndIndex);
               // Console.WriteLine("xxxxxxxxxxxxxxxx分词："+word.Word+"xxxxxxxxxxx起始位置："+word.StartIndex+"xxxxxxxxxx结束位置"+word.EndIndex);
                start += length;
                return token;

            }
            else
                return null;    
            
        }

        public override void Reset()
        {
            base.Reset();

            _InputText = ReadToEnd(base.m_input);
            RemoveStopWords(segmenter.Tokenize(_InputText,mode));


            start = 0;
            iter = _WordList.GetEnumerator();

        }

        public void RemoveStopWords(System.Collections.Generic.IEnumerable<JiebaNet.Segmenter.Token> words)
        {
            _WordList.Clear();
            
            foreach(var x in words)
            {
                if(stopWords.IndexOf(x.Word)==-1)
                {
                    _WordList.Add(x);
                }
            }

        }

    }
}
