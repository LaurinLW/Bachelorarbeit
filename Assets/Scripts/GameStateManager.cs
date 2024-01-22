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

    // Update is called once per frame
    public void exec()
    {
        mapGen.moveBack = new Vector3(0, 0, 0.3f * GameSpeed);
    }
}
