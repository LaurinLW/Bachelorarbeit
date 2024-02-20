using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public float GameSpeed = 1;
    public MapGenerator mapGen;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void exec()
    {
        mapGen.moveBack = new Vector3(0, 0, 0.4f * GameSpeed);
        mapGen.scoreValue = 0;
    }

    public void restart()
    {
        mapGen.moveBack = new Vector3(0, 0, 0.4f * GameSpeed);
        mapGen.restart();
        mapGen.scoreValue = 0;
    }
}
