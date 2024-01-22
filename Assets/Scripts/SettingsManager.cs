using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public GameObject settingsPlane;
    public GameObject settingsField;
    public GameStateManager GameState;
    public InputController balancing;
    private List<GameObject> settingsObjects;

    GameObject getText(GameObject sett)
    {
        for (int i = 0; i < sett.transform.childCount; i++)
        {
            if (sett.transform.GetChild(i).gameObject.transform.name == "Text")
            {
                return sett.transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    GameObject getSlider(GameObject sett)
    {
        for (int i = 0; i < sett.transform.childCount; i++)
        {
            if (sett.transform.GetChild(i).gameObject.transform.name == "Slider")
            {
                for (int j = 0; j < sett.transform.GetChild(i).transform.childCount; j++)
                {
                    if (sett.transform.GetChild(i).transform.GetChild(j).gameObject.transform.name == "InputSlider")
                    {
                        return sett.transform.GetChild(i).transform.GetChild(j).gameObject;
                    }
                }
            }
        }
        return null;
    }
    private bool correctPosition;

    void changeVolume(Slider slider)
    {
        AudioListener.volume = slider.value;
    }

    void changeGameSpeed(Slider slider)
    {
        GameState.GameSpeed = slider.value;
    }

    void changeBalancing(Slider slider)
    {
        balancing.balancing = slider.value;
    }

    void Start()
    {
        correctPosition = false;
        settingsObjects = new List<GameObject>();


        GameObject volume = GameObject.Instantiate(settingsField);
        volume.transform.SetParent(settingsPlane.transform);
        volume.SetActive(true);
        getText(volume).GetComponent<TextMeshPro>().text = "Volume";
        settingsObjects.Add(volume);
        GameObject slider = getSlider(volume);
        AudioListener.volume = 0.1f;
        slider.GetComponent<Slider>().value = AudioListener.volume;
        slider.GetComponent<Slider>().maxValue = 1f;
        slider.GetComponent<Slider>().minValue = 0f;
        slider.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeVolume(slider.GetComponent<Slider>()); });

        GameObject gameSpeed = GameObject.Instantiate(settingsField);
        gameSpeed.transform.SetParent(settingsPlane.transform);
        gameSpeed.SetActive(true);
        getText(gameSpeed).GetComponent<TextMeshPro>().text = "Starting Game Speed";
        settingsObjects.Add(gameSpeed);
        GameObject sliderGameSpeed = getSlider(gameSpeed);
        sliderGameSpeed.GetComponent<Slider>().value = GameState.GameSpeed;
        sliderGameSpeed.GetComponent<Slider>().maxValue = 2f;
        sliderGameSpeed.GetComponent<Slider>().minValue = 0.1f;
        sliderGameSpeed.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeGameSpeed(sliderGameSpeed.GetComponent<Slider>()); });

        GameObject gameBalancing = GameObject.Instantiate(settingsField);
        gameBalancing.transform.SetParent(settingsPlane.transform);
        gameBalancing.SetActive(true);
        getText(gameBalancing).GetComponent<TextMeshPro>().text = "Movement Balancing";
        settingsObjects.Add(gameBalancing);
        GameObject sliderBalancing = getSlider(gameBalancing);
        sliderBalancing.GetComponent<Slider>().value = balancing.balancing;
        sliderBalancing.GetComponent<Slider>().maxValue = 1f;
        sliderBalancing.GetComponent<Slider>().minValue = 0.5f;
        sliderBalancing.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBalancing(sliderBalancing.GetComponent<Slider>()); });


        settingsField.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!correctPosition)
        {
            int i = 0;
            foreach (GameObject setting in settingsObjects)
            {
                setting.transform.position = settingsField.transform.position + new Vector3(0, -0.2f * i, 0);
                setting.transform.rotation = settingsField.transform.rotation;
                getSlider(setting).transform.position = getSlider(settingsField).transform.position + new Vector3(500, 0, -950 + i * 200);
                i++;
            }
            correctPosition = true;
        }
    }
}
