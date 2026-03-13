using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public ThirdPersonCamera cameraSettings;

    public void OnSlideSensitivity(float value)
    {
        cameraSettings.mouseSensitivity = 0.3f * value;
    }
}
