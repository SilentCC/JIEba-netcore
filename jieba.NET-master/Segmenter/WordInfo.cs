using System;
namespace JiebaNet.Segmenter
{
    public class WordInfo
    {
        public WordInfo(string value,int position)
        {
            this.value = value;
            this.position = position;
        }
        //分词的内容
        public string value { get; set; }
        //分词的初始位置
        public int position { get; set; }
    }
}
