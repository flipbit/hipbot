using System;
using System.Collections.Generic;
using System.Linq;
using HipBot.Core;
using HipBot.Domain;
using HipBot.Interfaces.Services;
using Sugar;
using Sugar.Net;
using Sugar.Xml;

namespace HipBot.Services
{
    /// <summary>
    /// Service to manipulate rooms on the HipChat network.
    /// </summary>
    public class RoomService : IRoomService 
    {
        /// <summary>
        /// Gets or sets the HTTP service.
        /// </summary>
        /// <value>
        /// The HTTP service.
        /// </value>
        public IHttpService HttpService { get; set; }

        /// <summary>
        /// Gets or sets the credential service.
        /// </summary>
        /// <value>
        /// The credential service.
        /// </value>
        public ICredentialService CredentialService { get; set; }

        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        /// <summary>
        /// Gets or sets the hip chat service.
        /// </summary>
        /// <value>
        /// The hip chat service.
        /// </value>
        public IHipChatService HipChatService { get; set; }

        /// <summary>
        /// Gets all the rooms.
        /// </summary>
        /// <returns></returns>
        public IList<Room> List()
        {
            var rooms = new List<Room>();

            var credentials = CredentialService.GetCredentials();

            var url = "https://api.hipchat.com/v1/rooms/list?format=xml&auth_token=" + credentials.ApiToken;

            var response = HttpService.Get(url);

            if (response.Success)
            {
                var xml = response.ToXml();

                foreach (var node in xml.GetMatches("//room"))
                {
                    var room = new Room();

                    room.Id = node.GetInnerXml("//room_id").AsInt32();
                    room.Name = node.GetInnerXml("//name");
                    room.Topic = node.GetInnerXml("//topic");
                    room.LastActive = node.GetInnerXml("//last_actice").ToDouble().FromUnixTimestamp();
                    room.Created = node.GetInnerXml("//created").ToDouble().FromUnixTimestamp(); ;
                    room.OwnerUserId = node.GetInnerXml("//owner_user_id").AsInt32();
                    room.IsArchived = node.GetInnerXml("//is_archived") == "1";
                    room.IsPrivate = node.GetInnerXml("//is_private") == "1";
                    room.JabberId = node.GetInnerXml("//xmpp_jid");

                    // Strip domain part
                    room.JabberId = room.JabberId.SubstringBeforeChar("@");

                    rooms.Add(room);
                }
            }

            return rooms;
        }

        /// <summary>
        /// Joins the room with the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool Join(string name)
        {
            var joined = false;

            var rooms = List();

            foreach (var room in rooms)
            {
                // Check room name
                if (string.Compare(name, room.Name, true) != 0) continue;

                var config = ConfigService.GetConfig();

                config.SetValue("rooms", room.Name, room.JabberId);

                ConfigService.SetConfig(config);

                if (HipChatService.LoggedIn)
                {
                    HipChatService.Join(room);
                }
                else
                {
                    Out.WriteLine("{0} will be joined when you next login.", room.Name);
                }

                joined = true;

                break;
            }

            return joined;
        }

        /// <summary>
        /// Leaves the room with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Leave(string name)
        {
            var left = false;

            var rooms = List();

            foreach (var room in rooms)
            {
                // Check room name
                if (string.Compare(name, room.Name, true) != 0) continue;

                var config = ConfigService.GetConfig();

                config.Delete("rooms", room.Name);

                ConfigService.SetConfig(config);

                if (HipChatService.LoggedIn)
                {
                    HipChatService.Leave(room);
                }

                left = true;

                break;
            }

            return left;
        }

        /// <summary>
        /// Reconnects all rooms on login.
        /// </summary>
        public void Reconnect()
        {
            if (!HipChatService.LoggedIn)
            {
                return;                
            }

            var config = ConfigService.GetConfig();

            var lines = config.GetSection("Rooms");

            var rooms = List();

            foreach (var line in lines)
            {
                var l = line;

                if (rooms.Any(r => r.Name == l.Key))
                {
                    var room = rooms.First(r => r.Name == l.Key);

                    HipChatService.Join(room);
                }
                else
                {
                    config.Delete("Rooms", line.Key);
                }
            }

            ConfigService.SetConfig(config);
        }
    }
}
