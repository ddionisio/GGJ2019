using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Vuforia;

public class Source : MonoBehaviour, ITrackableEventHandler
{
    [Header("Info")]
    public GameObject trackActiveGO;
    
    [Header("Signals")]
    public SignalSource signalTrack;

    public bool trackActive { get; private set; }

    public TrackableBehaviour.Status previousStatus { get; private set; }
    public TrackableBehaviour.Status status { get; private set; }

    protected TrackableBehaviour mTrackableBehaviour;

    void OnDestroy()
    {
        if (mTrackableBehaviour)
            mTrackableBehaviour.UnregisterTrackableEventHandler(this);
    }

    void Awake()
    {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        mTrackableBehaviour.RegisterTrackableEventHandler(this);
    }

    void ITrackableEventHandler.OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        this.previousStatus = previousStatus;
        this.status = newStatus;

        if(trackActiveGO)
            trackActiveGO.SetActive(this.status == TrackableBehaviour.Status.TRACKED);

        if (signalTrack)
            signalTrack.Invoke(this);
    }
}
