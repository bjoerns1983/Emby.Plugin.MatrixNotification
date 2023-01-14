using System;
using System.Collections.Generic;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using System.IO;
using MediaBrowser.Model.Drawing;

namespace Emby.Plugin.MatrixNotification
{
    /// <summary>
    /// Class Plugin
    /// </summary>
    public class Plugin : BasePlugin, IHasWebPages, IHasThumbImage
    {
        private const string EditorJsName = "matixnotificationeditorjs";

        public IEnumerable<PluginPageInfo> GetPages()
        {
            return new[]
            {
                new PluginPageInfo
                {
                    Name = EditorJsName,
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.entryeditor.js"
                },
                new PluginPageInfo
                {
                    Name = "matrixeditortemplate",
                    EmbeddedResourcePath = GetType().Namespace + ".Configuration.entryeditor.template.html",
                    IsMainConfigPage = false
                }
            };
        }
        
		public string NotificationSetupModuleUrl => GetPluginPageUrl(EditorJsName);

        private Guid _id = new Guid("4C7018C4-2E18-45CA-AE5E-C1DF19AE2952");
        public override Guid Id
        {
            get { return _id; }
        }

        public static string StaticName = "Matrix";

        /// <summary>
        /// Gets the name of the plugin
        /// </summary>
        /// <value>The name.</value>
        public override string Name
        {
            get { return StaticName + " Notifications"; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get
            {
                return "Sends notifications via Matrix Messenger.";
            }
        }

        public Stream GetThumbImage()
        {
            var type = GetType();
            return type.Assembly.GetManifestResourceStream(type.Namespace + ".thumb.png");
        }

        public ImageFormat ThumbImageFormat
        {
            get
            {
                return ImageFormat.Png;
            }
        }

    }
}
