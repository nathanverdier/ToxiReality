using Assets.Scripts.Api;
using Assets.Scripts.Dialog;
using Assets.Scripts.Photo;
using MixedReality.Toolkit.UX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class DialogManager : MonoBehaviour
    {
        private IDialogService _dialogService;
        private IApiService _apiService;
        private IPhotoService _photoService;

        private void Start()
        {
            var dialogPool = FindObjectOfType<DialogPool>();
            if (dialogPool != null)
            {
                _dialogService = new DialogService(dialogPool);
                _apiService = FindObjectOfType<ApiService>();
                Debug.Log(_apiService);
                _photoService = new PhotoService();
                Debug.Log(_photoService);

                if (_apiService == null)
                {
                    Debug.LogError("ApiService component not found in the scene.");
                }
                else
                {
                    _dialogService.ShowDialog("Welcome to Lucie's App!", "This app is a photo capture test for my study project ToxiReality", "Good Luck", OnOkButtonClicked);
                }
            }
            else
            {
                Debug.LogError("DialogPool not assigned in DialogManager!");
            }
        }

        private void OnOkButtonClicked()
        {
            _photoService.CapturePhoto(photoData =>
            {
                if (photoData != null)
                {
                    StartCoroutine(_apiService.GetAccessToken(accessToken =>
                    {
                        StartCoroutine(_apiService.MakeHttpRequest(accessToken, photoData, response =>
                        {
                            Debug.Log("Response: " + response);
                        }, error =>
                        {
                            Debug.LogError("HTTP Request Error: " + error);
                        }));
                    }, error =>
                    {
                        Debug.LogError("Token Request Error: " + error);
                    }));
                }
                else
                {
                    Debug.LogError("Photo capture failed.");
                }
            });
        }
    }

}