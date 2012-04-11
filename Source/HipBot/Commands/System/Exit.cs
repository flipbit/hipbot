using System;
using Sugar.Command;

namespace HipBot.Commands.System
{
    public class Exit : BoundCommand<Exit.Options>
    {
        [Flag("exit")]
        public class Options {}

        public override void Execute(Options options)
        {
            Environment.Exit(0);
        }
    }
}
