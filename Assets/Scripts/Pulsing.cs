using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulsing : MonoBehaviour
{

    private float maxScale = 0.9f;
    private float minScale = 0.7f;

    private bool toMax = true;
    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (toMax)
            {
                Vector3 oldPos = gameObject.transform.position;
                gameObject.transform.localScale += new Vector3(0.02f, 0.02f, 0);
                gameObject.transform.position = oldPos;
                if (gameObject.transform.localScale.x >= maxScale && gameObject.transform.localScale.y >= maxScale)
                {
                    toMax = false;
                }
            }
            else
            {
                Vector3 oldPos = gameObject.transform.position;
                gameObject.transform.localScale -= new Vector3(0.02f, 0.02f, 0);
                gameObject.transform.position = oldPos;
                if (gameObject.transform.localScale.x <= minScale && gameObject.transform.localScale.y <= minScale)
                {
                    toMax = true;
                }
            }
        }
    }
}
