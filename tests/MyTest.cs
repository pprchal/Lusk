using NUnit.Framework;
using Lusk;
using Lusk.Core;
using System.IO;
using System.Net.Http;

namespace tests;

[TestFixture]
public class MyTest 
{
    [SetUp]
    public void Init()
    {
        LuskFactory.Run(
            new HttpServer(
                (request) => HttpResponse.Single(
                    File.ReadAllText("MockResponse.txt")
                )
            )
        );
    }

    [TearDown]
    public void Cleanup() => Lusk.Dispose();

    [Test]
    public void MyMockTest()
    {
        var response = new HttpClient().PostAsync(
            Lusk.Url, // <-- it's http://127.0.0.1:8080 by default, then port +1 
            new StringContent("Lusk")
        ).Result;
        
        Assert.That(response, Is.EqualTo("Lusk"));
    }

    LuskRuntime Lusk;
}
