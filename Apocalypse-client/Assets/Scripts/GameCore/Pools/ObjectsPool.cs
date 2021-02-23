using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool : MonoBehaviour
{
    [SerializeField]
    private GameObject mPrefab;
    private Queue<GameObject> mPoolInstanceQueue = new Queue<GameObject>();

    public GameObject GetInstance()
    {
        if (mPoolInstanceQueue.Count > 0)
        {
            GameObject rReuseInstance = mPoolInstanceQueue.Dequeue();
            rReuseInstance.SetActive(true);
            return rReuseInstance;
        }

        return Instantiate(mPrefab);
    }

    public void RecoveryInstance(GameObject rObj)
    {
        mPoolInstanceQueue.Enqueue(rObj);
        rObj.SetActive(false);
        rObj.transform.SetParent(this.transform);
    }

}
