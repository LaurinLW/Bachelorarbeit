using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryMe : MonoBehaviour
{
    void Update()
    {
        if (gameObject.transform.position.z < -90)
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}
