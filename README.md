# JIEba-netcore2.0

基于[jieba.NETCore](https://github.com/linezero/jieba.NET) 

在.net core版的JIEba分词上，做了修改，使其支持net core2.0 和支持[Lucene.net](https://github.com/apache/lucenenet)接口
ps: 修改了JIEba分词，导致的高亮bug

# Available On NuGet


 >[Lucene.JIEba.net](https://www.nuget.org/packages/Lucene.JIEba.net/)


# 集成到Lucene.Net示例

```c#
  Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Search);
  Analyzer analyzer = new JieBaAnalyzer(TokenizerMode.Default);
  TokenStream = analyzer.GetTokenStream(str,indexReader);

```

 # 相关文档
[JIEba.net 集成到Lucene.Net的过程](https://www.cnblogs.com/dacc123/p/8431369.html) 
