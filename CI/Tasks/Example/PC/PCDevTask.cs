using Cake.Frosting;
using Cake.Unity;
using Cake.Unity.Arguments;

namespace Build.Tasks.Example.PC;

[TaskName("PCDev")]
public class PCDevTask : FrostingTask<BuildContext>
{
    private const string ExecuteMethod = "Builds.BuildPC.BuildDev";


    public override void Run(BuildContext context)
    {
        var unityEditor = context.FindUnityEditor(2020, 3);
        context.UnityEditor(
            unityEditor.Path,
            new UnityEditorArguments
            {
                ExecuteMethod = ExecuteMethod,
                ProjectPath = $"../src/{ShareConstants.ProjectName}",
                LogFile = $"../logs/build-{nameof(PCDevTask)}.log",
                BuildTarget = BuildTarget.standalone,
                SetCustomArguments = (obj) => SetCustomArguments(obj, context)
            },
            new UnityEditorSettings
            {
                RealTimeLog = true,
            }
        );
    }

    private void SetCustomArguments(dynamic arguments, BuildContext context)
    {
        arguments.foo = context.Foo;
        arguments.bar = context.Bar;
    }
}