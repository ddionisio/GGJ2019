using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventFloat : UnityEvent<float>
{

}

public class SignalFloatProxy : MonoBehaviour
{
    public SignalFloat signal;
    public UnityEventFloat callback;

    void OnDestroy()
    {
        signal.callback -= OnSignal;
    }

    void Awake()
    {
        signal.callback += OnSignal;
    }

    void OnSignal(float f)
    {
        callback.Invoke(f);
    }
}
