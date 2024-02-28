using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManagement : MonoBehaviour
{
    public GameObject heartOne;
    public GameObject heartTwo;
    public GameObject heartThree;

    public int health;

    void Start()
    {
        health = 3;
    }
    void Update()
    {
        if (health > 0)
        {
            heartOne.SetActive(true);
        }
        else
        {
            heartOne.SetActive(false);
        }
        if (health > 1)
        {
            heartTwo.SetActive(true);
        }
        else
        {
            heartTwo.SetActive(false);
        }
        if (health > 2)
        {
            heartThree.SetActive(true);
        }
        else
        {
            heartThree.SetActive(false);
        }
    }

    public void damage()
    {
        if (health > 0) health--;
    }
}
