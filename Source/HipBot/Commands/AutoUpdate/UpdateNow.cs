using HipBot.Services;
using Sugar.Command;

namespace HipBot.Commands.AutoUpdate
{
    /// <summary>
    /// Sets the URL for the automatic updates
    /// </summary>
    public class UpdateNow : BoundCommand<UpdateNow.Options>
    {
        [Flag("update", "now")]
        public class Options { }

        #region Dependencies

        /// <summary>
        /// Gets or sets the update service.
        /// </summary>
        /// <value>
        /// The update service.
        /// </value>
        public IUpdateService UpdateService { get; set; }

        #endregion

        /// <summary>
        /// Executes the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        public override void Execute(Options options)
        {
            UpdateService.DownloadUpdate();
        }
    }
}
