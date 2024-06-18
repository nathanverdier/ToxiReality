using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Photo
{
    public class ImportPhotoFromStorageService : IPhotoService
    {
        public async Task<byte[]> CapturePhoto()
        {
            byte[] photoData = await LoadImageData();
            return photoData;
        }

        private async Task<byte[]> LoadImageData()
        {
            string imagePath = "C:/Users/lucie/Downloads/Snapchat-1209362483.jpg";

            if (File.Exists(imagePath))
            {
                byte[] imageData = await File.ReadAllBytesAsync(imagePath);
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