using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using HipBot.Domain;
using HipBot.Interfaces.Services;
using Sugar;
using Sugar.Net;
using Sugar.Xml;

namespace HipBot.Services
{
    /// <summary>
    /// Service for accessing and manipulating users.
    /// </summary>
    public class UserService : IUserService
    {
        #region Dependencies

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

        #endregion

        private static User GetUserFromXmlNode(IXPathNavigable node)
        {
            return new User
            {
                Id = node.GetInnerXml("//user_id").AsInt32(),
                Name = node.GetInnerXml("//name"),
                Email = node.GetInnerXml("//email"),
                Title = node.GetInnerXml("//title"),
                PhotoUrl = node.GetInnerXml("//photo_url"),
                Status = node.GetInnerXml("//status"),
                StatusMessage = node.GetInnerXml("//status_message"),
                IsGroupAdmin = node.GetInnerXml("//is_group_admin") == "1"
            };
        }

        /// <summary>
        /// Lists all the users on the network.
        /// </summary>
        /// <returns></returns>
        public IList<User> List()
        {
            var users = new List<User>();

            var credentials = CredentialService.GetCredentials();

            var url = "https://api.hipchat.com/v1/users/list?format=xml&auth_token=" + credentials.ApiToken;

            var response = HttpService.Get(url);

            if (response.Success)
            {
                var xml = response.ToXml();

                foreach (var node in xml.GetMatches("//user"))
                {
                    users.Add(GetUserFromXmlNode(node));
                }
            }

            return users;
        }

        /// <summary>
        /// Gets the user with the given id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public User GetUser(int userId)
        {
            User user = null;

            var credentials = CredentialService.GetCredentials();

            var url = string.Format("https://api.hipchat.com/v1/users/show?user_id={0}&format=xml&auth_token={1}", userId, credentials.ApiToken);

            var response = HttpService.Get(url);

            if (response.Success)
            {
                var xml = response.ToXml();

                user = GetUserFromXmlNode(xml);
            }

            return user;
        }

        /// <summary>
        /// Gets the jabber id for user with the given id.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public string GetJabberIdForUser(User user)
        {
            var jabberId = string.Empty;

            // Get all the rooms in the account
            var credentials = CredentialService.GetCredentials();
            var url = "https://api.hipchat.com/v1/rooms/list?format=xml&auth_token=" + credentials.ApiToken;
            var response = HttpService.Get(url);

            if (response.Success)
            {
                // Set the Jabber Id to the account id from the room name
                jabberId = response.ToXml().GetInnerXml("//xmpp_jid").SubstringBeforeChar("_");
            }

            return jabberId + "_" + user.Id;
        }
    }
}
