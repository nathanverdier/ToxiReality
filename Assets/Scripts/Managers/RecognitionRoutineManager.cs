using Assets.Scripts.Api;
using Assets.Scripts.Photo;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    [System.Serializable]
    public class Location
    {
        public int x1;
        public int x2;
        public int y1;
        public int y2;
    }

    [System.Serializable]
    public class ApiResponse
    {
        public List<Location> face_locations;
        public List<string> face_names;
    }

    public class RecognitionRoutineManager : MonoBehaviour
    {
        public GameObject cubePrefab;

        private bool isRepeating = false;
        private float repeatInterval = 5f; // Intervalle de 500 ms

        private IApiService _apiService;
        private string accessToken;
        private IPhotoService _photoService;

        private void Start()
        {
            _apiService = FindObjectOfType<ApiService>();
            Debug.Log(_apiService);
            _photoService = new PhotoCaptureService();
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
                        response => 
                        { 
                            Debug.Log("Request succeeded: " + response); 
                            HandleApiResponse(response); 
                        },
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
        private void HandleApiResponse(string response)
        {
            ApiResponse apiResponse = JsonUtility.FromJson<ApiResponse>(response);
            Debug.Log(apiResponse.ToString());

            if (apiResponse.face_locations != null && apiResponse.face_names != null &&
                apiResponse.face_locations.Count == apiResponse.face_names.Count)
            {
                CreateAssetsAndTexts(apiResponse.face_locations, apiResponse.face_names);
            }
        }

        private void CreateAssetsAndTexts(List<Location> faceLocations, List<string> faceNames)
        {
            Debug.Log("count : " + faceLocations.Count);
            for (int i = 0; i < faceLocations.Count; i++)
            {
                Location location = faceLocations[i];
                string name = faceNames[i];

                // int x = (location.x1 + location.x2) / 2;
                // int y = (location.y1 + location.y2) / 2;
                int x = 0;
                int y = 0;

                GameObject pc = (GameObject)Instantiate(cubePrefab, new Vector3(x, y, 10), Quaternion.identity);
                pc.transform.Rotate(new Vector3(-90, 180, -90));

                // Create text next to the cube
                GameObject textObject = new GameObject("NameText");
                textObject.transform.position = new Vector3(0, 0, 2) + Vector3.up * 1f; // Position the text slightly above the cube

                TextMesh textMesh = textObject.AddComponent<TextMesh>();
                textMesh.text = name;
                textMesh.fontSize = 1;
                textMesh.color = Color.white;
            }
        }
    }
}