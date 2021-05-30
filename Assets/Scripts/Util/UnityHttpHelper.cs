using System.Text;
using UnityEngine.Networking;

namespace Assets.Scripts.Util
{
    public static class UnityHttpHelper
    {
        public static UnityWebRequest Post(string url, string json)
        {
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            var data = Encoding.ASCII.GetBytes(json);
            var uploadHandler = new UploadHandlerRaw(data) { contentType = "application/json" };
            request.uploadHandler = uploadHandler;
            request.downloadHandler = new DownloadHandlerBuffer();
            return request;
        }

        public static UnityWebRequest Get(string url, string bearerToken)
        {
            var request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            request.SetRequestHeader("Authorization", $"Bearer {bearerToken}");
            request.downloadHandler = new DownloadHandlerBuffer();
            return request;
        }
    }
}
