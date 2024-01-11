using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputController inputController;
    private float timer;
    public float movementBlockTime = 0.5f;
    public float force = 1f;

    private bool isMoving = false;
    private Vector3 right = new Vector3(2, 0, 0);
    private Vector3 left = new Vector3(-2, 0, 0);
    private Vector3 up = new Vector3(0, 1.5f, 0);

    void Jump()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(up * force, ForceMode.Impulse);
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Left()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(left * force, ForceMode.Impulse);
        StartCoroutine(MoveBack());
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Right()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(right * force, ForceMode.Impulse);
        StartCoroutine(MoveBack());
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(128f, 70.97f, -5f);
    }

    void Update()
    {
        if (!isMoving)
        {
            if (inputController.inputDirectionLeftSide == InputController.Direction.Up
            && inputController.inputDirectionRightSide == InputController.Direction.Up)
            {
                Jump();
            }
            else if (inputController.inputDirectionLeftSide == InputController.Direction.Left
            && inputController.inputDirectionRightSide == InputController.Direction.Down)
            {
                Left();
            }
            else if (inputController.inputDirectionLeftSide == InputController.Direction.Down
            && inputController.inputDirectionRightSide == InputController.Direction.Right)
            {
                Right();
            }
        }
    }

    private IEnumerator CooldownRoutine()
    {
        while (isMoving)
        {
            yield return new WaitForSeconds(movementBlockTime);

            isMoving = false;
        }
    }

    private IEnumerator MoveBack()
    {
        yield return new WaitForSeconds(movementBlockTime / 2);

        if (gameObject.transform.position.x < 128)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(right * force, ForceMode.Impulse);
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().AddForce(left * force, ForceMode.Impulse);
        }
    }
}
