using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameObject leftWrist;
    public GameObject rightWrist;
    public GameObject leftShoulder;
    public GameObject rightShoulder;

    private MeshRenderer leftWristMeshRenderer;
    private MeshRenderer rightWristMeshRenderer;
    private MeshRenderer leftShoulderMeshRenderer;
    private MeshRenderer rightShoulderMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {

    }

    //find all important key points from the model
    void findAllElements()
    {
        leftWrist = GameObject.Find("leftWrist");
        rightWrist = GameObject.Find("rightWrist");
        leftShoulder = GameObject.Find("leftShoulder");
        rightShoulder = GameObject.Find("rightShoulder");
    }

    // MeshRenderers get assigned from the GameObjects
    void findMeshRenderer()
    {
        leftWristMeshRenderer = leftWrist.GetComponent<MeshRenderer>();
        leftShoulderMeshRenderer = leftShoulder.GetComponent<MeshRenderer>();
        rightWristMeshRenderer = rightWrist.GetComponent<MeshRenderer>();
        rightShoulderMeshRenderer = rightShoulder.GetComponent<MeshRenderer>();
    }

    // check if all key points are assigned
    bool allAssigned()
    {
        return (leftWrist != null && rightWrist != null && leftShoulder != null && rightShoulder != null);
    }

    // check if all MeshRenderers are enabled
    bool allActivated()
    {
        return leftWristMeshRenderer.enabled && leftShoulderMeshRenderer.enabled && rightWristMeshRenderer.enabled && rightShoulderMeshRenderer.enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (!allAssigned())
        {
            findAllElements();
            if (allAssigned()) findMeshRenderer(); //so this only gets called once.
        }
        else
        {
            if (allActivated())
            {
                
            }
        }
    }
}
