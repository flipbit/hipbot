using HipBot.Core;
using Sugar.Command;

namespace HipBot.Commands.System
{
    public class Help : BoundCommand<Help.Options>
    {
        [Flag("help")]
        public class Options {}

        public override void Execute(Options options)
        {
            var commands = new ParameterPrinter().Print(string.Empty, typeof (Help).Assembly);

            foreach(var command in commands)
            {
                Out.WriteLine(command);
            }
        }
    }
}
