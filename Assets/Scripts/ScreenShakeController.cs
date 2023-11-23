using UnityEngine;
using Cinemachine;

//1: attach this script to the virtual camera
//2: enable noise on the virtual camera
//  2.1: find the "Noise" setting on the virtual camera
//  2.2: select "Basic Multi Channel Perlin"
//  2.3: select a noise profile (6D shake is good)
//  2.4: set "Amplitude Gain" and "Frequency Gain" to 0

public class ScreenShakeController : MonoBehaviour
{
    private CinemachineFreeLook freeLookCamera;
    private CinemachineBasicMultiChannelPerlin cameraPerlin;

    [SerializeField] private float startingIntensity;
    [SerializeField] private float shakingFrequency;
    [SerializeField] private float shakeTimerTotal;
    [SerializeField] private float debugShakeTimer;

    private float shakeTimer;

    private void Awake()
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
            debugShakeTimer = shakeTimer;

            cameraPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0.0f, 1 - (shakeTimer / shakeTimerTotal));
        }
    }

    /// <summary>
    /// Shakes the (cinemachine) camera
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name="frequency"></param>
    /// <param name="time"></param>
    private void ShakeCamera(ScreenShakeEvent e)
    {
        cameraPerlin.m_AmplitudeGain = e.Intensity;
        cameraPerlin.m_FrequencyGain = e.Frequency;

        startingIntensity = e.Intensity;
        shakeTimerTotal = e.Duration;
        shakeTimer = e.Duration;
    }
}