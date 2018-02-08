using JiebaNet.Segmenter;
using System;
using System.Text;
using PanGu;
using PanGu.Match;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace jieba.NET
{
    
    class Program
    {
       
        static void Main(string[] args)
        {
            List<string> stopWords = new List<string>();
             string stopUrl = "./stopwords.txt";

            StreamReader rd = File.OpenText(stopUrl);
            string ss = "";
            while ((ss = rd.ReadLine()) != null)
            {
                stopWords.Add(ss);
            }

            var segmenter = new JiebaSegmenter();
            var segments = segmenter.Cut("我来到北京清华大学", cutAll: true);
            Console.WriteLine("【全模式】：{0}", string.Join("/ ", segments));

            segments = segmenter.Cut("我来到北京清华大学");  // 默认为精确模式
            Console.WriteLine("【精确模式】：{0}", string.Join("/ ", segments));

            segments = segmenter.Cut("他来到了网易杭研大厦");  // 默认为精确模式，同时也使用HMM模型
            Console.WriteLine("【新词识别】：{0}", string.Join("/ ", segments));

            segments = segmenter.CutForSearch("小明硕士毕业于中国科学院计算所，后在日本京都大学深造"); // 搜索引擎模式
            Console.WriteLine("【搜索引擎模式】：{0}", string.Join("/ ", segments));


           
            PanGu.Segment.Init();
            var s = new PanGu.Segment();
            var wordInfos = s.DoSegment("小明硕士毕业于中国科学院计算所，后在日本京都大学深造");

            var word = wordInfos.ToArray();
          
            foreach(var x in word)
            {
                Console.Write(" "+x.Word+" ");
            }
            Console.WriteLine();
            segments = segmenter.Cut("结过婚的和尚未结过婚的");
            Console.WriteLine("【歧义消除】：{0}", string.Join("/ ", segments));
            Console.ReadKey();
        }
    }
}