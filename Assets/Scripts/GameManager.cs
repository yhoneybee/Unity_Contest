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
    void Start()
    {
        Blocks.Sort((obj1,obj2) => int.Parse(obj1.name).CompareTo(int.Parse(obj2.name)));
        Algorithm.Instance.CellReset();
        Algorithm.Instance.Logic(1);
        Algorithm.Instance.PrintCell();

        for (int y = 0; y < Algorithm.Instance.cell_size.y; y++)
        {
            for (int x = 0; x < Algorithm.Instance.cell_size.x; x++)
            {
                Debug.Log($"{x},{y}:{Blocks[Algorithm.Instance.cell_size.x * y + x].name}");
                Blocks[Algorithm.Instance.cell_size.x * y + x].GetComponent<Block>().BlockValue = Algorithm.Instance.cell[x][y];
            }
        }
    }

    private void Update()
    {
        //foreach(var GameObj in BlockPosition)
        //{
        //    Debug.Log(GameObj);
        //}
    }
}