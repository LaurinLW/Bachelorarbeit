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

    [Range(0.0f, 100f)]
    public float leftPercent = 0;
    [Range(0.0f, 100f)]
    public float rightPercent = 0;


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

    void updatePercentage(float shoulderDistance)
    {
        if (leftWrist.transform.position.y < leftShoulder.transform.position.y)
        {
            leftPercent = ((leftWrist.transform.position.y) / leftShoulder.transform.position.y) * 100;
        }
        else
        {
            leftPercent = 100 - ((leftWrist.transform.position.y - leftShoulder.transform.position.y) / leftShoulder.transform.position.y) * 100;
        }
        if (rightWrist.transform.position.y < rightShoulder.transform.position.y)
        {
            rightPercent = ((rightWrist.transform.position.y) / rightShoulder.transform.position.y) * 100;
        }
        else
        {
            rightPercent = 100 - ((rightWrist.transform.position.y - rightShoulder.transform.position.y) / rightShoulder.transform.position.y) * 100;
        }

        if (leftPercent < 0) leftPercent *= -1;
        if (rightPercent < 0) rightPercent *= -1;
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
                float shoulderDistance = Mathf.Abs(leftShoulder.transform.position.x - rightShoulder.transform.position.x);

                updatePercentage(shoulderDistance);

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
