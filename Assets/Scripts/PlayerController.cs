using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InputController inputController;
    public MapGenerator mapGenerator;
    private float timer;
    public float movementBlockTime = 1.5f;
    public float freezeTime = 4f;
    public float force = 2f;

    private bool isMoving = false;
    private Vector3 right = new Vector3(1, 0, 0);
    private Vector3 left = new Vector3(-1, 0, 0);
    private Vector3 up = new Vector3(0, 1f, 0);

    private Animator anim;
    private bool leftRight;

    public GameObject gameOver;

    public bool juicy;

    void Jump()
    {
        anim.SetInteger("AnimationPar", 2);
        gameObject.GetComponent<Rigidbody>().AddForce(up * force, ForceMode.Impulse);
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Left()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(left * force, ForceMode.Impulse);
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Right()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(right * force, ForceMode.Impulse);
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(128f, 71f, -5f);
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.SetInteger("AnimationPar", 1);
        juicy = true;
        AudioListener.volume = 0.1f;
    }

    bool isInMovingZone()
    {
        List<GameObject> map = mapGenerator.getMap();
        float movingZoneStart = map[0].transform.Find("LeftPole").transform.position.z;
        float movingZoneEnd = map[0].transform.Find("Obstacle").transform.position.z;
        float movingZoneTwoStart = map[0].transform.Find("LeftPoleTwo").transform.position.z;
        float movingZoneTwoEnd = map[0].transform.Find("ObstacleTwo").transform.position.z;
        return (gameObject.transform.position.z > movingZoneStart && gameObject.transform.position.z < movingZoneEnd) ||
        (gameObject.transform.position.z > movingZoneTwoStart && gameObject.transform.position.z < movingZoneTwoEnd);
    }

    private Vector3 restoreSpeed;
    private bool freezed;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Obstacle" || collision.gameObject.name == "ObstacleTwo")
        {
            if (collision.gameObject.tag == "left")
            {
                if (inputController.leftPercent < 90f)
                {
                    gameOver.SetActive(true);
                    mapGenerator.moveBack = new Vector3(0, 0, 0);
                }
                else
                {
                    freezed = true;
                    StartCoroutine(Freeze());
                }
            }
            if (collision.gameObject.tag == "right")
            {
                if (inputController.rightPercent < 90f)
                {
                    gameOver.SetActive(true);
                    mapGenerator.moveBack = new Vector3(0, 0, 0);
                }
                else
                {
                    freezed = true;
                    StartCoroutine(Freeze());
                }
            }
            if (collision.gameObject.tag == "jump")
            {
                if (!(inputController.inputDirectionLeftSide == InputController.Direction.Up
                || inputController.inputDirectionRightSide == InputController.Direction.Up))
                {
                    gameOver.SetActive(true);
                    mapGenerator.moveBack = new Vector3(0, 0, 0);
                }
                else
                {
                    freezed = true;
                    StartCoroutine(Freeze());
                }
            }
        }
    }
    string nextMove()
    {
        List<GameObject> map = mapGenerator.getMap();
        float movingZoneStart = map[0].transform.Find("LeftPole").transform.position.z;
        float movingZoneEnd = map[0].transform.Find("Obstacle").transform.position.z;
        float movingZoneTwoStart = map[0].transform.Find("LeftPoleTwo").transform.position.z;
        float movingZoneTwoEnd = map[0].transform.Find("ObstacleTwo").transform.position.z;
        if (gameObject.transform.position.z > movingZoneStart && gameObject.transform.position.z < movingZoneEnd)
        {
            return map[0].transform.Find("Obstacle").tag;
        }
        if (gameObject.transform.position.z > movingZoneTwoStart && gameObject.transform.position.z < movingZoneTwoEnd)
        {
            return map[0].transform.Find("ObstacleTwo").tag;
        }
        return "";
    }

    void Update()
    {
        if (!isMoving && isInMovingZone() && nextMove() == "jump")
        {
            if (inputController.inputDirectionLeftSide == InputController.Direction.Up
            && inputController.inputDirectionRightSide == InputController.Direction.Up)
            {
                Jump();
            }
        }
        else if (!isMoving && isInMovingZone() && nextMove() == "left")
        {
            if (inputController.inputDirectionLeftSide == InputController.Direction.Left)
            {
                Left();
                leftRight = true;
            }
        }
        else if (!isMoving && isInMovingZone() && nextMove() == "right")
        {
            if (inputController.inputDirectionRightSide == InputController.Direction.Right)
            {
                Right();
                leftRight = true;
            }
        }

        if (!isMoving && leftRight)
        {
            MoveBack();
            leftRight = false;
        }
    }

    private IEnumerator CooldownRoutine()
    {
        while (isMoving)
        {
            yield return new WaitForSeconds(movementBlockTime);
            anim.SetInteger("AnimationPar", 1);
            isMoving = false;
        }
    }

    private IEnumerator Freeze()
    {
        while (freezed)
        {
            restoreSpeed = mapGenerator.moveBack;
            yield return new WaitForSeconds(freezeTime * 0.1f);
            mapGenerator.moveBack = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(freezeTime * 0.9f);
            mapGenerator.moveBack = restoreSpeed;
            freezed = false;
        }
    }

    private void MoveBack()
    {
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
