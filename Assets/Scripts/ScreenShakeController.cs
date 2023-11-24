using UnityEngine;
using Cinemachine;

//1: attach this script to the freelook camera (prefab/object called cinemachine)
//2: enable noise on the freelook camera
//  2.1: scroll down to the different rig settings (top rig, middle rig, bottom rig)
//  2.2: select the top rig
//  2.3: click on the noise setting
//  2.4: select "Basic Multi Channel Perlin"
//  2.5: select a noise profile (6D shake is good)
//  2.6: set "Amplitude Gain" and "Frequency Gain" to 0

public class ScreenShakeController : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;
    private CinemachineBasicMultiChannelPerlin cameraPerlin;

    private float startingIntensity;
    private float shakingFrequency;
    private float shakeTimerTotal;
    private float shakeTimer;

    private void Start()
    {
        freeLookCamera = GetComponent<CinemachineFreeLook>();
        cameraPerlin = freeLookCamera.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        EventManager.GetInstance().AddListener<ScreenShakeEvent>(ShakeCamera);
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;

            cameraPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0.0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }

    private void ShakeCamera(ScreenShakeEvent e)
    {
        cameraPerlin.m_AmplitudeGain = e.Intensity;
        cameraPerlin.m_FrequencyGain = e.Frequency;

        startingIntensity = e.Intensity;
        shakeTimerTotal = e.Duration;
        shakeTimer = e.Duration;
    }
}