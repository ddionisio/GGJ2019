using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vuforia;

public enum SourceType
{
    Sun,
    Cloud,
    Tree,

    Count
}

[System.Serializable]
public class SourceInfo
{
    public string name;
    public Source source;
    public M8.RangeFloat range;
    public Color color = Color.white;
    public SignalFloat signalScaleChanged;

    public Transform origin { get; private set; }

    public float scale { get; private set; }

    public void Init(Transform aOrigin)
    {
        origin = aOrigin;
        scale = 0f;

        source.signalTrack.callback += OnSignalTrackUpdate;
    }

    public void Deinit()
    {
        source.signalTrack.callback -= OnSignalTrackUpdate;
    }

    public void Update()
    {
        float newScale;
        if (source.status == Vuforia.TrackableBehaviour.Status.TRACKED)
        {
            var p1 = new Vector2(origin.position.x, origin.position.z);
            var p2 = new Vector2(source.transform.position.x, source.transform.position.z);

            var len = (p2 - p1).magnitude;

            newScale = (len - range.min) / range.length;
            if (newScale < 0f) newScale = 0f;
            else if (newScale > 1f) newScale = 1f;
        }
        else
            newScale = 0f;

        if (scale != newScale)
        {
            scale = newScale;
            if (signalScaleChanged)
                signalScaleChanged.Invoke(scale);
        }
    }

    void OnSignalTrackUpdate(Source src)
    {
        if(src.status == TrackableBehaviour.Status.TRACKED)
        {
            if (signalScaleChanged)
                signalScaleChanged.Invoke(scale);
        }
    }
}

public class Home : MonoBehaviour, ITrackableEventHandler
{
    public SourceInfo[] sources;

    public int debugGizmoInd = 0;

    public GameObject activeGO;

    protected TrackableBehaviour mTrackableBehaviour;

    void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].Deinit();
        }
    }

    void Awake()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        mTrackableBehaviour.RegisterTrackableEventHandler(this);

        for (int i = 0; i < sources.Length; i++)
        {
            sources[i].Init(transform);
        }

        activeGO.SetActive(false);
    }

    void Update()
    {
        for(int i = 0; i < sources.Length; i++)
        {
            sources[i].Update();
        }
    }

    void ITrackableEventHandler.OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        if (activeGO)
            activeGO.SetActive(newStatus == TrackableBehaviour.Status.TRACKED);
    }

    void OnDrawGizmos()
    {
        if (debugGizmoInd >= 0 && debugGizmoInd <= sources.Length - 1)
        {
            var src = sources[debugGizmoInd];
            Gizmos.color = src.color;
            Gizmos.DrawWireSphere(transform.position, src.range.min);
            Gizmos.DrawWireSphere(transform.position, src.range.max);
        }
    }
}
