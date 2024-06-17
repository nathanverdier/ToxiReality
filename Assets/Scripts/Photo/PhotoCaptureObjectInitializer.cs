using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class PhotoCaptureObjectInitializer : MonoBehaviour
{
    public static PhotoCaptureObjectInitializer Instance { get; private set; }
    public PhotoCapture photoCaptureObject = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        // check camera permission
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("Camera permission denied.");
            return;
        }
        Debug.Log("Camera authorization granted");
        InitializePhotoCapture();
    }

    public void InitializePhotoCapture()
    {
        // Create PhotoCapture object
        Debug.Log("Create photo capture object");
        PhotoCapture.CreateAsync(false, OnCaptureObjectCreated);
    }

    void OnCaptureObjectCreated(PhotoCapture captureObject)
    {
        photoCaptureObject = captureObject;

        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();

        CameraParameters c = new CameraParameters();
        c.hologramOpacity = 0.0f;
        c.cameraResolutionWidth = cameraResolution.width;
        c.cameraResolutionHeight = cameraResolution.height;
        c.pixelFormat = CapturePixelFormat.BGRA32;

        Debug.Log("Photo capture object created, start photo mode");
        captureObject.StartPhotoModeAsync(c, OnPhotoModeStarted);
    }

    private void OnPhotoModeStarted(PhotoCapture.PhotoCaptureResult result)
    {
        if (result.success)
        {
            Debug.Log("photo mode started, ready to capture photo");
        }
        else
        {
            Debug.Log("Unable to start photo mode!");
        }
    }

    // Util method to get the PhotoCapture object
    public PhotoCapture GetPhotoCaptureObject()
    {
        return photoCaptureObject;
    }
}
