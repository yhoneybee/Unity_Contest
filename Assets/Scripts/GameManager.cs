using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void CALLBACK();
    public CALLBACK drag_end_callback;
    public delegate void CALLBACK_GAMEOBJECT(GameObject obj);
    public CALLBACK_GAMEOBJECT draging_callback;
    public int Score;

    public static GameManager instance;
    public List<GameObject> Blocks { get; set; } = new List<GameObject>();
    public List<GameObject> BlockPosition { get; set; } = new List<GameObject>();
    public List<GameObject> UndoList = new List<GameObject>();
    public List<GameObject> TempBlockList { get; set; } = new List<GameObject>();
    private List<int> Test = new List<int>();
    public GameObject ExplosionPrefab;

    public Block[,] ingame_block;

    [Header("로직 부를 횟수")]
    public int logic_count = 25;

    [Space(20)]
    public bool isClick;
    public bool SameBlock;
    public int cell_size_xy;
    public int Unblocks;
    public sbyte ReRollCount;
    public bool Clear { get; set; }
    public int DragCount { get; set; }

    public bool block2zero = false;
    [HideInInspector]
    public int block2zero_count = 3;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Algorithm.Instance.cell_size = DonDestroy.instance.cell_size;

        drag_end_callback = () => { };
        draging_callback = (o) => { };

        cell_size_xy = Algorithm.Instance.cell_size.x * Algorithm.Instance.cell_size.y;

        ingame_block = new Block[Algorithm.Instance.cell_size.x, Algorithm.Instance.cell_size.y];

        Transform plate = GameObject.Find("Plate").transform;
        float val = 10.0f / Algorithm.Instance.cell_size.x * 25.0f;
        plate.GetComponent<GridLayoutGroup>().cellSize = new Vector2((int)val, (int)val);
        plate.GetComponent<GridLayoutGroup>().constraintCount = Algorithm.Instance.cell_size.x;
        Block block_temp = null;

        for (int y = 0; y < Algorithm.Instance.cell_size.y; y++)
        {
            for (int x = 0; x < Algorithm.Instance.cell_size.x; x++)
            {
                GameObject temp = Instantiate(Resources.Load<GameObject>("Block"));
                temp.name = $"{x + Algorithm.Instance.cell_size.x * y}";
                temp.transform.SetParent(plate);
                temp.transform.localScale = Vector3.one;
                block_temp = temp.GetComponent<Block>();
                block_temp.myBlockNumber = x + Algorithm.Instance.cell_size.x * y;
                ingame_block[x, y] = block_temp;
            }
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
            Block temp = item.GetComponent<Block>();
            if (temp.isUnblock == false)
            {
                if (temp.BlockValue == 0 || temp.BlockValue == -1)
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
        SoundManager.Instance.Play("SkillClick");

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

    public void SwitchSetZeroBool()
    {
        SoundManager.Instance.Play("SkillClick");

        if (block2zero_count > 0)
            block2zero = !block2zero;
        Text text = GameObject.Find("BlockValue2Zero").transform.GetChild(0).GetComponent<Text>();
        if (block2zero)
            text.color = Color.yellow;
        else
            text.color = Color.white;
    }

    public void SetBlockZero(Block block)
    {
        if (block2zero && block2zero_count > 0)
        {
            if (block.isPortal || block.BlockValue <= 0) return;

            --block2zero_count; block2zero = false;

            GameObject.Find("BlockValue2Zero").transform.GetChild(0).GetComponent<Text>().color = Color.white;

            block.BlockValue = 0;
        }
    }
}