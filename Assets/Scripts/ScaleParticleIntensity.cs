using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleParticleIntensity : MonoBehaviour
{
    public ParticleSystem particleSource;

    public M8.RangeFloat startDelayMultRange;

    public SignalFloat signalScaleChanged;

    void OnDestroy()
    {
        signalScaleChanged.callback -= OnSignalScaleChanged;
    }

    void Awake()
    {
        OnSignalScaleChanged(0f);

        signalScaleChanged.callback += OnSignalScaleChanged;
    }

    void OnSignalScaleChanged(float s)
    {
        var dat = particleSource.main;

        if (s > 0f)
        {
            dat.startDelayMultiplier = startDelayMultRange.Lerp(s);
            dat.loop = true;

            if (!particleSource.isPlaying)
                particleSource.Play(false);
        }
        else
        {
            particleSource.Stop();
        }
    }
}
