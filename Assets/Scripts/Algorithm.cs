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

    public List<Portal> portals = new List<Portal>();

    int lastLogic;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        CellReset();
        GameManager.instance.drag_end_callback += OnDragEnd;
        GameManager.instance.draging_callback += OnDraging;
        PortalCreate(new Vector2Int(2, 2), new Vector2Int(4, 0), true);
        //PortalCreate(new Vector2Int(4, 4), new Vector2Int(0, 4));
    }

    IEnumerator CFlashing(List<Vector2Int> exit_near)
    {
        List<Block> near_blocks = new List<Block>();

        foreach (var near in exit_near)
            if (near.x >= 0 && near.x < cell_size.x && near.y >= 0 && near.y < cell_size.y)
                near_blocks.Add(GameManager.instance.Blocks.Find((o) =>
                {
                    return o.GetComponent<Block>().myBlockNumber == near.y * cell_size.x + near.x;
                })?.GetComponent<Block>());

        foreach (var block in near_blocks)
            block.img.color = new Color(block.img.color.r, block.img.color.g, block.img.color.b, 0);

        while (true)
        {
            var linq = from near in near_blocks
                       where near.gameObject == GameManager.instance.BlockPosition.Last()
                       select near;

            if (linq.Count() > 0)
            {
                foreach (var block in near_blocks)
                    block.img.color = new Color(block.img.color.r, block.img.color.g, block.img.color.b, 1);
                break;
            }
            else
            {
                for (int i = 0; i < near_blocks.Count; i++)
                {
                    if (near_blocks[i].img.color.a < 1)
                    {
                        near_blocks[i].img.color += new Color(0, 0, 0, 0.003921568627451f);
                    }
                    else
                    {
                        near_blocks[i].img.color = new Color(near_blocks[i].img.color.r, near_blocks[i].img.color.g, near_blocks[i].img.color.b, 0);
                    }
                    yield return null;
                }
            }

            yield return null;
        }
    }

    private void OnDraging(GameObject obj)
    {
        Block draged = obj.GetComponent<Block>();
        if (draged.isPortal)
        {
            print("��Ż�� �巹�� �� �Դϴ�.");
            foreach (var portal in portals)
            {
                if (portal.enter_pos.y * cell_size.x + portal.enter_pos.x == draged.myBlockNumber)
                {
                    print("��Ż�� �Ա��� �巹�� �Ͽ����ϴ�. �ⱸ�� �̵��մϴ�.");
                    GameManager.instance.BlockPosition.Add(GameManager.instance.Blocks.Find((o) =>
                    {
                        StartCoroutine(CFlashing(new List<Vector2Int>
                        {
                            portal.exit_pos + new Vector2Int(1, 0),
                            portal.exit_pos + new Vector2Int(-1, 0),
                            portal.exit_pos + new Vector2Int(0, 1),
                            portal.exit_pos + new Vector2Int(0, -1),
                        }));
                        return o.GetComponent<Block>().myBlockNumber == portal.exit_pos.y * cell_size.x + portal.exit_pos.x;
                    }));
                    return;
                }
            }
        }
    }


    // �巹�� ������ �Ҹ��� �Լ�
    private void OnDragEnd()
    {
        if (GameManager.instance.BlockPosition.Count > 1)
        {
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
            if (before[i].BlockValue != 0)
            {
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
        }

        for (int i = 1; i < 8; i++)
            if (before[i].BlockValue != 0)
            {
                if (block_value == 0) break;
                before[i].BlockValue = block_value;
                break;
            }
    }

    public class Portal
    {
        public Vector2Int enter_pos;
        public Vector2Int exit_pos;
    }

    public void PortalCreate(Vector2Int enter, Vector2Int exit, bool isRandomDir = false)
    {
        Debug.Log($"{enter} / {exit}"); // (x, y)

        Portal portal = new Portal();

        portal.enter_pos = enter;
        portal.exit_pos = exit;

        portals.Add(portal);

        Block block_enter = GameManager.instance.Blocks[enter.y * cell_size.x + enter.x].GetComponent<Block>();
        Block block_exit = GameManager.instance.Blocks[exit.y * cell_size.x + exit.x].GetComponent<Block>();

        block_enter.isPortal = true;
        block_enter.isEnter = true;
        block_exit.isPortal = true;
        block_exit.isExit = true;

        block_enter.BlockValueTxt.text = "I";
        block_exit.BlockValueTxt.text = "O";

        block_enter.BlockValue = -1;
        block_exit.BlockValue = -1;

        block_enter.BlockValueTxt.color = new Color(1, 1, 1);
        block_exit.BlockValueTxt.color = new Color(1, 1, 1);

        block_enter.img.color = new Color(0.1f, 0.3f, 0.5f);
        block_exit.img.color = new Color(0.1f, 0.5f, 0.5f);
    }

    public void Warp()
    {
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
