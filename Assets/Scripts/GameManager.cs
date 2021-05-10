using System.Collections.Generic;
using System.Linq;
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
        Algorithm.Instance.Logic(25);
        Algorithm.Instance.PrintCell();

        Blocks.Sort((o1, o2) => int.Parse(o1.name).CompareTo(int.Parse(o2.name)));

        for (int y = 0; y < Algorithm.Instance.cell_size.y; y++)
        {
            for (int x = 0; x < Algorithm.Instance.cell_size.x; x++)
            {
                Debug.Log($"{x},{y}:{Blocks[Algorithm.Instance.cell_size.x * y + x].name}");
                Blocks[Algorithm.Instance.cell_size.x * y + x].GetComponent<Block>().BlockValue = Algorithm.Instance.cell[x][y];
            }
        }

        Algorithm.Instance.CellReset();
    }

    private void Update()
    {
        //foreach(var GameObj in BlockPosition)
        //{
        //    Debug.Log(GameObj);
        //}
    }
}