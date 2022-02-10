using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Net;
using MediaBrowser.Model.Logging;
using MediaBrowser.Model.Services;
using Emby.Plugin.MatrixNotification.Configuration;
using MediaBrowser.Model.Serialization;
using System.Net.Http;
using System.Text;


namespace Emby.Plugin.MatrixNotification.Api
{
    [Route("/Notification/Matrix/Test/{UserID}", "POST", Summary = "Tests Matrix")]
    public class TestNotification : IReturnVoid
    {
        [ApiMember(Name = "UserID", Description = "User Id", IsRequired = true, DataType = "string", ParameterType = "path", Verb = "GET")]
        public string UserID { get; set; }
    }

    class ServerApiEndpoints : IService
    {
        private readonly IHttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IJsonSerializer _jsonSerializer;

        public ServerApiEndpoints(ILogManager logManager, IHttpClient httpClient, IJsonSerializer jsonSerializer)
        {
            _logger = logManager.GetLogger(GetType().Name);
            _httpClient = httpClient;
            _jsonSerializer = jsonSerializer;
        }
        private MatrixOptions GetOptions(String userID)
        {
            return Plugin.Instance.Configuration.Options
                .FirstOrDefault(i => string.Equals(i.MediaBrowserUserId, userID, StringComparison.OrdinalIgnoreCase));
        }

        public void Post(TestNotification request)
        {
            var task = PostAsync(request);
            Task.WaitAll(task);
        }

        public async Task PostAsync(TestNotification request)
        {
            var options = GetOptions(request.UserID);

            var parameters = new Dictionary<string, string> { };
            parameters.Add("msgtype", "m.text");
            parameters.Add("body", "This is a Test Message from Emby");
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
    }
}
