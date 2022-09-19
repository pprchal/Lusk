# Lusk
Collection of (http) servers for integration testing.
It's usually hosted within unit test.

* HTTP
* POP3


## Basic ussage
```csharp
[TestFixture]
public class MyTest 
{
    [SetUp]
    public void Init()
    {
        Lusk = LuskFactory.Run(
            new HttpServer(
                (request) => HttpServerResponse.Single(
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
```

## Special thanks add inspiration
* https://stackoverflow.com/questions/55893092/content-length-cant-be-trusted-when-reading-a-response-from-sslstream
* https://docs.microsoft.com/en-us/windows/win32/winsock/complete-server-code
* https://csharp.hotexamples.com/examples/-/Socket/Receive/php-socket-receive-method-examples.html
* https://searchcode.com/file/14302589/WinsockHttpServer/winsock%20http%20server.cpp/
