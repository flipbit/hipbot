using System;

namespace HipBot.Core
{
    public static class Out
    {
        public static void WriteLine(string value, params object[] args)
        {
            if (In.Listening)
            {
                var left = Console.CursorLeft;

                Console.MoveBufferArea(0, Console.CursorTop, Console.WindowWidth, 1, 0, Console.CursorTop + 1);
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine(value, args);
                Console.SetCursorPosition(left, Console.CursorTop);
            }
            else
            {
                Console.WriteLine(value, args);
            }
        }
    }
}
