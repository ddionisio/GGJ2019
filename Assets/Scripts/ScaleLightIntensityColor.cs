using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleLightIntensityColor : MonoBehaviour
{
    public Light lightSource;
    public Source source;

    public Color[] colorRange = new Color[] { Color.white };
    public M8.RangeFloat intensityRange;

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
        if (source && source.status == Vuforia.TrackableBehaviour.Status.TRACKED)
        {
            lightSource.color = M8.ColorUtil.Lerp(colorRange, s);
            lightSource.intensity = intensityRange.Lerp(s);
        }
        else
        {
            lightSource.color = colorRange[colorRange.Length - 1];
            lightSource.intensity = intensityRange.max;
        }
    }
}
