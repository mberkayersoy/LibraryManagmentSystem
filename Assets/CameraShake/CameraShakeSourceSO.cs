using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeSourceSO", menuName = "Camera Shake Source")]
public class CameraShakeSourceSO : ScriptableObject
{
    public float AmplitudeGain;
    public float FrequencyGain;
    public float Duration;
    private List<CameraShakeListener> Listeners = new List<CameraShakeListener>();

    public void Register(CameraShakeListener listener)
    {
        Listeners.Add(listener);
    }

    public void Unregister(CameraShakeListener listener)
    {
        Listeners.Remove(listener);
    }

    public void Shake()
    {
        foreach (var listener in Listeners)
        {
            listener.OnShake(this);
        }
    }
}
