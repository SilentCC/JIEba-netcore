using System.Diagnostics;
using System.Text;
using System;
using Test;
using Xunit.Abstractions;
using Xunit.Sdk;

[assembly: Xunit.TestFramework($"{nameof(Test)}.{nameof(JiebaTestFramework)}", nameof(Test))]
namespace Test
{
    internal class JiebaTestFramework : XunitTestFramework
    {
        public JiebaTestFramework(IMessageSink messageSink) : base(messageSink)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Trace.Listeners.Add(new ConsoleTraceListener());
        }
    }
}
