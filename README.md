# JIEba-netcore2.0
基于[jieba.NETCore](https://github.com/linezero/jieba.NET) 

在.net core版的JIEba分词上，做了修改，使其支持net core2.0 和支持Lucene接口

使用时，可以直接

Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Search);
             

Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Default);
