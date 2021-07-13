using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public delegate void CALLBACK();
    public CALLBACK drag_end_callback;
    public delegate void CALLBACK_GAMEOBJECT(GameObject obj);
    public CALLBACK_GAMEOBJECT draging_callback;

    public static GameManager instance;
    public List<GameObject> Blocks { get; set; } = new List<GameObject>();
    public List<GameObject> BlockPosition { get; set; } = new List<GameObject>();
    public List<GameObject> UndoList = new List<GameObject>();
    public List<GameObject> TempBlockList { get; set; } = new List<GameObject>();
    private List<int> Test = new List<int>();
    public GameObject ExplosionPrefab;

    [Header("로직 부를 횟수")]
    public int logic_count = 25;

    [Space(20)]
    public bool isClick;
    public bool SameBlock;
    public int cell_size_xy;
    public int Unblocks;
    public sbyte ReRollCount;
    public bool Clear;
    public int DragCount { get; set; }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        drag_end_callback = () => { };
        draging_callback = (o) => { };

        cell_size_xy = Algorithm.Instance.cell_size.x * Algorithm.Instance.cell_size.y;

        for (int i = 0; i < cell_size_xy; i++)
        {
            GameObject temp = Instantiate(Resources.Load<GameObject>("Block"));
            temp.name = $"{i + 1}";
            temp.transform.SetParent(GameObject.Find("Plate").transform);
            temp.transform.localScale = Vector3.one;
            temp.GetComponent<Block>().myBlockNumber = i;
        }

        //Blocks.Sort((o1, o2) => o1.GetComponent<Block>().myBlockNumber.CompareTo(o2.GetComponent<Block>().myBlockNumber));

        Algorithm.Instance.Logic(logic_count);
        Algorithm.Instance.PrintCell();

        SetBlockValue();

        Algorithm.Instance.CellReset();

        if (DonDestroy.instance.ModeSelect == 1)
            CreateUnBlock();
    }

    public void SetBlockValue()
    {
        DragCount = 0;
        for (int y = 0; y < Algorithm.Instance.cell_size.y; y++)
        {
            for (int x = 0; x < Algorithm.Instance.cell_size.x; x++)
            {
                //Debug.Log($"{x},{y}:{Blocks[Algorithm.Instance.cell_size.x * y + x].name}");
                Blocks[Algorithm.Instance.cell_size.x * y + x].GetComponent<Block>().BlockValue = Algorithm.Instance.cell[x][y];
            }
        }
    }

    private void Update()
    {
        int i = 0;
        foreach (var item in Blocks)
        {
            if (item.GetComponent<Block>().isUnblock == false)
            {
                if (item.GetComponent<Block>().BlockValue == 0)
                {
                    i++;
                }
            }
        }
        if (i == cell_size_xy)
        {
            Clear = true;
        }
    }

    public void Undo()
    {
        if (UndoList.Count != 0)
        {
            Debug.Log(UndoList.Count);
            if (DonDestroy.instance.ModeSelect == 3)
                for (int i = 0; i < cell_size_xy; i++)
                    Blocks[i].GetComponent<Block>().BlockValue = Algorithm.Instance.undo_block_value[i];
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
    public void CreateUnBlock()
    {
        Unblocks = 0;
        foreach (var item in Blocks)
        {
            item.GetComponent<Block>().isUnblock = false;
        }
        for (int i = 0; i < 3;)
        {
            int RandomUnBlock = Random.Range(0, cell_size_xy);
            if (Blocks[RandomUnBlock].GetComponent<Block>().isUnblock == false)
            {
                Blocks[RandomUnBlock].GetComponent<Block>().isUnblock = true;
                Unblocks++;
                i++;
                Debug.Log("isUnBlock");
            }
        }
    }
}