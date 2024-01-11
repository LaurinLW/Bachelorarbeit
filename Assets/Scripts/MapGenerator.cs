using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject leftPart;
    public GameObject rightPart;
    public GameObject jumpPart;
    public GameObject mapObject;

    public Vector3 moveBack = new Vector3(0, 0, 0.1f);

    private long seed;
    private List<GameObject> map;

    public int conLength = 10;

    GameObject giveRandomMapPart()
    {
        switch (UnityEngine.Random.Range(0, 3))
        {
            case 0:
                return leftPart;
            case 1:
                return rightPart;
            case 2:
                return jumpPart;
            default:
                return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DateTime currentDateTime = DateTime.Now;
        seed = currentDateTime.Ticks;
        UnityEngine.Random.InitState((int)seed);
        map = new List<GameObject>();
        for (int i = 0; i < conLength; i++)
        {
            GameObject part = GameObject.Instantiate(giveRandomMapPart());
            part.transform.position = new Vector3(0, 0, 100 * i);
            part.transform.parent = mapObject.transform;
            map.Add(part);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject toRemove = null;
        foreach (GameObject part in map)
        {
            if (part.transform.position.z == -100)
            {
                toRemove = part;
            }
            else
            {
                part.transform.position = part.transform.position - moveBack;
            }
        }
        if (toRemove != null)
        {
            map.Remove(toRemove);
            Destroy(toRemove);
            GameObject part = GameObject.Instantiate(giveRandomMapPart());
            part.transform.position = new Vector3(0, 0, map[8].transform.position.z + 100);
            part.transform.parent = mapObject.transform;
            map.Add(part);
        }
    }
}
