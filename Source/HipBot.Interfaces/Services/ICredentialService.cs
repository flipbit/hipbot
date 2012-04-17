using HipBot.Domain;

namespace HipBot.Services
{
    /// <summary>
    /// Service interface for storing and retrieving HipChat credentials.
    /// </summary>
    public interface ICredentialService
    {
        /// <summary>
        /// Gets the default credentials.
        /// </summary>
        /// <returns></returns>
        Credentials GetCredentials();

        /// <summary>
        /// Sets the credentials.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        void SetCredentials(Credentials credentials);

        /// <summary>
        /// Determines if the credentials for the HipChat service have been set.
        /// </summary>
        /// <returns></returns>
        bool CredentialsSet();
    }
}
