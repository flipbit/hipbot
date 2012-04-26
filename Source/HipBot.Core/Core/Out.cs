using System;

namespace HipBot.Core
{
    public static class Out
    {
        private class Locker
        {
        } 

        public static void WriteLine(string value, params object[] args)
        {
            if (In.Listening)
            {
                var left = Console.CursorLeft;

                Console.MoveBufferArea(0, Console.CursorTop, Console.WindowWidth, 1, 0, Console.CursorTop + 1);
                Console.SetCursorPosition(0, Console.CursorTop);
                WriteColorCodedLine(string.Format(value, args));
                Console.WriteLine(string.Empty);
                Console.SetCursorPosition(left, Console.CursorTop);
            }
            else
            {
                WriteColorCodedLine(string.Format(value, args));
                Console.WriteLine(Environment.NewLine);
            }
        }

        public static void Write(string value, params object[] args)
        {
            if (In.Listening)
            {
                var left = Console.CursorLeft;

                Console.MoveBufferArea(0, Console.CursorTop, Console.WindowWidth, 1, 0, Console.CursorTop + 1);
                Console.SetCursorPosition(0, Console.CursorTop);
                WriteColorCodedLine(string.Format(value, args));
                Console.SetCursorPosition(left, Console.CursorTop);
            }
            else
            {
                WriteColorCodedLine(string.Format(value, args));
            }
        }

        public static void WriteColorCodedLine(string value)
        {
            var segements = value.Split('|');

            lock (typeof(Locker))
            {
                foreach (var segement in segements)
                {
                    ConsoleColor color;

                    if (Enum.TryParse(segement, true, out color))
                    {
                        Console.ForegroundColor = color;
                    }
                    else
                    {
                        Console.Write(segement);
                    }
                }
            }
        }

        public static int GetColorCodedLineLength(string value)
        {
            var length = 0;

            var segements = value.Split('|');

            lock (typeof (Locker))
            {
                foreach (var segement in segements)
                {
                    ConsoleColor color;

                    if (Enum.TryParse(segement, true, out color))
                    {
                    }
                    else
                    {
                        length += segement.Length;
                    }
                }
            }

            return length;
        }
    }
}
