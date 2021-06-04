using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Algorithm : MonoBehaviour
{
    public static Algorithm Instance { get; set; }

    public Vector2Int cell_size;

    public List<List<int>> cell = new List<List<int>>();

    public Dictionary<string, int> portals = new Dictionary<string, int>();

    int lastLogic;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        CellReset();
        GameManager.instance.drag_end_callback += OnDragEnd;
    }

    // �巹�� ������ �Ҹ��� �Լ�
    private void OnDragEnd()
    {
        if (GameManager.instance.BlockPosition.Count > 1)
        {
            Debug.Log("DragEnd!");
            //���� ���⼭ ���� linked list���� �����°� �ϸ� ��
            CirculationClock(new Vector2Int(1, 1), new Vector2Int(3, 3));
        }
    }
    /// <summary>
    /// LT�� RB��ǥ�� �ѱ�� �� ���� �׵θ��� �ִ� ���� �����ش�.
    /// </summary>
    /// <param name="LT">LT��ǥ</param>
    /// <param name="RB">RB��ǥ</param>
    void CirculationClock(Vector2Int LT, Vector2Int RB)
    {
        List<Block> before = new List<Block>();

        Vector2Int add = new Vector2Int(1, 1);

        for (int i = 0; i < 2; i++)
        {
            for (int x = (add.x == 1 ? LT.x : RB.x); (add.x == 1 ? x <= RB.x : x >= LT.x); x += add.x)
                before.Add(GameManager.instance.Blocks[cell_size.x * (add.y == 1 ? LT.y : RB.y) + x].GetComponent<Block>());

            for (int y = (add.y == 1 ? LT.y : RB.y); (add.y == 1 ? y <= RB.y : y >= LT.y); y += add.y)
                before.Add(GameManager.instance.Blocks[cell_size.x * y + (add.x == 1 ? RB.x : LT.x)].GetComponent<Block>());

            add = new Vector2Int(-1, -1);
        }

        before = before.Distinct().ToList();

        int block_value = before[0].BlockValue;

        for (int i = before.Count - 1; i >= 0; i--)
        {
            if (before[i].BlockValue == 0)
                continue;
            if (before[i == before.Count - 1 ? 0 : i + 1].BlockValue == 0)
            {
                int next = i;
                while (true)
                {
                    next++;
                    if (next >= 8) break;
                    if (before[next == before.Count - 1 ? 0 : next + 1].BlockValue != 0)
                    {
                        before[next == before.Count - 1 ? 0 : next + 1].BlockValue = before[i].BlockValue;
                        break;
                    }
                }
            }
            else
                before[i == before.Count - 1 ? 0 : i + 1].BlockValue = before[i].BlockValue;
        }

        //before[1].BlockValue = block_value;
    }

    public void PortalCreate(Vector2Int enter, Vector2Int exit)
    {
        portals.Add(enter.ToString(), portals.Count);
        portals.Add(exit.ToString(), portals.Count);
    }

    void Update()
    {

    }
    /// <summary>
    /// ���� �������� ����� �Լ�
    /// </summary>
    /// <param name="start">���� ��ġ</param>
    /// <param name="block">���� ��ġ���� ���� �巡�׵� �� ��(loop)</param>
    /// <param name="loop_count">���� ���� �þ�� �ִ� ��(ex : ó�� �����̶�� �ִ밡 1�� )</param>
    void RandomPosAdd(Vector2Int start, int block, int loop_count)
    {
        Vector2Int axis = new Vector2Int();
        int axis_case, left;
        List<Vector2Int> com = new List<Vector2Int>();
        for (int i = 0; i < block; i++)
        {
            left = 0;
            cell[start.x][start.y]++;
            com.Add(start);

            while (true)
            {
                if (left >= 30)
                {
                    break;
                }

                axis_case = Random.Range(0, 4);

                switch (axis_case)
                {
                    case 0: axis = Vector2Int.up; break;
                    case 1: axis = Vector2Int.down; break;
                    case 2: axis = Vector2Int.left; break;
                    case 3: axis = Vector2Int.right; break;
                }

                if (cell_size.x <= start.x + axis.x ||
                    cell_size.y <= start.y + axis.y ||
                    0 > start.x + axis.x ||
                    0 > start.y + axis.y)
                {
                    left++;
                    continue;
                }

                if (cell[start.x + axis.x][start.y + axis.y] >= loop_count)
                {
                    left++;
                    continue;
                }

                left = 0;
                start += axis;

                break;
            }

            /*            Debug.Log($"left block : {Mathf.Abs(block - i)}\n i : {i}");*/
        }
        string debug = "";
        foreach (var item in com)
        {
            debug += $"{item.x},{item.y} -> ";
        }
        //Debug.Log(debug);
    }
    /// <summary>
    /// ������ ���ư��� �������� �������� �Ҹ��� �Լ�
    /// </summary>
    /// <param name="drag">�巡�� Ƚ��</param>
    public void Logic(int drag)
    {
        CellReset();

        lastLogic = drag;

        for (int i = 0; i < drag; i++)
            RandomPosAdd(new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)), Random.Range(2, cell_size.x * cell_size.y), i + 1);
    }

    public void ReRoll()
    {
        Logic(lastLogic);
        GameManager.instance.SetBlockValue();
        GameManager.instance.CreateUnBlock();
        GameManager.instance.ReRollCount = 0;
        GameManager.instance.Clear = false;
    }
    public void PrintCell()
    {
        string debug = "";
        for (int y = 0; y < cell_size.y; y++)
        {
            for (int x = 0; x < cell_size.x; x++)
            {
                debug += $"({x},{y})\t{cell[x][y]}\t";
            }
            debug += "\n";
        }
        Debug.Log(debug);
    }
    public void CellReset()
    {
        foreach (var c in cell)
            c.Clear();

        cell.Clear();

        for (int i = 0; i < cell_size.x; i++)
            cell.Add(new List<int>());

        for (int y = 0; y < cell_size.y; y++)
            for (int x = 0; x < cell_size.x; x++)
                cell[y].Add(0);
    }
}
