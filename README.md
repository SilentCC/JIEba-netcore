# JIEba-netcore

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
>[具体demo参考](https://gitee.com/shshshdy/net-core-tool) 

# 感谢原作

在[原作](https://github.com/SilentCC/JIEba-netcore2.0/)的基础上,修改了加载用户分词的BUG.
