using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Photo
{
    public class PhotoService : IPhotoService
    {
        public void CapturePhoto(Action<byte[]> onPhotoCaptured)
        {
            byte[] photoData = LoadImageData();
            onPhotoCaptured?.Invoke(photoData);
        }

        private byte[] LoadImageData()
        {
            string imagePath = "C:/Users/lucie/Pictures/Screenshots/Screenshot 2024-02-18 215404.png";
            if (System.IO.File.Exists(imagePath))
            {
                byte[] imageData = System.IO.File.ReadAllBytes(imagePath);
                Debug.Log("Image loaded successfully. Size: " + imageData.Length + " bytes");
                return imageData;
            }
            else
            {
                Debug.LogError("Image file not found at path: " + imagePath);
                return null;
            }
        }
    }
}