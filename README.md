# Description

This repository showcases the behaviour of the `NetworkStream` type in .NET in which consecutive write calls have disproportionately high latency.

Running the test:
```
dotnet run -c release --project server
dotnet run -c release --project client
```

See the source code for more info.