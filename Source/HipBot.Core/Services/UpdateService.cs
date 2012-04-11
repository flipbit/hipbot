using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using HipBot.Interfaces.Services;
using SevenZipLib;
using Sugar;
using Sugar.IO;
using Sugar.Net;
using Microsoft.VisualBasic;

namespace HipBot.Services
{
    /// <summary>
    /// Service to handle to automatic updating of the Bot.
    /// </summary>
    public class UpdateService : IUpdateService
    {
        #region Dependencies

        /// <summary>
        /// Gets or sets the file service.
        /// </summary>
        /// <value>
        /// The file service.
        /// </value>
        public IFileService FileService { get; set; }

        /// <summary>
        /// Gets or sets the HTTP service.
        /// </summary>
        /// <value>
        /// The HTTP service.
        /// </value>
        public IHttpService HttpService { get; set; }

        /// <summary>
        /// Gets or sets the config service.
        /// </summary>
        /// <value>
        /// The config service.
        /// </value>
        public IConfigService ConfigService { get; set; }

        #endregion

        /// <summary>
        /// Gets the version directory from URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public string GetVersionDirectoryFromUrl(string url)
        {
            string result;

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                var uri = new Uri(url);

                var path = uri.AbsolutePath;

                var fileName = Path.GetFileName(path);

                // Strip extension
                fileName = fileName.SubstringBeforeLastChar(".");

                // Get last filename component
                fileName = fileName.SubstringAfterLastChar(".");

                var userDirectory = FileService.GetUserDataDirectory();

                // Combine paths
                result = Path.Combine(userDirectory, "hipbot", fileName);
            }
            else
            {
                throw new ApplicationException("Invalid update URL: " + url);
            }

            return result;
        }

        /// <summary>
        /// Determines whether if a new version of the Bot is available.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if a new version is available; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewVersionAvailable()
        {
            var config = ConfigService.GetConfig();

            var current = config.GetValue("Update", "Current", string.Empty);
            var latest = GetLatestVersionUrl();

            // New version available?
            return current != latest;
        }

        private string GetLatestVersionUrl()
        {
            var url = string.Empty;

            var config = ConfigService.GetConfig();

            var updateUrl = config.GetValue("Update", "Url", "https://raw.github.com/flipbit/hipbot/master/version.txt");

            var response = HttpService.Get(updateUrl);

            if (response.Success)
            {
                url = response.ToString().Trim();
            }

            return url;
        }

        /// <summary>
        /// Downloads the latest version of the bot from the Update server.
        /// </summary>
        public void DownloadUpdate()
        {
            var config = ConfigService.GetConfig();

            var url = GetLatestVersionUrl();

            var response = HttpService.Get(url);

            if (response.Success)
            {
                var path = GetVersionDirectoryFromUrl(url);

                var fileName = Path.Combine(path, Path.GetFileName(url) ?? "update.zip");

                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                File.WriteAllBytes(fileName, response.Bytes);

                UncompressFile(fileName);

                config.SetValue("Update", "Current", url);

                ConfigService.SetConfig(config);
            }

        }



        private void UncompressFile(string archiveFilename)
        {
            if (!File.Exists(archiveFilename))
            {
                Console.WriteLine("Can't Decompress: {0}", archiveFilename);
            }

            Console.WriteLine("Decompressing: {0}", archiveFilename);

            var path = Path.GetDirectoryName(archiveFilename) ?? string.Empty;

            using (var archive = new SevenZipArchive(archiveFilename))
            {
                foreach (var entry in archive)
                {
                    if (entry.IsDirectory) continue;

                    var filename = entry.FileName.SubstringAfterChar("\\");

                    Console.WriteLine("Extracting File: {0}", filename);

                    var filePath = Path.Combine(path, filename);

                    using (var stream = File.Create(filePath))
                    {
                        entry.Extract(stream, ExtractOptions.OverwriteExistingFiles);

                        stream.Flush();

                        stream.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Runs the latest version of the bot.
        /// </summary>
        public void RunLatestVersion(bool waitForExit, bool allowThisInstance)
        {
            // Don't run latest version if debugging
            if (Debugger.IsAttached) return;

            var userDirectory = FileService.GetUserDataDirectory();
            var searchPath = Path.Combine(userDirectory, "hipbot");

            var files = Directory.GetFiles(searchPath, "hipbot.exe", SearchOption.AllDirectories);

            var latestVersion = string.Empty;
            var latestBuildDate = new DateTime(2000, 1, 1);

            foreach (var file in files)
            {
                if (File.GetLastWriteTime(file) > latestBuildDate)
                {
                    latestVersion = file;
                    latestBuildDate = File.GetLastWriteTime(file);
                }
            }

            if (string.IsNullOrWhiteSpace(latestVersion)) return;

            if (!allowThisInstance)
            {
                if (Process.GetCurrentProcess().MainModule.FileName == latestVersion) return;
            }

            Thread.Sleep(1000);

            var info = new ProcessStartInfo { FileName = latestVersion, UseShellExecute = false };

            var process = Process.Start(info);

            if (waitForExit) process.WaitForExit();

            Environment.Exit(0);
        }
    }
}
