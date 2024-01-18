using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MouseHover : MonoBehaviour
{
    void Start()
    {
        this.GetComponent<TextMeshPro>().color = Color.black;
    }

    void OnMouseEnter()
    {
        this.GetComponent<TextMeshPro>().color = Color.white;
    }

    void OnMouseExit()
    {
        this.GetComponent<TextMeshPro>().color = Color.black;
    }

    public GameObject parent;
    void OnMouseUp()
    {
        if (this.GetComponent<TextMeshPro>().text == "Play")
        {
            parent.SetActive(false);
        }
        else if (this.GetComponent<TextMeshPro>().text == "Settings")
        {
            Debug.Log("sett");

        }
        else if (this.GetComponent<TextMeshPro>().text == "Quit")
        {
            Debug.Log("ex");

        }
    }
}
