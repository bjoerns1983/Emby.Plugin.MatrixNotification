using System;
using System.Collections.Generic;
using MediaBrowser.Model.Plugins;

namespace Emby.Plugin.MatrixNotification.Configuration
{
    /// <summary>
    /// Class PluginConfiguration
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        public MatrixOptions[] Options { get; set; }

        public PluginConfiguration()
        {
            Options = new MatrixOptions[] { };
        }
    }

    public class MatrixOptions
    {
        public Boolean Enabled { get; set; }
        public String RoomID { get; set; }
        public String ClientToken { get; set; }
        public String ServerAddress { get; set; }
        public Boolean SendDescription { get; set; }
        public String DeviceName { get; set; }
        public int Priority { get; set; }
        public string MediaBrowserUserId { get; set; }


    }


}
