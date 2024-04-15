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
    private Vector3 left = new Vector3(124f, 70.5f, -5f);
    private Vector3 right = new Vector3(132f, 70.5f, -5f);
    private Vector3 middle = new Vector3(128f, 70.5f, -5f);


    private Vector3 up = new Vector3(0, 1.2f, 0);

    private Animator anim;
    public GameObject gameOver;

    public AudioSource hurt;
    public AudioSource success;

    public ParticleSystem smoke;

    public GameObject angryLeft;
    public GameObject angryRight;
    public GameObject juicyText;

    public GameObject stars;
    public bool juicy;
    public HealthManagement healthManagement;
    private bool healthCooldown;
    public GameObject Flag;
    public Material Red;
    public Material Green;

    void Jump()
    {
        anim.SetInteger("AnimationPar", 2);
        gameObject.GetComponent<Rigidbody>().AddForce(up * force, ForceMode.Impulse);
        isMoving = true;
        StartCoroutine(CooldownRoutine());
    }

    void Left()
    {
        isMoving = true;
        StartCoroutine(MoveLeft());
        StartCoroutine(MoveMiddle());
    }
    private IEnumerator MoveLeft()
    {
        Vector3 origPos = gameObject.transform.position;
        float totalMovementTime = 1f;
        float currentMovementTime = 0f;
        while (Vector3.Distance(gameObject.transform.position, left) > 0.1f && anim.applyRootMotion)
        {
            currentMovementTime += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(origPos, left, currentMovementTime / totalMovementTime);
            yield return null;
        }
    }

    void Right()
    {
        isMoving = true;
        StartCoroutine(MoveRight());
        StartCoroutine(MoveMiddle());
    }

    private IEnumerator MoveRight()
    {
        Vector3 origPos = gameObject.transform.position;
        float totalMovementTime = 1f;
        float currentMovementTime = 0f;
        while (Vector3.Distance(gameObject.transform.position, right) > 0.1f && anim.applyRootMotion)
        {
            currentMovementTime += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(origPos, right, currentMovementTime / totalMovementTime);
            yield return null;
        }

    }

    private IEnumerator MoveMiddle()
    {
        yield return new WaitForSeconds(1.5f);
        isMoving = false;
        Vector3 origPos = gameObject.transform.position;
        float totalMovementTime = 0.5f;
        float currentMovementTime = 0f;
        while (Vector3.Distance(gameObject.transform.position, middle) > 0.1f && anim.applyRootMotion)
        {
            currentMovementTime += Time.deltaTime;
            gameObject.transform.position = Vector3.Lerp(origPos, middle, currentMovementTime / totalMovementTime);
            yield return null;
        }
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

    void juicyRoutine()
    {
        if (juicy)
        {
            smoke.Play();
            juicyText.SetActive(true);
            StartCoroutine(Angry());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.name == "Obstacle" || collision.gameObject.name == "ObstacleTwo") && collision.gameObject.transform.position.z > gameObject.transform.position.z && !freezed)
        {
            anim.applyRootMotion = false;
            hurt.Play(0);
            if (collision.gameObject.tag == "left")
            {
                if (inputController.leftPercent < 90f)
                {
                    loseHealth(collision.gameObject.tag);
                }
                else
                {
                    juicyRoutine();
                    freezed = true;
                    StartCoroutine(Freeze(collision.gameObject.tag));
                }
            }
            if (collision.gameObject.tag == "right")
            {
                if (inputController.rightPercent < 90f)
                {
                    loseHealth(collision.gameObject.tag);
                }
                else
                {
                    juicyRoutine();
                    freezed = true;
                    StartCoroutine(Freeze(collision.gameObject.tag));
                }
            }
            if (collision.gameObject.tag == "jump")
            {
                if (!(inputController.inputDirectionLeftSide == InputController.Direction.Up
                || inputController.inputDirectionRightSide == InputController.Direction.Up))
                {
                    loseHealth(collision.gameObject.tag);
                }
                else
                {
                    juicyRoutine();
                    freezed = true;
                    StartCoroutine(Freeze(collision.gameObject.tag));
                }
            }
        }
    }

    void loseHealth(string obst)
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
                StartCoroutine(Freeze(obst));
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
        if (gameObject.transform.position.z < movingZoneEnd)
        {
            return map[mapPart].transform.Find("Obstacle").tag;
        }
        if (gameObject.transform.position.z < movingZoneTwoEnd)
        {
            return map[mapPart].transform.Find("ObstacleTwo").tag;
        }
        return "";
    }

    void Update()
    {
        if (healthManagement.health == 0)
        {
            mapGenerator.moveBack = new Vector3(0, 0, 0);
        }
        if (nextMove() == "jump")
        {
            if (inputController.inputDirectionLeftSide == InputController.Direction.Up
                        && inputController.inputDirectionRightSide == InputController.Direction.Up)
            {
                Flag.GetComponent<MeshRenderer>().material = Green;
            }
            else
            {
                Flag.GetComponent<MeshRenderer>().material = Red;
            }
        }
        else if (nextMove() == "left")
        {
            if (inputController.inputDirectionLeftSide == InputController.Direction.Left)
            {
                Flag.GetComponent<MeshRenderer>().material = Green;
            }
            else
            {
                Flag.GetComponent<MeshRenderer>().material = Red;
            }
        }
        else if (nextMove() == "right")
        {
            if (inputController.inputDirectionRightSide == InputController.Direction.Right)
            {
                Flag.GetComponent<MeshRenderer>().material = Green;
            }
            else
            {
                Flag.GetComponent<MeshRenderer>().material = Red;
            }
        }

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
                GameObject starsClone = GameObject.Instantiate(stars);
                starsClone.transform.parent = gameObject.transform;
                starsClone.transform.position = new Vector3(128, 71, -5);
                StartCoroutine(RemoveObject(starsClone, 2.0f));
                success.Play();
            }
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
    private IEnumerator StopDamage()
    {
        healthCooldown = true;
        yield return new WaitForSeconds(0.25f);
        healthCooldown = false;
    }

    private IEnumerator Freeze(string obst)
    {
        while (freezed)
        {
            if (obst == "jump")
            {
                anim.SetInteger("AnimationPar", 4);
            }
            else if (obst == "left")
            {
                anim.SetInteger("AnimationPar", 6);
            }
            else if (obst == "right")
            {
                anim.SetInteger("AnimationPar", 5);
            }
            if (obst == "jump")
            {
                restoreSpeed = mapGenerator.moveBack;
                mapGenerator.moveBack = new Vector3(0, 0, 0);
                yield return new WaitForSeconds(0.5f);
                mapGenerator.moveBack = new Vector3(0, 0, 0.05f);
                yield return new WaitForSeconds(1f);
                mapGenerator.moveBack = new Vector3(0, 0, 0.3f);
                yield return new WaitForSeconds(0.1f);
                anim.SetInteger("AnimationPar", 1);
            }
            else if (obst == "left")
            {
                restoreSpeed = mapGenerator.moveBack;
                mapGenerator.moveBack = new Vector3(0, 0, 0);
                yield return new WaitForSeconds(0.8f);
                mapGenerator.moveBack = new Vector3(0, 0, 0.05f);
                yield return new WaitForSeconds(3.5f);
                anim.SetInteger("AnimationPar", 1);
            }
            else if (obst == "right")
            {
                restoreSpeed = mapGenerator.moveBack;
                mapGenerator.moveBack = new Vector3(0, 0, 0);
                yield return new WaitForSeconds(0.5f);
                mapGenerator.moveBack = new Vector3(0, 0, 0.05f);
                yield return new WaitForSeconds(3.4f);
                anim.SetInteger("AnimationPar", 1);
            }
            juicyText.SetActive(false);
            anim.applyRootMotion = true;
            Vector3 addUp = new Vector3(0, 0, (restoreSpeed.z - mapGenerator.moveBack.z) * 0.0625f);
            for (; mapGenerator.moveBack.z < restoreSpeed.z; mapGenerator.moveBack += addUp)
            {
                yield return new WaitForSeconds(0.25f);
            }
            freezed = false;
        }
    }
}
