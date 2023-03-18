# Unity CI template (CLI)

This is an example project for Unity CI. It uses the Unity CLI to build the project.

The project is configured to build for Windows and WebGL.

## Setup

1. Clone the repository
2. Paste your Unity project into the `src` folder
3. Open the `CI/ShareConstants.cs` file and change the `ProjectName` to your project name. You can add more constants if you want and use them in your build scripts (like the `UnityLicense`)
4. Open the `CI/Tasks` folder and create a new task for each platform you want to build for. 
You can use the tasks in the `CI/Tasks/Example` folder as a template.
5. Open your Unity project and go to `Edit > Project Settings > Player` and set the `Product Name` to the same value as the `ProjectName` in `CI/ShareConstants.cs`
6. Import the `UnityCIExample.unitypackage` into your project
7. Open the `Editor/Builds` folder and create a new build script for each platform you want to build for. You can use the scripts in the `Editor/Builds/Examples` folder as a template.
8. Make sure that `ExecuteMethod` constant in your tasks match the names of your build scripts (in a Namespace.Class.Method format)

You can use additional arguments in your build scripts. For that you need to add fields to the `BuildContext` class and add them to the `BuildContext` constructor using the `Argument` or `HasArgument` extension methods. You can then access the arguments in your build scripts using the `context.Foo` syntax.

```csharp
public class BuildContext : FrostingContext
{
    public string Foo { get; }
    public bool Bar { get; }
    public BuildContext(ICakeContext context) : base(context)
    {
        // This is the argument that is passed to the build script as --some-argument1=foo
        Foo = context.Argument("foo", "foo");
        // This is the argument that is passed to the build script as --some-argument2
        Bar = context.HasArgument("bar");
    }
}
```
```csharp
...

new UnityEditorArguments
{
    ExecuteMethod = ExecuteMethod,
    ProjectPath = $"../src/{ShareConstants.ProjectName}",
    LogFile = $"../logs/build-pc.log",
    BuildTarget = BuildTarget.standalone,
    SetCustomArguments = (obj) => SetCustomArguments(obj, context)
},

...

private void SetCustomArguments(dynamic arguments, BuildContext context)
{
    arguments.foo = context.Foo;
    arguments.bar = context.Bar;
}
```

In Unity you can use `BuildOptionsArgs` class for provide additional arguments to the build process. Just add new fields to the `BuildOptionsWrapper` class and use `BuildOptionsArgs.ParseBuildConfigPath` method to get `BuildOptionsArgs` with parsed arguments.

```csharp
public static class BuildOptionsArgs
{
    public class BuildOptionsWrapper
    {
        public string Version { get; set; } = Application.version;
    }
    public static BuildOptionsWrapper ParseBuildConfigPath()
    {
        BuildOptionsWrapper options = new BuildOptionsWrapper();
        string[] args = Environment.GetCommandLineArgs();
        foreach (string t in args)
        {
            if (t.Contains("--version"))
            {
                options.Version = t.Split('=')[1];
                BuildUtils.SetVersionFile(BuildUtils.GetArtifactPath(), options.Version);
            }
        }
        return options;
    }
}
```
```csharp
private static BuildPlayerOptions CreateBuildPlayerOptions()
{
    var options = BuildOptionsArgs.ParseBuildConfigPath();
...
}
```

## Usage

Run
```bash
dotnet run --project CI/CI.csproj --target=<task name>
# or
cd CI
dotnet run --target=<task name>
```
Change `<task name>` to the name of the task you want to run.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details