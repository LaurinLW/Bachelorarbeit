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
    public MapGenerator mapGenerator;

    public InputController balancing;
    private List<GameObject> settingsObjects;

    public PlayerController playerController;

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

    GameObject getSliderText(GameObject sett)
    {
        for (int i = 0; i < sett.transform.childCount; i++)
        {
            if (sett.transform.GetChild(i).gameObject.transform.name == "Slider")
            {
                for (int j = 0; j < sett.transform.GetChild(i).transform.childCount; j++)
                {
                    if (sett.transform.GetChild(i).transform.GetChild(j).gameObject.transform.name == "SliderText")
                    {
                        return sett.transform.GetChild(i).transform.GetChild(j).gameObject;
                    }
                }
            }
        }
        return null;
    }
    private bool correctPosition;

    void changeVolume(Slider slider, GameObject textObject)
    {
        AudioListener.volume = slider.value;
        textObject.GetComponent<TextMeshProUGUI>().text = slider.GetComponent<Slider>().value.ToString("0.0");
    }

    void changeGameSpeed(Slider slider, GameObject textObject)
    {
        GameState.GameSpeed = slider.value;
        textObject.GetComponent<TextMeshProUGUI>().text = slider.GetComponent<Slider>().value.ToString("0.00");
    }

    void changeBalancing(Slider slider, GameObject textObject)
    {
        balancing.balancing = slider.value;
        textObject.GetComponent<TextMeshProUGUI>().text = slider.GetComponent<Slider>().value.ToString("0.00");
    }

    void changeJuicy(Slider slider, GameObject textObject)
    {
        playerController.juicy = slider.value == 1 ? true : false;
        textObject.GetComponent<TextMeshProUGUI>().text = slider.GetComponent<Slider>().value.ToString();
    }

    void changeTutorial(Slider slider, GameObject textObject)
    {
        mapGenerator.withTutorial = slider.value == 1 ? true : false;
        mapGenerator.restart();
        textObject.GetComponent<TextMeshProUGUI>().text = slider.GetComponent<Slider>().value.ToString();
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
        slider.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeVolume(slider.GetComponent<Slider>(), getSliderText(volume)); });
        getSliderText(volume).GetComponent<TextMeshProUGUI>().text = slider.GetComponent<Slider>().value.ToString();

        GameObject gameSpeed = GameObject.Instantiate(settingsField);
        gameSpeed.transform.SetParent(settingsPlane.transform);
        gameSpeed.SetActive(true);
        getText(gameSpeed).GetComponent<TextMeshPro>().text = "Starting Game Speed";
        settingsObjects.Add(gameSpeed);
        GameObject sliderGameSpeed = getSlider(gameSpeed);
        sliderGameSpeed.GetComponent<Slider>().value = GameState.GameSpeed;
        sliderGameSpeed.GetComponent<Slider>().maxValue = 2f;
        sliderGameSpeed.GetComponent<Slider>().minValue = 0.1f;
        sliderGameSpeed.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeGameSpeed(sliderGameSpeed.GetComponent<Slider>(), getSliderText(gameSpeed)); });
        getSliderText(gameSpeed).GetComponent<TextMeshProUGUI>().text = sliderGameSpeed.GetComponent<Slider>().value.ToString();

        GameObject gameBalancing = GameObject.Instantiate(settingsField);
        gameBalancing.transform.SetParent(settingsPlane.transform);
        gameBalancing.SetActive(true);
        getText(gameBalancing).GetComponent<TextMeshPro>().text = "Movement Balancing";
        settingsObjects.Add(gameBalancing);
        GameObject sliderBalancing = getSlider(gameBalancing);
        sliderBalancing.GetComponent<Slider>().value = balancing.balancing;
        sliderBalancing.GetComponent<Slider>().maxValue = 1f;
        sliderBalancing.GetComponent<Slider>().minValue = 0.5f;
        sliderBalancing.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeBalancing(sliderBalancing.GetComponent<Slider>(), getSliderText(gameBalancing)); });
        getSliderText(gameBalancing).GetComponent<TextMeshProUGUI>().text = sliderBalancing.GetComponent<Slider>().value.ToString();

        GameObject gameJuicy = GameObject.Instantiate(settingsField);
        gameJuicy.transform.SetParent(settingsPlane.transform);
        gameJuicy.SetActive(true);
        getText(gameJuicy).GetComponent<TextMeshPro>().text = "Juicy";
        settingsObjects.Add(gameJuicy);
        GameObject sliderJuicy = getSlider(gameJuicy);
        sliderJuicy.GetComponent<Slider>().value = playerController.juicy ? 1 : 0;
        sliderJuicy.GetComponent<Slider>().wholeNumbers = true;
        sliderJuicy.GetComponent<Slider>().maxValue = 1;
        sliderJuicy.GetComponent<Slider>().minValue = 0;
        sliderJuicy.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeJuicy(sliderJuicy.GetComponent<Slider>(), getSliderText(gameJuicy)); });
        getSliderText(gameJuicy).GetComponent<TextMeshProUGUI>().text = sliderJuicy.GetComponent<Slider>().value.ToString();

        GameObject gameTutorial = GameObject.Instantiate(settingsField);
        gameTutorial.transform.SetParent(settingsPlane.transform);
        gameTutorial.SetActive(true);
        getText(gameTutorial).GetComponent<TextMeshPro>().text = "Tutorial";
        settingsObjects.Add(gameTutorial);
        GameObject sliderTutorial = getSlider(gameTutorial);
        sliderTutorial.GetComponent<Slider>().value = mapGenerator.withTutorial ? 1 : 0;
        sliderTutorial.GetComponent<Slider>().wholeNumbers = true;
        sliderTutorial.GetComponent<Slider>().maxValue = 1;
        sliderTutorial.GetComponent<Slider>().minValue = 0;
        sliderTutorial.GetComponent<Slider>().onValueChanged.AddListener(delegate { changeTutorial(sliderTutorial.GetComponent<Slider>(), getSliderText(gameTutorial)); });
        getSliderText(gameTutorial).GetComponent<TextMeshProUGUI>().text = sliderTutorial.GetComponent<Slider>().value.ToString();


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
                getSliderText(setting).transform.position = getSliderText(settingsField).transform.position + new Vector3(0, 0, -945 + i * 200);

                i++;
            }
            correctPosition = true;
        }
    }
}
