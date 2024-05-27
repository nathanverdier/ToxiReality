using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Api
{
    public interface IApiService
    {
        IEnumerator GetAccessToken(Action<string> onSuccess, Action<string> onError);
        IEnumerator MakeHttpRequest(string accessToken, byte[] imageData, Action<string> onSuccess, Action<string> onError);
    }
}