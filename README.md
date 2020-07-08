# WireMockTest
A simple Test demonstrating a "bug" in WireMock.net

Commenting out `AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);` makes the test pass. 
