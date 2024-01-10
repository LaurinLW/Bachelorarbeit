using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysInFront : MonoBehaviour
{
    // Set the sorting layer and order in layer in the Inspector.
    public string sortingLayerName = "Foreground";
    public int orderInLayer = 999;

    void Start()
    {
        // Set the sorting layer and order in layer programmatically.
        GetComponent<Renderer>().sortingLayerName = sortingLayerName;
        GetComponent<Renderer>().sortingOrder = orderInLayer;
    }
}
