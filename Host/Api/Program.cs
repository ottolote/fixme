using FixMe.Api;

var app = FixMeApi.Create(args);

Console.CancelKeyPress += (_, eventArgs) =>
{
    eventArgs.Cancel = true;
    app.Stop();
};

await app.RunAsync();
