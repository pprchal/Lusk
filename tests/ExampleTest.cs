/// Example test
///

using NUnit.Framework;
using Lusk;
using Lusk.Core;
using System.IO;
using System.Net.Http;

namespace tests;


[TestFixture]
public class ExampleTest 
{
    [Test]
    public void MyMockTest()
    {
        var response = new HttpClient().PostAsync(
            Lusk.Url, // <-- it's http://127.0.0.1:8080 by default, then port +1 
            new StringContent("Lusk is awesome")
        )
        .Result
        .Content
        .ReadAsStringAsync()
        .Result;
        
        Assert.That(response, Is.EqualTo("Lusk!"));
    }

    [SetUp]
    public void Init()
    {
        Lusk = LuskFactory.Run(
            new HttpServer(
                (request) => HttpResponse.Single(
                    File.ReadAllText("../../../MockResponse.txt")
                )
            )
        );
        TestContext.Progress.WriteLine($"server listenting on: {Lusk.Url}");
    }

    [TearDown]
    public void Cleanup() => Lusk.Dispose();

    LuskRuntime Lusk;
}
