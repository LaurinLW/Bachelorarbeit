using UnityEngine;

public class WebcamController : MonoBehaviour
{
    private WebCamTexture webcamTexture;

    void Start()
    {
        webcamTexture = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    public WebCamTexture giveTexture()
    {
        return webcamTexture;
    }
}