using Cake.Frosting;
using Cake.Unity;
using Cake.Unity.Arguments;

namespace Build.Tasks.Example.WebGL;

[TaskName("WebGLDev")]
public class WebGLDevTask : FrostingTask<BuildContext>
{
    private const string ExecuteMethod = "Builds.BuildWebGL.BuildDev";


    public override void Run(BuildContext context)
    {
        var unityEditor = context.FindUnityEditor(2020, 3);
        context.UnityEditor(
            unityEditor.Path,
            new UnityEditorArguments
            {
                ExecuteMethod = ExecuteMethod,
                ProjectPath = $"../src/{ShareConstants.ProjectName}",
                LogFile = $"../logs/build-{nameof(WebGLDevTask)}.log",
                BuildTarget = BuildTarget.standalone
            },
            new UnityEditorSettings
            {
                RealTimeLog = true,
            }
        );
    }
}