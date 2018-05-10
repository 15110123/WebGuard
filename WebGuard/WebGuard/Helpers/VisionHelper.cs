using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebGuard.Helpers
{
    public sealed class VisionHelper
    {
        private readonly string _subscriptionKey;
        private readonly string _urlBase;
        // Request parameters. A third optional parameter is "details".
        private readonly string _requestParameters;

        public VisionHelper(string requestParameters = "", string urlBase = "https://southeastasia.api.cognitive.microsoft.com/vision/v1.0/analyze", string subscriptionKey = "4b243d7d816048d9ac710b944e746ba0")
        {
            _requestParameters = requestParameters;
            _urlBase = urlBase;
            _subscriptionKey = subscriptionKey;
        }


        public async Task<dynamic> MakeAnalysisRequest<T>(string imageFilePath)
        {
            var client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);

            // Assemble the URI for the REST API Call.
            var uri = _urlBase + "?" + _requestParameters;

            // Request body. Posts a locally stored JPEG image.
            var byteData = GetImageAsByteArray(imageFilePath);

            using (var content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json" and "multipart/form-data".
                content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                var response = await (await client.PostAsync(uri, content))
                    .Content
                    .ReadAsStringAsync();

                return typeof(T) == typeof(string) ?
                    response :
                    JsonConvert.DeserializeObject(response);
            }
        }

        private static byte[] GetImageAsByteArray(string imageFilePath)
        {
            var fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read);
            var binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }
}
