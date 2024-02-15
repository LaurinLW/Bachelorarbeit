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

    public GameObject menu;
    public GameObject settings;
    public GameObject score;
    public GameObject gameOver;

    public GameStateManager gameManager;
    void OnMouseUp()
    {
        if (this.GetComponent<TextMeshPro>().text == "Play")
        {
            menu.SetActive(false);
            gameManager.restart();
            score.SetActive(true);
        }
        else if (this.GetComponent<TextMeshPro>().text == "Settings")
        {
            menu.SetActive(false);
            settings.SetActive(true);
        }
        else if (this.GetComponent<TextMeshPro>().text == "Quit")
        {
            Application.Quit();
        }
        else if (this.GetComponent<TextMeshPro>().text == "Quit Settings")
        {
            menu.SetActive(true);
            settings.SetActive(false);
        }
        else if (this.GetComponent<TextMeshPro>().text == "Restart")
        {
            menu.SetActive(false);
            gameManager.restart();
            score.SetActive(true);
        }
        else if (this.GetComponent<TextMeshPro>().text == "Menu")
        {
            menu.SetActive(true);
            score.SetActive(false);
            settings.SetActive(false);
            gameOver.SetActive(false);
        }
    }
}
