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
        PortalCreate(new Vector2Int(2, 2), new Vector2Int(4, 0), true);
        //PortalCreate(new Vector2Int(4, 4), new Vector2Int(0, 4));
    }

    // 드레그 끝나고 불리는 함수
    private void OnDragEnd()
    {
        if (GameManager.instance.BlockPosition.Count > 1)
        {
            //이제 여기서 블럭을 linked list마냥 돌리는거 하면 됨
            CirculationClock(new Vector2Int(1, 1), new Vector2Int(3, 3));
        }
    }
    /// <summary>
    /// LT와 RB좌표를 넘기면 그 구역 테두리에 있는 블럭을 돌려준다.
    /// </summary>
    /// <param name="LT">LT좌표</param>
    /// <param name="RB">RB좌표</param>
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

        public Vector2 enter_dir; // 들어가는 방향
        public Vector2 exit_dir;  // 나오는 방향
    }

    public void PortalCreate(Vector2Int enter, Vector2Int exit, bool isRandomDir = false)
    {
        Debug.Log($"{enter} / {exit}"); // (x, y)

        Portal portal = new Portal();

        portal.enter_pos = enter;
        portal.exit_pos = exit;

        if (isRandomDir)
        {
            if (enter.x == 0 || enter.y == 0)
            {
                if (enter.x == 0 && enter.y == 0)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0: portal.enter_dir = Vector2Int.up; break;
                        case 1: portal.enter_dir = Vector2Int.left; break;
                    }
                }
                else if (enter.x == 0)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.enter_dir = Vector2Int.up; break;
                        case 1: portal.enter_dir = Vector2Int.left; break;
                        case 2: portal.enter_dir = Vector2Int.down; break;
                    }
                    if (enter.y == cell_size.y - 1)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0: portal.enter_dir = Vector2Int.down; break;
                            case 1: portal.enter_dir = Vector2Int.left; break;
                        }
                    }
                }
                else if (enter.y == 0)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.enter_dir = Vector2Int.up; break;
                        case 1: portal.enter_dir = Vector2Int.left; break;
                        case 2: portal.enter_dir = Vector2Int.right; break;
                    }
                    if (enter.x == cell_size.x - 1)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0: portal.enter_dir = Vector2Int.up; break;
                            case 1: portal.enter_dir = Vector2Int.right; break;
                        }
                    }
                }
            }
            else if (enter.x == cell_size.x - 1 || enter.y == cell_size.y - 1)
            {
                if (enter.x == cell_size.x - 1 && enter.y == cell_size.y - 1)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0: portal.enter_dir = Vector2Int.down; break;
                        case 1: portal.enter_dir = Vector2Int.right; break;
                    }
                }
                else if (enter.x == cell_size.x - 1)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.enter_dir = Vector2Int.up; break;
                        case 1: portal.enter_dir = Vector2Int.right; break;
                        case 2: portal.enter_dir = Vector2Int.down; break;
                    }
                }
                else if (enter.y == cell_size.y - 1)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.enter_dir = Vector2Int.down; break;
                        case 1: portal.enter_dir = Vector2Int.left; break;
                        case 2: portal.enter_dir = Vector2Int.right; break;
                    }
                }
            }
            else
            {
                switch (Random.Range(0, 4))
                {
                    case 0: portal.enter_dir = Vector2Int.up; break;
                    case 1: portal.enter_dir = Vector2Int.down; break;
                    case 2: portal.enter_dir = Vector2Int.right; break;
                    case 3: portal.enter_dir = Vector2Int.left; break;
                }
            }

            if (exit.x == 0 || exit.y == 0)
            {
                if (exit.x == 0 && exit.y == 0)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0: portal.exit_dir = Vector2Int.down; break;
                        case 1: portal.exit_dir = Vector2Int.right; break;
                    }
                }
                else if (exit.x == 0)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.exit_dir = Vector2Int.up; break;
                        case 1: portal.exit_dir = Vector2Int.right; break;
                        case 2: portal.exit_dir = Vector2Int.down; break;
                    }
                    if (exit.y == cell_size.y - 1)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0: portal.exit_dir = Vector2Int.up; break;
                            case 1: portal.exit_dir = Vector2Int.right; break;
                        }
                    }
                }
                else if (exit.y == 0)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.exit_dir = Vector2Int.down; break;
                        case 1: portal.exit_dir = Vector2Int.left; break;
                        case 2: portal.exit_dir = Vector2Int.right; break;
                    }
                    if (exit.x == cell_size.x - 1)
                    {
                        switch (Random.Range(0, 2))
                        {
                            case 0: portal.exit_dir = Vector2Int.down; break;
                            case 1: portal.exit_dir = Vector2Int.left; break;
                        }
                    }
                }
            }
            else if (exit.x == cell_size.x - 1 || exit.y == cell_size.y - 1)
            {
                if (exit.x == cell_size.x - 1 && exit.y == cell_size.y - 1)
                {
                    switch (Random.Range(0, 2))
                    {
                        case 0: portal.exit_dir = Vector2Int.up; break;
                        case 1: portal.exit_dir = Vector2Int.left; break;
                    }
                }
                else if (exit.x == cell_size.x - 1)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.exit_dir = Vector2Int.up; break;
                        case 1: portal.exit_dir = Vector2Int.left; break;
                        case 2: portal.exit_dir = Vector2Int.down; break;
                    }
                }
                else if (exit.y == cell_size.y - 1)
                {
                    switch (Random.Range(0, 3))
                    {
                        case 0: portal.exit_dir = Vector2Int.up; break;
                        case 1: portal.exit_dir = Vector2Int.left; break;
                        case 2: portal.exit_dir = Vector2Int.right; break;
                    }
                }
            }
            else
            {
                switch (Random.Range(0, 4))
                {
                    case 0: portal.exit_dir = Vector2Int.up; break;
                    case 1: portal.exit_dir = Vector2Int.down; break;
                    case 2: portal.exit_dir = Vector2Int.right; break;
                    case 3: portal.exit_dir = Vector2Int.left; break;
                }
            }
        }
        else
        {
            if (enter.x == 0)
            {
                portal.enter_dir = Vector2Int.left;
                portal.exit_dir = Vector2Int.left;
            }
            else
            {
                portal.enter_dir = Vector2Int.right;
                portal.exit_dir = Vector2Int.right;
            }
        }

        Debug.Log($"{portal.enter_dir} / {portal.exit_dir}");

        portals.Add(portal);

        Block block_enter = GameManager.instance.Blocks[enter.y * cell_size.x + enter.x].GetComponent<Block>();
        Block block_exit = GameManager.instance.Blocks[exit.y * cell_size.x + exit.x].GetComponent<Block>();

        block_enter.isPortal = true;
        block_exit.isPortal = true;

        if (portal.enter_dir.x != 0)
        {
            if (portal.enter_dir.x > 0)
                block_enter.BlockValueTxt.text = "I＞";
            else
                block_enter.BlockValueTxt.text = "I＜";
        }
        else if (portal.enter_dir.y != 0)
        {
            if (portal.enter_dir.y > 0)
                block_enter.BlockValueTxt.text = "I∧";
            else
                block_enter.BlockValueTxt.text = "I∨";
        }

        if (portal.exit_dir.x != 0)
        {
            if (portal.exit_dir.x > 0)
                block_exit.BlockValueTxt.text = "O＞";
            else
                block_exit.BlockValueTxt.text = "O＜";
        }
        else if (portal.exit_dir.y != 0)
        {
            if (portal.exit_dir.y > 0)
                block_exit.BlockValueTxt.text = "O∧";
            else
                block_exit.BlockValueTxt.text = "O∨";
        }

        block_enter.BlockValue = -1;
        block_exit.BlockValue = -1;

        block_enter.BlockValueTxt.color = new Color(1, 1, 1);
        block_exit.BlockValueTxt.color = new Color(1, 1, 1);

        block_enter.img.color = new Color(0.1f, 0.3f, 0.5f);
        block_exit.img.color = new Color(0.1f, 0.5f, 0.5f);
    }

    public void Warp()
    {
        foreach (var p in portals)
        {
            Block enter_target = GameManager.instance.Blocks[(int)(p.enter_pos.y + p.enter_dir.y * -1) * cell_size.x + (int)(p.enter_pos.x + p.enter_dir.x * -1)].GetComponent<Block>();
            //행과 열 이동 구현후에 만들것. ex ) 2048의 밀리는 이동
        }
    }

    void Update()
    {

    }
    /// <summary>
    /// 숫자 랜덤으로 만드는 함수
    /// </summary>
    /// <param name="start">시작 위치</param>
    /// <param name="block">시작 위치에서 부터 드래그될 블럭 수(loop)</param>
    /// <param name="loop_count">지금 돌때 늘어나는 최대 수(ex : 처음 실행이라면 최대가 1임 )</param>
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
    /// 실제로 돌아가는 스테이지 시작전에 불리는 함수
    /// </summary>
    /// <param name="drag">드래그 횟수</param>
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
