using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputController inputController;
    private float timer;
    public float movementBlockTime = 0.5f;
    void Jump()
    {
        float currentTime = Time.deltaTime;
        while (currentTime + movementBlockTime > Time.deltaTime)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(0, 0.1f, 0);
        }
    }

    void Left()
    {

    }

    void Right()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inputController.inputDirectionLeftSide == InputController.Direction.Up && inputController.inputDirectionRightSide == InputController.Direction.Up)
        {
            Jump();
        }
    }
}
