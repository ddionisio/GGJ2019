using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantSpawn : MonoBehaviour
{
    public string poolName;
    public GameObject template;

    public Transform root;

    public Source sourceSun;
    public Source sourceTree;

    public M8.RangeInt countRange;

    public SignalFloat signalTreeChanged;

    private M8.PoolController mPoolCtrl;

    private M8.CacheList<Transform> mSpawnPts;
    private M8.CacheList<M8.PoolDataController> mPlantActives;

    private int mCurCount = 0;

    void OnDestroy()
    {
        signalTreeChanged.callback -= OnScaleTreeChange;
    }

    void Awake()
    {
        mSpawnPts = new M8.CacheList<Transform>(root.childCount);
        for (int i = 0; i < root.childCount; i++)
            mSpawnPts.Add(root.GetChild(i));

        mPlantActives = new M8.CacheList<M8.PoolDataController>(root.childCount);

        mPoolCtrl = M8.PoolController.CreatePool(poolName);
        mPoolCtrl.AddType(template, mSpawnPts.Capacity, mSpawnPts.Capacity);

        signalTreeChanged.callback += OnScaleTreeChange;
    }

    void Update()
    {
        var activeCount = mPlantActives.Count;
        if (activeCount > mCurCount)
        {
            int count = activeCount - mCurCount;
            for (int i = 0; i < count; i++)
            {
                var pdc = mPlantActives.RemoveLast();
                mSpawnPts.Add(pdc.transform.parent);
                pdc.Release();
            }
        }
        else if(activeCount < mCurCount)
        {
            int count = mCurCount - activeCount;
            for(int i = 0; i < count; i++)
            {
                int spawnToInd = Random.Range(0, mSpawnPts.Count);
                var spawnToTrans = mSpawnPts[spawnToInd];
                mSpawnPts.RemoveAt(spawnToInd);

                var parms = new M8.GenericParams();
                parms[Plant.parmSourceSun] = sourceSun;

                var pdc = mPoolCtrl.Spawn(template.name, "plant", spawnToTrans, parms);
                pdc.transform.localPosition = Vector3.zero;
                pdc.transform.localScale = Vector3.one;

                mPlantActives.Add(pdc);
            }
        }
    }

    void OnScaleTreeChange(float s)
    {
        if (sourceTree.status == Vuforia.TrackableBehaviour.Status.TRACKED)
        {
            mCurCount = countRange.Lerp(s);
        }
        else
            mCurCount = 0;
    }
}
