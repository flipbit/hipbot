using System;
using System.Text;
using System.Threading;

namespace HipBot.Core
{
    public static class In
    {
        private static readonly StringBuilder sb = new StringBuilder();

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="In"/> is listening.
        /// </summary>
        /// <value>
        ///   <c>true</c> if listening; otherwise, <c>false</c>.
        /// </value>
        public static bool Listening { get; set; }

        public static void Listen()
        {
            Listening = true;

            var thread = new Thread(Listener);

            thread.Start();
        }


        private static void Listener()
        {
            while (true)
            {
                var key = Console.ReadKey(true);

                Console.Write(key.KeyChar);

                if (key.Key == ConsoleKey.Enter)
                {
                    OnInput(new OnInputArgs {Input = sb.ToString()});

                    sb.Clear();
                }
                else
                {
                    sb.Append(key.KeyChar);


                }
            }
        }

        public static event OnInputHandler OnInput;
    }

    public delegate void OnInputHandler(OnInputArgs args);

    public class OnInputArgs
    {
        public string Input { get; set; }
    }
}
