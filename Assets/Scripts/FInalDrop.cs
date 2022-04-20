using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FInalDrop : MonoBehaviour
{
    enum audienceType
    {
        cloud, fire
    }

    [SerializeField]
    GameObject finalDropPrefab;
    [SerializeField]
    Transform dropTrans;

    Vector3 dropPos;
    [SerializeField]
    float footStep = 1.0f;

    [SerializeField]
    audienceType type;


    public int cnt;
    // Start is called before the first frame update
    void Start()
    {
        dropPos = dropTrans.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator showResult()
    {
        if (type == audienceType.cloud)
        {
            cnt = GameManager.Instance.cloudCnt;
        }
        else
        {
            cnt = GameManager.Instance.fireCnt;
        }

        for (int i = 0; i < cnt; i++)
        {
            yield return StartCoroutine(dropPrefabs());
        }
    }

    IEnumerator dropPrefabs()
    {
        Vector3 startPoint = dropPos;
        startPoint.y = 0.0f;
        GameObject newObj = Instantiate(finalDropPrefab, startPoint, Quaternion.identity) as GameObject;
        float startTime = Time.time;
        while (true)
        {
            float a = (Time.time - startTime);
            if (a < 1)
            {
                newObj.transform.position = Vector3.Lerp(startPoint, dropPos, a);
                //transform.rotation = Quaternion.Slerp(_startRotation, _endRotation, a);
            }
            else if (a >= 1f)
            {
                dropPos += ((int)type - 0.5f) * 2 * dropTrans.up * footStep;
                yield break;
            }
            yield return null;
        }
        

    }
}
