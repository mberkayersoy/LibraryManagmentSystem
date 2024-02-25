using Cinemachine;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeListener : MonoBehaviour
{
    //Should Use Basic multi channel perlin' and '6D Shake' on virtual cam.
    [SerializeField] private CinemachineVirtualCamera VirtualCamera;
    [SerializeField] private List<CameraShakeSourceSO> Sources;

    private CinemachineBasicMultiChannelPerlin _multiChannelPerlin;
    private Tween _amplitudeGain;
    private Tween _frequencyGain;

    private void Awake()
    {
        _multiChannelPerlin = VirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void OnEnable()
    {
        ResetNoise();
        foreach (var source in Sources)
        {
            source.Register(this);
        }
    }

    private void OnDisable()
    {
        foreach (var source in Sources)
        {
            source.Unregister(this);
        }
    }

    public void OnShake(CameraShakeSourceSO source)
    {
        UpdateAmplitudeGain(source);
        UpdateFrequencyGain(source);
    }

    private void ResetNoise()
    {
        _multiChannelPerlin.m_AmplitudeGain = 0f;
        _multiChannelPerlin.m_FrequencyGain = 0f;
    }

    private void UpdateAmplitudeGain(CameraShakeSourceSO source)
    {
        _amplitudeGain?.Kill();
        _amplitudeGain = DOVirtual.Float(source.AmplitudeGain, 0f, source.Duration, value =>
        {
            _multiChannelPerlin.m_AmplitudeGain = value;
        }).SetEase(Ease.OutQuad);
    }

    private void UpdateFrequencyGain(CameraShakeSourceSO source)
    {
        _frequencyGain?.Kill();
        _frequencyGain = DOVirtual.Float(source.FrequencyGain, 0f, source.Duration, value =>
        {
            _multiChannelPerlin.m_FrequencyGain = value;
        }).SetEase(Ease.OutQuad);
    }
}