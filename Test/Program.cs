using JiebaNet.Segmenter;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
           
            var segmenter = new JiebaSegmenter();
            var segments = segmenter.Cut("WebApi 教程", cutAll: true);
            Console.WriteLine("【全模式】：{0}", string.Join("/ ", segments));

            segments = segmenter.Cut("WebApi 教程");  // 默认为精确模式
            Console.WriteLine("【精确模式】：{0}", string.Join("/ ", segments));

            segments = segmenter.Cut("WebApi 教程");  // 默认为精确模式，同时也使用HMM模型
            Console.WriteLine("【新词识别】：{0}", string.Join("/ ", segments));

            segments = segmenter.CutForSearch("软件开发代码规范"); // 搜索引擎模式
            Console.WriteLine("【搜索引擎模式】：{0}", string.Join("/ ", segments));

            segments = segmenter.Cut("webapi 教程");
            Console.WriteLine("【歧义消除】：{0}", string.Join("/ ", segments));
            Console.ReadKey();
        }
    }
}
