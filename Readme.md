# Lusk
Small http server for integration testing.
It's usually hosted within unit test.

```csharp
var lusk = Lusk.Run();


// or some customizations
var lusk = Lusk.Run<Reader, Writer>("127.0.0.1", 8080);


class JsonReader : Reader
{
    override Read()
}
var lusk = Lusk.Run<JsonReader, JsonWriter>("127.0.0.1", 8080);

```

* https://stackoverflow.com/questions/55893092/content-length-cant-be-trusted-when-reading-a-response-from-sslstream
* https://docs.microsoft.com/en-us/windows/win32/winsock/complete-server-code
* https://csharp.hotexamples.com/examples/-/Socket/Receive/php-socket-receive-method-examples.html
* https://searchcode.com/file/14302589/WinsockHttpServer/winsock%20http%20server.cpp/
