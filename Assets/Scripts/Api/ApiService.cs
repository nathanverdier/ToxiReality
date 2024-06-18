using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Api
{
    public class ApiService : MonoBehaviour, IApiService
    {
        public IEnumerator GetAccessToken(Action<string> onSuccess, Action<string> onError)
        {
            WWWForm form = new();
            form.AddField("client_id", "eAf8z4vS3B47LGatTn34q38IRdJrNNvc");
            form.AddField("client_secret", "n_oChtKZlpXG60n-N1V4LwXkaFYbvRGoWT-4Lsk8nwPo3aXIEONevoHUa8uMEN4g");
            form.AddField("audience", "https://toxiapi/");
            form.AddField("grant_type", "client_credentials");

            using (UnityWebRequest webRequest = UnityWebRequest.Post("https://dev-3ja73wpfp1j6uzed.us.auth0.com/oauth/token", form))
            {
                webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                yield return webRequest.SendWebRequest();

                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    onError?.Invoke(webRequest.error);
                }
                else
                {
                    string responseJson = webRequest.downloadHandler.text;
                    AccessTokenResponse response = JsonUtility.FromJson<AccessTokenResponse>(responseJson);
                    onSuccess?.Invoke(response.access_token);
                }
            }
        }

        //public IEnumerator MakeHttpRequest(string accessToken, byte[] imageData, Action<string> onSuccess, Action<string> onError)
        //{
        //    List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //    formData.Add(new MultipartFormFileSection("image", imageData, "image.jpg", "image/jpeg"));

        //    UnityWebRequest webRequest = UnityWebRequest.Post("https://codefirst.iut.uca.fr/containers/ToxiTeam-toxi-api/v1/identity", formData);
        //    webRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);
        //    webRequest.downloadHandler = new DownloadHandlerBuffer();

        //    yield return webRequest.SendWebRequest();

        //    if (webRequest.result != UnityWebRequest.Result.Success)
        //    {
        //        onError?.Invoke(webRequest.error);
        //    }
        //    else
        //    {
        //        string responseData = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
        //        onSuccess?.Invoke(responseData);
        //    }
        //}

        public IEnumerator MakeHttpRequest(string accessToken, byte[] imageData, Action<string> onSuccess, Action<string> onError)
        {
            string url = "https://codefirst.iut.uca.fr/containers/ToxiTeam-toxi-api/v1/identity";

            string boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;
            webRequest.Method = "POST";
            webRequest.Headers["Authorization"] = "Bearer " + accessToken;

            // Construct the body of the request
            string boundaryLine = "--" + boundary + "\r\n";
            string contentDisposition = "Content-Disposition: form-data; name=\"image\"; filename=\"image.jpg\"\r\n";
            string contentType = "Content-Type: image/jpeg\r\n\r\n";

            byte[] boundaryBytes = Encoding.UTF8.GetBytes(boundaryLine);
            byte[] dispositionBytes = Encoding.UTF8.GetBytes(contentDisposition);
            byte[] typeBytes = Encoding.UTF8.GetBytes(contentType);
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            byte[] postData;
            using (MemoryStream postStream = new())
            {
                postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                postStream.Write(dispositionBytes, 0, dispositionBytes.Length);
                postStream.Write(typeBytes, 0, typeBytes.Length);
                postStream.Write(imageData, 0, imageData.Length);
                postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                postData = postStream.ToArray();
            }

            webRequest.ContentLength = postData.Length;

            try
            {
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(postData, 0, postData.Length);
                }

                using (WebResponse response = webRequest.GetResponse())
                {
                    using (StreamReader reader = new(response.GetResponseStream()))
                    {
                        string responseData = reader.ReadToEnd();
                        onSuccess?.Invoke(responseData);
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (StreamReader reader = new(ex.Response.GetResponseStream()))
                    {
                        string errorResponse = reader.ReadToEnd();
                        onError?.Invoke(errorResponse);
                    }
                }
                else
                {
                    onError?.Invoke(ex.Message);
                }
            }

            yield return null;
        }


        [Serializable]
        public class AccessTokenResponse
        {
            public string access_token;
        }
    }
}