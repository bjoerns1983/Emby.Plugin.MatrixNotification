using System.Collections.Generic;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Notifications;
using MediaBrowser.Model.Logging;
using Emby.Plugin.MatrixNotification.Configuration;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Serialization;

namespace Emby.Plugin.MatrixNotification
{
    public class Notifier : INotificationService
    {
        private readonly ILogger _logger;
        private readonly IHttpClient _httpClient;
        private readonly IJsonSerializer _jsonSerializer;

        public Notifier(ILogManager logManager, IHttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }

        public bool IsEnabledForUser(User user)
        {
            var options = GetOptions(user);

            return options != null && IsValid(options) && options.Enabled;
            //return options != null  && options.Enabled;
        }

        private MatrixOptions GetOptions(User user)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, user.Id.ToString("N"), StringComparison.OrdinalIgnoreCase));
        }

        public string Name
        {
            get { return Plugin.Instance.Name; }
        }

        public async Task SendNotification(UserNotification request, CancellationToken cancellationToken)
        {

            var options = GetOptions(request.User);
            string message = (request.Name);

            if (string.IsNullOrEmpty(request.Description) == false && options.SendDescription == true)
            {
                message = (request.Name + "\n\n" + request.Description); 
            }

            //_logger.Debug("Matrix to Token : {0} - {1} - {2}", options.ClientToken, options.RoomID, request.Name);


            var parameters = new Dictionary<string, string> { };
            parameters.Add("msgtype", "m.text");
            parameters.Add("body", message);
            string json = _jsonSerializer.SerializeToString(parameters);

            char[] content = json.ToCharArray();

            var httpRequestOptions = new HttpRequestOptions
            {
                Url = options.ServerAddress + "_matrix/client/r0/rooms/" + Uri.EscapeDataString(options.RoomID) + "/send/m.room.message?access_token=" + options.ClientToken,
                CancellationToken = CancellationToken.None
            };
            httpRequestOptions.RequestContentType = "application/json";
            httpRequestOptions.RequestContent = content;


            using (await _httpClient.Post(httpRequestOptions).ConfigureAwait(false))
            {

            }


        }

        private bool IsValid(MatrixOptions options)
        {
            return !string.IsNullOrEmpty(options.RoomID) &&
                !string.IsNullOrEmpty(options.ClientToken) &&
                !string.IsNullOrEmpty(options.ServerAddress);
        }
    }
}
