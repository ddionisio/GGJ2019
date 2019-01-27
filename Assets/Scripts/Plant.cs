using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour, M8.IPoolSpawn
{
    public const string parmSourceSun = "sun";
    
    public M8.Animator.Animate animator;
    [M8.Animator.TakeSelector(animatorField = "animator")]
    public string take;

    public float growthDelay = 1f;

    public M8.RangeFloat sunDistanceRange;
    public M8.RangeFloat scaleRange;
    
    public float curGrowthTime { get { return mCurGrowthTime; } }

    private float mCurGrowthTime;
    private float mCurGrowthTimeScale;
    private float mAnimScale;
    
    private float mCurDelay;

    private Source mSourceSun;
    
    void Update()
    {
        /*if (mSourceSun.status != Vuforia.TrackableBehaviour.Status.TRACKED)
            mCurGrowthTimeScale = 0f;
        else
        {
            var p1 = new Vector2(transform.position.x, transform.position.z);
            var p2 = new Vector2(mSourceSun.transform.position.x, mSourceSun.transform.position.z);

            var len = (p2 - p1).magnitude;

            float newScale = (len - sunDistanceRange.min) / sunDistanceRange.length;
            if (newScale < 0f) newScale = 0f;
            else if (newScale > 1f) newScale = 1f;

            mCurGrowthTimeScale = scaleRange.Lerp(newScale);
        }

        float curTime = mCurGrowthTime + Time.deltaTime * mCurGrowthTimeScale;
        if (mCurGrowthTime != curTime)
        {
            mCurGrowthTime = curTime;

            mAnimScale = mCurGrowthTime / growthDelay;

            if (mAnimScale < 0f) mAnimScale = 0f;
            else if (mAnimScale > 1f) mAnimScale = 1f;

            if(animator && !string.IsNullOrEmpty(take))
                animator.Goto(take, mAnimScale * animator.GetTakeTotalTime(take));
        }*/
    }

    void M8.IPoolSpawn.OnSpawned(M8.GenericParams parms)
    {
        mSourceSun = parms.GetValue<Source>(parmSourceSun);

        if (animator && !string.IsNullOrEmpty(take))
            //animator.ResetTake(take);
            animator.Play(take);
    }
}
