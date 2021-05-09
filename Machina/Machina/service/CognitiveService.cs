using Machina.model;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Machina.service
{
    public static class CognitiveService
    {
        // CLEF d'API
        // URL (ENDPOINT)
        private static readonly string API_KEY = "987c7af07c8f497eb7d6a210a7c093e2";
        private static readonly string ENDPOINT_URL = "https://jrcognitiveface.cognitiveservices.azure.com/" + "face/v1.0/";

        public static async Task<FaceDetectResult> FaceDetect(Stream imageStream)
        {
            if (imageStream == null)
            {
                return null;
            }

            var url = ENDPOINT_URL + "detect" + "?returnFaceAttributes=age,gender";

            // Header
            // Body

            using (var webClient = new WebClient())
            {
                try
                {
                    webClient.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
                    webClient.Headers.Add("Ocp-Apim-Subscription-Key", API_KEY);


                    var data = ReadStream(imageStream);

                    var result = await Task.Run(() => webClient.UploadData(url, data) );
                    
                    //var result = webClient.UploadData(url, data);

                    if (result == null)
                    {
                        return null;
                    }

                    string json = Encoding.UTF8.GetString(result, 0, result.Length);

                    var faceResult = Newtonsoft.Json.JsonConvert.DeserializeObject<FaceDetectResult[]>(json);

                    if (faceResult.Length >= 1)
                    {
                        return faceResult[0];
                    }

                    Console.WriteLine("Réponse OK : " + json);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception webclient : " + ex.Message);
                }

                return null;
            }


        }

        private static byte[] ReadStream(Stream input)
        {
            BinaryReader b = new BinaryReader(input);
            byte[] data = b.ReadBytes((int)input.Length);
            return data;
        }

    }
}
