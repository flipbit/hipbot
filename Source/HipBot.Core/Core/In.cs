using System;
using System.Text;
using System.Threading;
using Sugar;

namespace HipBot.Core
{
    public static class In
    {
        private static readonly StringBuilder Sb = new StringBuilder();
        private static int position;
        
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="In"/> is listening.
        /// </summary>
        /// <value>
        ///   <c>true</c> if listening; otherwise, <c>false</c>.
        /// </value>
        public static bool Listening { get; set; }

        /// <summary>
        /// Gets or sets the prompt.
        /// </summary>
        /// <value>
        /// The prompt.
        /// </value>
        public static string Prompt { get; private set; }

        public static Func<string> DynamicPrompt { get; set; } 

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public static ConsoleColor Color { get; set; }
       
        public static void Listen()
        {
            Listen(null);
        }

        public static void Listen(string prompt)
        {
            if (prompt != null)
            {
                Prompt = prompt;
            }

            Listening = true;

            var thread = new Thread(Listener);

            thread.Start();

            WritePrompt();
        }

        public static void WritePrompt()
        {
            var top = Console.CursorTop;

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, top);
            Console.Write(" ".Repeat(Console.WindowWidth));
            Console.SetCursorPosition(0, top);

            string prompt;

            if (DynamicPrompt != null)
            {
                prompt = DynamicPrompt.Invoke();
            }
            else
            {
                prompt = Prompt;
            }

            if (string.IsNullOrWhiteSpace(prompt))
            {
                prompt = string.Empty;
            }

            Console.Write(prompt);

            Console.Write(Sb.ToString());
            Console.SetCursorPosition(position + prompt.Length, top);
            Console.CursorVisible = true;
        }

        private static void Listener()
        {
            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        OnInput(new OnInputArgs {Input = Sb.ToString()});
                        Sb.Clear();
                        position = 0;
                        break;

                    case ConsoleKey.Backspace:
                        if (position > 1)
                        {
                            position--;
                            Sb.Remove(position, 1);
                        }
                        break;

                    case ConsoleKey.LeftArrow:
                        if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        {
                            position = 0;
                        }
                        else
                        {
                            position--;                            
                        }
                        break;

                    case ConsoleKey.RightArrow:
                        if ((key.Modifiers & ConsoleModifiers.Shift) != 0)
                        {
                            position = Sb.Length;
                        }
                        else
                        {
                            if (position < Sb.Length)
                            {
                                position++;
                            }                            
                        }
                        break;                        

                    case ConsoleKey.Escape:
                        Sb.Clear();
                        position = 0;
                        break;

                    default:
                        if (!Char.IsControl(key.KeyChar))
                        {
                            Sb.Insert(position, key.KeyChar);
                            position++;
                        }
                        break;
                }

                WritePrompt();
            }
        }

        public static event OnInputHandler OnInput;

        public static void SetPrompt(string prompt)
        {
            Prompt = prompt;

            if (Listening)
            {
                WritePrompt();
            }
        }
    }

    public delegate void OnInputHandler(OnInputArgs args);

    public class OnInputArgs
    {
        public string Input { get; set; }
    }
}
