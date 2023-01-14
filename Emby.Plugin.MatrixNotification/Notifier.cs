using System.Collections.Generic;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Serialization;
using Emby.Notifications;
using MediaBrowser.Controller;

namespace Emby.Plugin.MatrixNotification
{
    public class Notifier : IUserNotifier
    {
        private readonly ILogger _logger;
        private IServerApplicationHost _appHost;
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        public Notifier(ILogger logger, IServerApplicationHost applicationHost, IHttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _appHost = applicationHost;
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }

        private Plugin Plugin => _appHost.Plugins.OfType<Plugin>().First();

        public string Name => Plugin.StaticName;

        public string Key => "matrixnotifications";

        public string SetupModuleUrl => Plugin.NotificationSetupModuleUrl;

        public async Task SendNotification(InternalNotificationRequest request, CancellationToken cancellationToken)
        {

            var options = request.Configuration.Options;

            options.TryGetValue("RoomID", out string roomID);
            options.TryGetValue("ClientToken", out string clientToken);
            options.TryGetValue("ServerAddress", out string serverAddress);


            string message = (request.Title);

            if (string.IsNullOrEmpty(request.Description) == false)
            {
                message = (request.Title + "\n\n" + request.Description);
            }

            var parameters = new Dictionary<string, string> { };
            parameters.Add("msgtype", "m.text");
            parameters.Add("body", message);
            string json = _jsonSerializer.SerializeToString(parameters);

            char[] content = json.ToCharArray();

            var httpRequestOptions = new HttpRequestOptions
            {
                Url = serverAddress + "_matrix/client/r0/rooms/" + Uri.EscapeDataString(roomID) + "/send/m.room.message?access_token=" + clientToken,
                CancellationToken = cancellationToken
            };
            httpRequestOptions.RequestContentType = "application/json";
            httpRequestOptions.RequestContent = content;


            using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
            {

            }


        }

    }
}
