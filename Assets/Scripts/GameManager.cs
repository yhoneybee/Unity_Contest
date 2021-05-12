using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void CALLBACK();
    public CALLBACK drag_end_callback;

    public static GameManager instance;
    public List<GameObject> Blocks { get; set; } = new List<GameObject>();
    public List<GameObject> BlockPosition { get; set; } = new List<GameObject>();
    public List<GameObject> UndoList = new List<GameObject>();

    public bool isClick;
    public bool SameBlock;
    public int DragCount { get; set; }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Algorithm.Instance.Logic(10);
        Algorithm.Instance.PrintCell();

        Blocks.Sort((o1, o2) => int.Parse(o1.name).CompareTo(int.Parse(o2.name)));

        SetBlockValue();

        Algorithm.Instance.CellReset();
    }

    public void SetBlockValue()
    {
        DragCount = 0;
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
    }

    public void Undo()
    {
        if (UndoList.Count !=0)
        {
            Debug.Log(UndoList.Count);
            foreach (var GameObj in UndoList)
            {
                GameObj.GetComponent<Block>().BlockValue++;
            }
            UndoList.Clear();
            if (DragCount > 0)
            {
                DragCount--;
            }
        }
    }
}