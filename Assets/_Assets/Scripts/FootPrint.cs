using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPrint : MonoBehaviour
{
    public float timeNo;
    void Start()
    {
        StartCoroutine(DestoryIEnum());
    }

    IEnumerator DestoryIEnum()
    {
        yield return new WaitForSeconds(timeNo);
        Destroy(gameObject);
    }
}
