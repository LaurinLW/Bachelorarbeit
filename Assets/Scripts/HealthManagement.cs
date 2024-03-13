using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    public GameObject heartOne;
    public GameObject heartTwo;
    public GameObject heartThree;

    public int health;

    private bool animateOne;
    private bool animateTwo;
    private bool animateThree;

    private Vector3[] origPos = new Vector3[3];

    void Start()
    {
        health = 3;
        origPos[0] = heartOne.transform.position;
        origPos[1] = heartTwo.transform.position;
        origPos[2] = heartThree.transform.position;
    }

    public void Restart()
    {
        animateOne = false;
        animateTwo = false;
        animateThree = false;
        heartOne.SetActive(true);
        heartTwo.SetActive(true);
        heartThree.SetActive(true);
        health = 3;
        heartOne.transform.position = origPos[0];
        heartTwo.transform.position = origPos[1];
        heartThree.transform.position = origPos[2];
    }

    void Update()
    {
        if (health > 0)
        {
            heartOne.SetActive(true);
            animateOne = false;
        }
        else
        {
            if (heartOne.activeSelf && !animateOne)
            {
                animateOne = true;
                StartCoroutine(Fall(heartOne));
            }
        }
        if (health > 1)
        {
            heartTwo.SetActive(true);
            animateTwo = false;
        }
        else
        {
            if (heartTwo.activeSelf && !animateTwo)
            {
                animateTwo = true;
                StartCoroutine(Fall(heartTwo));
            }
        }
        if (health > 2)
        {
            heartThree.SetActive(true);
            animateThree = false;
        }
        else
        {
            if (heartThree.activeSelf && !animateThree)
            {
                animateThree = true;
                StartCoroutine(Fall(heartThree));
            }
        }
    }

    public void damage()
    {
        if (health > 0) health--;
    }

    bool animateObject(GameObject o)
    {
        if (o == heartThree) return animateThree;
        if (o == heartTwo) return animateTwo;
        if (o == heartOne) return animateOne;
        return false;
    }

    private IEnumerator Fall(GameObject o)
    {
        Vector3 postion = o.transform.position;
        Quaternion rotation = o.transform.rotation;
        while (o.transform.position.y > -100 && animateObject(o))
        {
            o.transform.position = o.transform.position - new Vector3(-0.2f, 0.4f, -0.4f);
            yield return new WaitForSeconds(0.05f);
        }
        o.SetActive(false);
        o.transform.position = postion;
        o.transform.rotation = rotation;
    }
}
