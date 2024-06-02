using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Photo
{
    public interface IPhotoService
    {
        Task<byte[]> CapturePhoto();
    }
}