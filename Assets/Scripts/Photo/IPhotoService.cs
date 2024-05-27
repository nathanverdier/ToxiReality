using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Photo
{
    public interface IPhotoService
    {
        void CapturePhoto(Action<byte[]> onPhotoCaptured);
    }
}