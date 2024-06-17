using Assets.Scripts.Photo;
using MixedReality.Toolkit.UX;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows.WebCam;

namespace Assets.Scripts.Photo
{
    public class PhotoCaptureService : IPhotoService
    {
        private PhotoCapture photoCaptureObject = null;
        private TaskCompletionSource<byte[]> photoTaskCompletionSource;

        public async Task<byte[]> CapturePhoto()
        {
            photoTaskCompletionSource = new TaskCompletionSource<byte[]>();
            var initializer = PhotoCaptureObjectInitializer.Instance;
            if (initializer == null)
            {
                Debug.LogError("PhotoCaptureObjectInitializer instance not found.");
                return null;
            }

            photoCaptureObject = initializer.GetPhotoCaptureObject();
            if (photoCaptureObject == null)
            {
                Debug.LogError("PhotoCapture object is not initialized.");
                return null;
            }

            photoTaskCompletionSource = new();

            // Capture the photo
            photoCaptureObject.TakePhotoAsync(OnCapturedPhotoHandler);

            // Await the photo capture to complete
            var photoData = await photoTaskCompletionSource.Task;

            if (photoData == null)
            {
                Debug.LogError("Photo is null, error processing photo");
            }
            return photoData;
        }

        private void OnCapturedPhotoHandler(PhotoCapture.PhotoCaptureResult result, PhotoCaptureFrame photoCaptureFrame)
        {
            if (result.success)
            {
                Debug.Log("Photo captured successfully");
                List<byte> imageBufferList = new();
                Debug.Log("Copy the raw IMFMediaBuffer data into our empty byte list.");
                photoCaptureFrame.CopyRawImageDataIntoBuffer(imageBufferList);

                int stride = 4;
                float denominator = 1.0f / 255.0f;
                Debug.Log("Creating colors array");
                List<Color> colorArray = new();
                for (int i = imageBufferList.Count - 1; i >= 0; i -= stride)
                {
                    float a = (int)(imageBufferList[i - 0]) * denominator;
                    float r = (int)(imageBufferList[i - 1]) * denominator;
                    float g = (int)(imageBufferList[i - 2]) * denominator;
                    float b = (int)(imageBufferList[i - 3]) * denominator;

                    colorArray.Add(new Color(r, g, b, a));
                }

                Debug.Log("Treatment done");
                var photoData = imageBufferList.ToArray();
                photoTaskCompletionSource.SetResult(photoData);
            }
            else
            {
                photoTaskCompletionSource.SetResult(null);
            }
        }
    }
}