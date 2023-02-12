using PSSN.Api;

//тестовый коментарий для сонаркуб
var cancelTokenSource = new CancellationTokenSource();
var token = cancelTokenSource.Token;
Startup
    .ConfigApp(
        Startup
            .ConfigureHost(
                WebApplication
                    .CreateBuilder(new WebApplicationOptions { Args = args }))
            .Build(),
        token
    )
    .Run();

cancelTokenSource.Cancel();
cancelTokenSource.Dispose();