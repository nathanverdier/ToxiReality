using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Assets.Scripts.Api
{
    public class ApiService : MonoBehaviour, IApiService
    {
        public IEnumerator GetAccessToken(Action<string> onSuccess, Action<string> onError)
        {
            WWWForm form = new WWWForm();
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

        public IEnumerator MakeHttpRequest(string accessToken, byte[] imageData, Action<string> onSuccess, Action<string> onError)
        {
            UnityWebRequest webRequest = new UnityWebRequest("http://localhost:8081/v1/identity", "POST");
            webRequest.SetRequestHeader("Content-Type", "image/jpeg");
            webRequest.SetRequestHeader("Authorization", "Bearer " + accessToken);

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(imageData);
            webRequest.uploadHandler = uploadHandler;
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(webRequest.error);
            }
            else
            {
                string responseData = System.Text.Encoding.UTF8.GetString(webRequest.downloadHandler.data);
                onSuccess?.Invoke(responseData);
            }
        }

        [Serializable]
        public class AccessTokenResponse
        {
            public string access_token;
        }
    }
}