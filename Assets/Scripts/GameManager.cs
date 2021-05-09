using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<GameObject> Blocks = new List<GameObject>();
    public List<GameObject> BlockPosition = new List<GameObject>();

    public bool isClick;
    public bool SameBlock;

    void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //foreach(var GameObj in BlockPosition)
        //{
        //    Debug.Log(GameObj);
        //}
    }
}