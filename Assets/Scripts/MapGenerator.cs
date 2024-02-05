using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{
    public GameObject leftPart;
    public GameObject rightPart;
    public GameObject jumpPart;
    public GameObject mapObject;

    public GameObject tutorialPart;
    public GameObject tutorialPartTwo;


    public Vector3 moveBack = new Vector3(0, 0, 0.3f);

    private long seed;
    private List<GameObject> map;

    public int conLength = 10;
    public GameObject score;
    public float scoreValue = 0;

    public Vector3 maxSpeed = new Vector3(0, 0, 1.5f);

    public bool withTutorial;

    public List<GameObject> getMap()
    {
        return map;
    }

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
        withTutorial = true;
        DateTime currentDateTime = DateTime.Now;
        seed = currentDateTime.Ticks;
        UnityEngine.Random.InitState((int)seed);
        map = new List<GameObject>();
        if (withTutorial)
        {
            GameObject tutorial = GameObject.Instantiate(tutorialPart);
            tutorial.transform.position = new Vector3(0, 0, 80);
            tutorial.transform.parent = mapObject.transform;
            map.Add(tutorial);
            // GameObject tutorialTwo = GameObject.Instantiate(tutorialPartTwo);
            // tutorialTwo.transform.position = new Vector3(0, 0, 0);
            // tutorialTwo.transform.parent = mapObject.transform;
            // map.Add(tutorialTwo);
            for (int i = 0; i < conLength; i++)
            {
                GameObject part = GameObject.Instantiate(giveRandomMapPart());
                part.transform.position = new Vector3(0, 0, 100 * i + 230);
                part.transform.parent = mapObject.transform;
                map.Add(part);
            }
        }
        else
        {
            for (int i = 0; i < conLength; i++)
            {
                GameObject part = GameObject.Instantiate(giveRandomMapPart());
                part.transform.position = new Vector3(0, 0, 100 * i);
                part.transform.parent = mapObject.transform;
                map.Add(part);
            }
        }
    }

    void updateScore()
    {
        scoreValue += moveBack.z;
        score.GetComponent<TextMeshProUGUI>().text = scoreValue.ToString("0");
        if (((int)scoreValue) % 300 == 0 && moveBack.z > 0 && moveBack.z < maxSpeed.z) moveBack = moveBack + new Vector3(0, 0, 0.01f);
    }

    public void restart()
    {
        DateTime currentDateTime = DateTime.Now;
        seed = currentDateTime.Ticks;
        UnityEngine.Random.InitState((int)seed);
        foreach (GameObject part in map)
        {
            Destroy(part);
        }
        map.Clear();
        if (withTutorial)
        {
            GameObject tutorial = GameObject.Instantiate(tutorialPart);
            tutorial.transform.position = new Vector3(0, 0, 80);
            tutorial.transform.parent = mapObject.transform;
            map.Add(tutorial);
            // GameObject tutorialTwo = GameObject.Instantiate(tutorialPartTwo);
            // tutorialTwo.transform.position = new Vector3(0, 0, 0);
            // tutorialTwo.transform.parent = mapObject.transform;
            // map.Add(tutorialTwo);
            for (int i = 0; i < conLength; i++)
            {
                GameObject part = GameObject.Instantiate(giveRandomMapPart());
                part.transform.position = new Vector3(0, 0, 100 * i + 230);
                part.transform.parent = mapObject.transform;
                map.Add(part);
            }
        }
        else
        {
            for (int i = 0; i < conLength; i++)
            {
                GameObject part = GameObject.Instantiate(giveRandomMapPart());
                part.transform.position = new Vector3(0, 0, 100 * i);
                part.transform.parent = mapObject.transform;
                map.Add(part);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        GameObject toRemove = null;
        updateScore();
        foreach (GameObject part in map)
        {
            if (part.transform.position.z < -150)
            {
                toRemove = part;
            }
            else
            {
                part.transform.position = part.transform.position - (moveBack * Time.deltaTime * 20);
            }
        }
        if (toRemove != null)
        {
            map.Remove(toRemove);
            Destroy(toRemove);
            GameObject part = GameObject.Instantiate(giveRandomMapPart());
            part.transform.position = new Vector3(0, 0, map[9].transform.position.z + 100);
            part.transform.parent = mapObject.transform;
            map.Add(part);
        }
    }
}
