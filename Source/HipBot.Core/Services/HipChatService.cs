using System;
using agsXMPP;
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

            // Construct message
            var message = new Message
            {
                From = msg.From.Resource,
                Room = msg.From.User,
                Body = msg.Body,
                To = msg.To.Resource,
                Received = DateTime.Now
            };

            // Construct args
            var args = new MessageEventArgs
            {
                Message = message
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

        public event EventHandler<LoginEventArgs> OnLogin;


        void ConnectionOnAuthError(object sender, agsXMPP.Xml.Dom.Element e)
        {
            Out.WriteLine(e.InnerXml);
        }

        void ConnectionOnError(object sender, Exception ex)
        {
            Out.WriteLine(ex.Message);
        }

    }
}
