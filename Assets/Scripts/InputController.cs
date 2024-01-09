using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public GameObject videoScreen;
    private GameObject leftWrist;
    private GameObject rightWrist;
    private GameObject leftShoulder;
    private GameObject rightShoulder;

    private MeshRenderer leftWristMeshRenderer;
    private MeshRenderer rightWristMeshRenderer;
    private MeshRenderer leftShoulderMeshRenderer;
    private MeshRenderer rightShoulderMeshRenderer;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [Tooltip("Direction of the Body/Arms")]
    public Direction inputDirectionLeftSide = Direction.Down;
    public Direction inputDirectionRightSide = Direction.Down;

    [Tooltip("Balance the amount of effort for the direction")]
    [Range(0.5f, 1f)]
    public float balancing = 0.5f;

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
                float shoulderDistance = Mathf.Abs(leftShoulder.transform.position.x - rightShoulder.transform.position.x) / 2;
                //TODO: hand is down, up , left or right -> slidebar to adjust some sort of difficulty?
                if (leftWrist.transform.position.y < leftShoulder.transform.position.y - shoulderDistance * balancing
                    && Vector2.Distance(leftWrist.transform.position, leftShoulder.transform.position) > shoulderDistance)
                {
                    inputDirectionLeftSide = Direction.Down;
                }
                else if (leftWrist.transform.position.y > leftShoulder.transform.position.y + shoulderDistance * balancing
                    && Vector2.Distance(leftWrist.transform.position, leftShoulder.transform.position) > shoulderDistance)
                {
                    inputDirectionLeftSide = Direction.Up;
                }
                else if (Vector2.Distance(leftWrist.transform.position, leftShoulder.transform.position) > shoulderDistance)
                {
                    inputDirectionLeftSide = Direction.Left;
                }
                if (rightWrist.transform.position.y < rightShoulder.transform.position.y - shoulderDistance * balancing
                    && Vector2.Distance(rightWrist.transform.position, rightShoulder.transform.position) > shoulderDistance)
                {
                    inputDirectionRightSide = Direction.Down;
                }
                else if (rightWrist.transform.position.y > rightShoulder.transform.position.y + shoulderDistance * balancing
                    && Vector2.Distance(rightWrist.transform.position, rightShoulder.transform.position) > shoulderDistance)
                {
                    inputDirectionRightSide = Direction.Up;
                }
                else if (Vector2.Distance(rightWrist.transform.position, rightShoulder.transform.position) > shoulderDistance)
                {

                    inputDirectionRightSide = Direction.Right;
                }
            }
        }
    }
}
