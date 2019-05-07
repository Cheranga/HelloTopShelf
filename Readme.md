# Notes

## Using TopShelf

* Install `TopShelf` from nuget.
* Create a class which has the methods to be executed when the windows service starts and stops (There can be other situations too, such as pause, continuing and, all)
* In the main method use the `TopShelf` API to configure the service behaviour.

### Problems which you might face

If you are using the .net core you'll find that the console applications are getting built as a `dll` not as an `exe` as before. So to install the service we cannot use the same command as we did when `TopShelf` was used with .net framework.
(`[exe name] install`)

Follow the below mentioned steps to install the windows service,

* Browse to the location where the `dll` is built.
* Use the below command to install the service.

`dotnet [SERVICE NAME WITH THE EXTENSION DLL] install --localsystem`

But just by executing the command above the windows service will not work. If you start to `start` or `stop` the service from SCM, you'll get an error.

* Open registry editor.
* Browse to `HKEY_LOCAL_MACHINE\SYSTEM\CONTROLSET001\SERVICES\[YOUR SERVICE NAME]` and open the	`ImagePath` variable and modify the variable value to,

`"c:\program files\dotnet\dotnet.exe" "[FULL PATH TO THE DLL]"  -displayname "[LEAVE THE EXISTING VALUES]" -servicename "[LEAVE THE EXISTING VALUES]"`


> Reference - [Read the `boekabart` comment on this](https://github.com/Topshelf/Topshelf/issues/485)


## Dependency Injection

When implementing DI in a console (or a non web) application you'll need at least the below nuget libraries.

* `Microsoft.Extensions.DependencyInjection`
* `Microsoft.Extensions.Configuration.Json`
* `Microsoft.Extensions.Options.ConfigurationExtensions`

Always it's better to move the dependency registrations to a different class and, also to have separate your different configurations into their
own config files / sections. In here I have created a class called `Bootstrapper` and I have created a separate `JSON` configuration file called `appsettings.json`.

```JavaScript
{
  "ToDoApiConfig": {
    "Url": "some url"
  }
}
```

* Make sure you set this file's properties as to `Copy Always` (or to a setting which it will be available in the same directory as the executable)

The above are the pre-requisites which you need to do have dependency injection in the application. The next part is the code part where you register the dependencies.

* First you need the actual configuration object where your custom configuration data is present. That object is represented by the `IConfigurationRoot`.

```CSharp
private static IConfigurationRoot GetConfiguration()
{
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    return configuration;
}
```

* Then load the configuration data and register them as a dependency.

```CSharp
var configuration = GetConfiguration();
services.Configure<ToDoApiConfig>(configuration.GetSection("ToDoApi"));
services.AddSingleton(provider =>
{
    var apiConfig = provider.GetRequiredService<IOptions<ToDoApiConfig>>().Value;
    return apiConfig;
});
```

* Register the other dependencies as you would usually.

services.AddHttpClient<ITodoApiClient, TodoApiClient>();
services.AddTransient<IInvoiceProcessor, InMemoryInvoiceProcessor>();

> __`IServiceCollection.AddHttpClient`__ extension method is coming from the __`Microsoft.Extensions.Http`__ nuget library. In this application it's required because it has a typed HTTP client to communicate with the external web API.

