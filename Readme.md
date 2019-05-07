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


