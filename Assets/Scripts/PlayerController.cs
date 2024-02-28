using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public InputController inputController;
    public MapGenerator mapGenerator;
    private float timer;
    public float movementBlockTime = 1.5f;
    public float freezeTime = 4f;
    public float force = 2f;

    private bool isMoving = false;
    private Vector3 leftRightVector = new Vector3(2f, 0, 0);
    private Vector3 up = new Vector3(0, 1.2f, 0);

    private Animator anim;
    private bool leftRight;

    public GameObject gameOver;

    public AudioSource hurt;
    public AudioSource success;

    public ParticleSystem smoke;

    public GameObject angryLeft;
    public GameObject angryRight;
    public GameObject stars;
    public bool juicy;
    private bool stop;
    public HealthManagement healthManagement;
    private bool healthCooldown;
    void Jump()
    {
        anim.SetInteger("AnimationPar", 2);
        gameObject.GetComponent<Rigidbody>().AddForce(up * force, ForceMode.Impulse);
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Left()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(-leftRightVector * force, ForceMode.Impulse);
        isMoving = true;
        stop = false;
        StartCoroutine(CooldownRoutine());
        StartCoroutine(StopLeft());
    }

    void Right()
    {
        gameObject.GetComponent<Rigidbody>().AddForce(leftRightVector * force, ForceMode.Impulse);
        isMoving = true;
        stop = false;
        StartCoroutine(CooldownRoutine());
        StartCoroutine(StopRight());
    }

    void Start()
    {
        gameObject.transform.position = new Vector3(128f, 71f, -5f);
        anim = gameObject.GetComponentInChildren<Animator>();
        anim.SetInteger("AnimationPar", 1);
        juicy = true;
        AudioListener.volume = 0.1f;
        smoke.Stop();
        angryLeft.SetActive(false);
        angryRight.SetActive(false);
        healthCooldown = false;
    }

    bool isInMovingZone()
    {
        List<GameObject> map = mapGenerator.getMap();
        int mapPart = 0;
        while (map[mapPart].transform.Find("Obstacle").transform.position.z < gameObject.transform.position.z &&
         map[mapPart].transform.Find("ObstacleTwo").transform.position.z < gameObject.transform.position.z)
        {
            mapPart = mapPart + 1;
        }
        float movingZoneStart = map[mapPart].transform.Find("LeftPole").transform.position.z;
        float movingZoneEnd = map[mapPart].transform.Find("Obstacle").transform.position.z;
        float movingZoneTwoStart = map[mapPart].transform.Find("LeftPoleTwo").transform.position.z;
        float movingZoneTwoEnd = map[mapPart].transform.Find("ObstacleTwo").transform.position.z;
        return (gameObject.transform.position.z > movingZoneStart && gameObject.transform.position.z < movingZoneEnd) ||
        (gameObject.transform.position.z > movingZoneTwoStart && gameObject.transform.position.z < movingZoneTwoEnd);
    }

    private Vector3 restoreSpeed;
    private bool freezed;

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.name == "Obstacle" || collision.gameObject.name == "ObstacleTwo") && collision.gameObject.transform.position.z > gameObject.transform.position.z)
        {
            if (juicy)
            {
                smoke.Play();
                StartCoroutine(Angry());
            }
            if (collision.gameObject.tag == "left")
            {
                if (inputController.leftPercent < 90f)
                {
                    loseHealth();
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
                    loseHealth();
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
                    loseHealth();
                }
                else
                {
                    freezed = true;
                    StartCoroutine(Freeze());
                }
            }
        }
    }

    void loseHealth()
    {
        if (!healthCooldown)
        {
            StartCoroutine(StopDamage());
            healthManagement.damage();
            if (healthManagement.health == 0)
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

    string nextMove()
    {
        List<GameObject> map = mapGenerator.getMap();
        int mapPart = 0;
        while (map[mapPart].transform.Find("Obstacle").transform.position.z < gameObject.transform.position.z &&
         map[mapPart].transform.Find("ObstacleTwo").transform.position.z < gameObject.transform.position.z)
        {
            mapPart = mapPart + 1;
        }
        float movingZoneStart = map[mapPart].transform.Find("LeftPole").transform.position.z;
        float movingZoneEnd = map[mapPart].transform.Find("Obstacle").transform.position.z;
        float movingZoneTwoStart = map[mapPart].transform.Find("LeftPoleTwo").transform.position.z;
        float movingZoneTwoEnd = map[mapPart].transform.Find("ObstacleTwo").transform.position.z;
        if (gameObject.transform.position.z > movingZoneStart && gameObject.transform.position.z < movingZoneEnd)
        {
            return map[mapPart].transform.Find("Obstacle").tag;
        }
        if (gameObject.transform.position.z > movingZoneTwoStart && gameObject.transform.position.z < movingZoneTwoEnd)
        {
            return map[mapPart].transform.Find("ObstacleTwo").tag;
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
                GameObject starsClone = GameObject.Instantiate(stars);
                starsClone.transform.parent = gameObject.transform;
                starsClone.transform.position = new Vector3(128, 71, -5);
                StartCoroutine(RemoveObject(starsClone, 2.0f));
                success.Play();
            }
        }
        else if (!isMoving && isInMovingZone() && nextMove() == "left")
        {
            if (inputController.inputDirectionLeftSide == InputController.Direction.Left)
            {
                Left();
                leftRight = true;
                GameObject starsClone = GameObject.Instantiate(stars);
                starsClone.transform.parent = gameObject.transform;
                starsClone.transform.position = new Vector3(128, 71, -5);
                StartCoroutine(RemoveObject(starsClone, 2.0f));
                success.Play();
            }
        }
        else if (!isMoving && isInMovingZone() && nextMove() == "right")
        {
            if (inputController.inputDirectionRightSide == InputController.Direction.Right)
            {
                Right();
                leftRight = true;
                GameObject starsClone = GameObject.Instantiate(stars);
                starsClone.transform.parent = gameObject.transform;
                starsClone.transform.position = new Vector3(128, 71, -5);
                StartCoroutine(RemoveObject(starsClone, 2.0f));
                success.Play();
            }
        }

        if (!isMoving && leftRight)
        {
            MoveBack();
            leftRight = false;
        }

        if (gameObject.transform.position.y < 70)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 71, gameObject.transform.position.z);
        }
    }

    private IEnumerator RemoveObject(GameObject t, float time)
    {
        yield return new WaitForSeconds(time);
        UnityEngine.Object.Destroy(t);
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

    private IEnumerator Angry()
    {
        yield return new WaitForSeconds(freezeTime * 0.2f);
        angryLeft.SetActive(true);
        angryRight.SetActive(true);
        yield return new WaitForSeconds(freezeTime * 0.8f);
        angryLeft.SetActive(false);
        angryRight.SetActive(false);
    }

    private IEnumerator StopLeft()
    {
        while (!stop)
        {
            yield return new WaitForSeconds(0.325f);
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            stop = true;
        }
    }

    private IEnumerator StopRight()
    {
        while (!stop)
        {
            yield return new WaitForSeconds(0.325f);
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            stop = true;
        }
    }

    private IEnumerator StopDamage()
    {
        healthCooldown = true;
        yield return new WaitForSeconds(0.25f);
        healthCooldown = false;
    }

    private IEnumerator StopMiddle()
    {
        while (!stop)
        {
            yield return new WaitForSeconds(0.325f);
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            if (gameObject.transform.position.x < 128)
            {
                gameObject.transform.position = gameObject.transform.position + (new Vector3(128 - gameObject.transform.position.x, 0, 0));
            }
            else if (gameObject.transform.position.x > 128)
            {
                gameObject.transform.position = gameObject.transform.position - (new Vector3(Math.Abs(128 - gameObject.transform.position.x), 0, 0));
            }
            stop = true;
        }
    }

    private IEnumerator Freeze()
    {
        while (freezed)
        {
            if (juicy)
            {
                hurt.Play(0);
                anim.SetInteger("AnimationPar", 3);
            }
            restoreSpeed = mapGenerator.moveBack;
            float diff = mapGenerator.moveBack.z > 1 ? 2 : 1;
            yield return new WaitForSeconds(freezeTime * (0.2f / diff));
            mapGenerator.moveBack = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(freezeTime * (0.8f / diff));
            anim.SetInteger("AnimationPar", 1);
            Vector3 addUp = new Vector3(0, 0, 0.002f);
            for (; mapGenerator.moveBack.z < restoreSpeed.z; mapGenerator.moveBack += addUp)
            {
                yield return new WaitForSeconds(0.01f);
            }
            freezed = false;
        }
    }

    private void MoveBack()
    {
        if (gameObject.transform.position.x < 128)
        {
            gameObject.GetComponent<Rigidbody>().AddForce(leftRightVector * force, ForceMode.Impulse);
            stop = false;
            StartCoroutine(StopMiddle());
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().AddForce(-leftRightVector * force, ForceMode.Impulse);
            stop = false;
            StartCoroutine(StopMiddle());
        }
    }
}
