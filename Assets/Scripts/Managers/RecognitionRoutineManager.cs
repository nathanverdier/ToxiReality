using Assets.Scripts.Api;
using Assets.Scripts.Dialog;
using Assets.Scripts.Photo;
using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class RecognitionRoutineManager : MonoBehaviour
    {

        private bool isRepeating = false;
        private float repeatInterval = 0.5f; // Intervalle de 500 ms

        private IApiService _apiService;
        private string accessToken;
        private IPhotoService _photoService;

        private void Start()
        {
            _apiService = FindObjectOfType<ApiService>();
            Debug.Log(_apiService);
            _photoService = new ImportPhotoFromStorageService();
            Debug.Log(_photoService);

            StartCoroutine(GetAccessToken());
        }

        private IEnumerator GetAccessToken()
        {
            yield return _apiService.GetAccessToken(
                token => { accessToken = token; Debug.Log("Access token okay :)"); },
                error => { Debug.LogError("Failed to get access token: " + error); }
            );

            if (string.IsNullOrEmpty(accessToken))
            {
                Debug.LogError("Failed to retrieve access token.");
            }
        }

        public void StartRepeatingAction()
        {
            if (!isRepeating)
            {
                isRepeating = true;
                StartCoroutine(RepeatedAction());
            }
        }

        public void StopRepeatingAction()
        {
            isRepeating = false;
        }

        private IEnumerator RepeatedAction()
        {
            while (isRepeating)
            {
                // Appel de la méthode asynchrone CapturePhoto
                Task<byte[]> captureTask = _photoService.CapturePhoto();

                // Attendre que la tâche se termine
                while (!captureTask.IsCompleted)
                {
                    yield return null;
                }

                byte[] photoData = captureTask.Result;

                if (photoData != null)
                {
                    Debug.Log("Photo captured successfully!");
                    // Make API call
                    StartCoroutine(_apiService.MakeHttpRequest(
                        accessToken,
                        photoData,
                        response => { Debug.Log("Request succeeded: " + response); },
                        error => { Debug.LogError("Request failed: " + error); }
                    ));
                }
                else
                {
                    Debug.LogError("Failed to capture photo.");
                }

                yield return new WaitForSeconds(repeatInterval);
            }
        }

    }
}