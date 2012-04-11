using System;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.protocol.extensions.chatstates;
using agsXMPP.protocol.extensions.html;
using agsXMPP.Xml.Dom;
using HipBot.Core;
using HipBot.Domain;
using HipBot.Events;
using HipBot.Interfaces.Services;
using Message = HipBot.Domain.Message;
using Msg = agsXMPP.protocol.client.Message;

namespace HipBot.Services
{
    /// <summary>
    /// Service for interacting with the HipChat network
    /// </summary>
    public class HipChatService : IHipChatService
    {
        private XmppClientConnection connection;
        private Credentials current;

        public void Login(Credentials credentials)
        {
            connection = new XmppClientConnection("chat.hipchat.com");

            connection.OnLogin += ConnectionOnLogin;
            connection.OnAuthError += ConnectionOnAuthError;
            connection.OnError += ConnectionOnError;
            connection.OnMessage += ConnectionOnMessage;

            connection.UseStartTLS = true;
            connection.Open(credentials.JabberId, credentials.Password, "bot");

            current = credentials;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is logged in.
        /// </summary>
        /// <value>
        ///   <c>true</c> if logged in; otherwise, <c>false</c>.
        /// </value>
        public bool LoggedIn { get; private set; }

        void ConnectionOnMessage(object sender, Msg msg)
        {
            // Prevent loop-back
            if (msg.From.Resource == current.Name) return;

            // Ignore composing messages
            if (msg.Chatstate == Chatstate.composing) return;

            // Construct message
            var message = new Message
            {
                From = msg.From.Resource,
                Room = msg.From.User,
                Body = msg.Body,
                To = msg.To.Resource,
                Received = DateTime.Now
            };

            var room = new Room
            {
                JabberId = msg.From.User,
                IsChat = msg.Type == MessageType.chat
            };

            if (room.IsChat)
            {
                room.JabberId = msg.From.User;
            }

            // Construct args
            var args = new MessageEventArgs
            {
                Message = message,
                Room = room
            };

            // Fire
            OnMessage(this, args);
        }

        public event EventHandler<MessageEventArgs> OnMessage;

        void ConnectionOnLogin(object sender)
        {
            LoggedIn = true;


            OnLogin(this, new LoginEventArgs());
        }

        public bool Join(Room room)
        {
            var manager = new agsXMPP.protocol.x.muc.MucManager(connection);

            var jid = new Jid(room.JabberId, "conf.hipchat.com", string.Empty);

            manager.JoinRoom(jid, current.Name);

            Out.WriteLine("Joined {0}.", room.Name);

            return true;
        }

        /// <summary>
        /// Says the message in the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="message">The message.</param>
        public void Say(Room room, string message)
        {
            Say(room, message, false);
        }

        /// <summary>
        /// Says the message in the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        public void Say(Room room, string message, params object[] args)
        {
            Say(room, string.Format(message, args), false);
        }

        /// <summary>
        /// Says the message in the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="message">The message.</param>
        public void SayHtml(Room room, string message)
        {
            Say(room, message, true);
        }

        /// <summary>
        /// Says the message in the specified room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="message">The message.</param>
        /// <param name="args">The args.</param>
        public void SayHtml(Room room, string message, params object[] args)
        {
            Say(room, string.Format(message, args), true);
        }

        private void Say(Room room, string message, bool html)
        {
            Msg msg;

            if (room.IsChat)
            {
                var to = new Jid(room.JabberId, "chat.hipchat.com", string.Empty);

                msg = new Msg(to, current.JabberId, MessageType.chat, message);
            }
            else
            {
                Out.WriteLine("{0:HH:mm:ss} [{1}] {2}", DateTime.Now, current.Name, message);

                var to = new Jid(room.JabberId, "conf.hipchat.com", string.Empty);

                msg = new Msg(to, current.JabberId, MessageType.groupchat, message);
            }

            if (html)
            {
                msg.Html = new Html();
                msg.Html.Body = new Body();
                msg.Html.Body.Value = message;
            }

            connection.Send(msg);
        }


        public event EventHandler<LoginEventArgs> OnLogin;


        void ConnectionOnAuthError(object sender, Element e)
        {
            Out.WriteLine(e.InnerXml);
        }

        void ConnectionOnError(object sender, Exception ex)
        {
            Out.WriteLine(ex.Message);
        }

    }
}
