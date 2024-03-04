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

    void Start()
    {
        health = 3;
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

    private IEnumerator Fall(GameObject o)
    {
        Vector3 postion = o.transform.position;
        Quaternion rotation = o.transform.rotation;
        while (o.transform.position.y > -100)
        {
            o.transform.position = o.transform.position - new Vector3(-0.2f, 0.4f, -0.4f);
            yield return new WaitForSeconds(0.05f);
        }
        o.SetActive(false);
        o.transform.position = postion;
        o.transform.rotation = rotation;
    }
}
