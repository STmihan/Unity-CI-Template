using Cake.Common;
using Cake.Core;
using Cake.Frosting;

namespace Build;

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