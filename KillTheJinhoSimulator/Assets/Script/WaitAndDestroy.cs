using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAndDestroy : MonoBehaviour
{
    [SerializeField]
    private float DestroyDelay = 0f;
    void Start()
    {
        StartCoroutine(WaitDestroy());
    }
    private IEnumerator WaitDestroy() {

        yield return new WaitForSeconds(DestroyDelay);
        Destroy(gameObject);
    }
}
