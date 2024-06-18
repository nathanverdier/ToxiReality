using Assets.Scripts.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Api
{
    public class ApiFakerService : MonoBehaviour, IApiService
    {
        public IEnumerator GetAccessToken(Action<string> onSuccess, Action<string> onError)
        {
            onSuccess?.Invoke("fake8token");
            yield return "fake8token";
        }

        public IEnumerator MakeHttpRequest(string accessToken, byte[] imageData, Action<string> onSuccess, Action<string> onError)
        {
            yield return new WaitForSeconds(0.5f);
            string fakeJsonResponse = "{ \"face_locations\":[{\"x1\":12,\"x2\":24,\"y1\":24,\"y2\":12}], \"face_names\":[\"Lucie\"] }";
            onSuccess?.Invoke(fakeJsonResponse);
        }

    }
}