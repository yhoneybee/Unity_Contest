using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Algorithm : MonoBehaviour
{
    public static Algorithm Instance { get; set; }

    // 이거만 바꾸면 GameManager가 실행될때에 알아서 됨
    public Vector2Int cell_size;

    public List<List<int>> cell = new List<List<int>>();

    public List<Portal> portals = new List<Portal>();

    public List<int> undo_block_value = new List<int>();

    int lastLogic;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        cell_size = DonDestroy.instance.cell_size;

        undo_block_value.Clear();

        var linq = from block in GameManager.instance.Blocks
                   select block.GetComponent<Block>().BlockValue;

        undo_block_value.AddRange(linq);
        CellReset();
        GameManager.instance.drag_end_callback += OnDragEnd;
        GameManager.instance.draging_callback += OnDraging;
        if (DonDestroy.instance.ModeSelect == 2)
        {
            PortalCreate(new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)), new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)));
            PortalCreate(new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)), new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)));
        }
    }

    IEnumerator CFlashing(List<Vector2Int> exit_near)
    {
        List<Block> near_blocks = new List<Block>();

        foreach (var near in exit_near)
            if (near.x >= 0 && near.x < cell_size.x && near.y >= 0 && near.y < cell_size.y)
                near_blocks.Add(GameManager.instance.Blocks.Find((o) =>
                { return o.GetComponent<Block>().myBlockNumber == near.y * cell_size.x + near.x; }
                )?.GetComponent<Block>());

        foreach (var block in near_blocks)
            block.img.color = new Color(block.img.color.r, block.img.color.g, block.img.color.b, 0);

        while (true)
        {
            if (GameManager.instance.BlockPosition.Count > 0)
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
                        if (!near_blocks[i].GetComponent<Block>().isUnblock)
                        {
                            if (near_blocks[i].img.color.a < 1)
                                near_blocks[i].img.color += new Color(0, 0, 0, 0.003921568627451f);
                            else
                                near_blocks[i].img.color = new Color(near_blocks[i].img.color.r, near_blocks[i].img.color.g, near_blocks[i].img.color.b, 0);
                        }
                        yield return null;
                    }
                }
            }
            else
                break;

            yield return null;
        }
    }

    private void OnDraging(GameObject obj)
    {
        Block draged = obj.GetComponent<Block>();
        if (draged.isPortal)
        {
            print("포탈을 드레그 중 입니다.");
            foreach (var portal in portals)
            {
                if (portal.enter_pos.y * cell_size.x + portal.enter_pos.x == draged.myBlockNumber)
                {
                    print("포탈의 입구를 드레그 하였습니다. 출구로 이동합니다.");
                    SoundManager.Instance.Play("PortalinEffect");
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


    // 드레그 끝나고 불리는 함수
    private void OnDragEnd()
    {
        if (DonDestroy.instance.ModeSelect==3)
        {
            if (GameManager.instance.BlockPosition.Count > 1)
            {
                //이제 여기서 블럭을 linked list마냥 돌리는거 하면 됨
                CirculationClock(new Vector2Int(1, 1), new Vector2Int(3, 3));
                SoundManager.Instance.Play("RotateEffect");
            }
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

        undo_block_value.Clear();

        var linq = from block in GameManager.instance.Blocks
                   select block.GetComponent<Block>().BlockValue;

        undo_block_value.AddRange(linq);

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
                        if (next >= before.Count) break;
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

        for (int i = 1; i < before.Count; i++)
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

    public void PortalCreate(Vector2Int enter, Vector2Int exit)
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

    void Update()
    {
        foreach (var portal in portals)
        {
            List<Vector2Int> exit_v2i = new List<Vector2Int>();
            List<Vector2Int> enter_v2i = new List<Vector2Int>();
            List<Block> exit_near = new List<Block>();
            List<Block> enter_near = new List<Block>();

            exit_v2i.Add(portal.exit_pos + new Vector2Int(1, 0));
            exit_v2i.Add(portal.exit_pos + new Vector2Int(-1, 0));
            exit_v2i.Add(portal.exit_pos + new Vector2Int(0, 1));
            exit_v2i.Add(portal.exit_pos + new Vector2Int(0, -1));

            enter_v2i.Add(portal.enter_pos + new Vector2Int(1, 0));
            enter_v2i.Add(portal.enter_pos + new Vector2Int(-1, 0));
            enter_v2i.Add(portal.enter_pos + new Vector2Int(0, 1));
            enter_v2i.Add(portal.enter_pos + new Vector2Int(0, -1));

            foreach (var near in exit_v2i)
                if (near.x >= 0 && near.x < cell_size.x && near.y >= 0 && near.y < cell_size.y)
                    exit_near.Add(GameManager.instance.Blocks.Find((o) =>
                    {
                        return o.GetComponent<Block>().myBlockNumber == near.y * cell_size.x + near.x;
                    }
                    ).GetComponent<Block>());

            foreach (var near in enter_v2i)
                if (near.x >= 0 && near.x < cell_size.x && near.y >= 0 && near.y < cell_size.y)
                    enter_near.Add(GameManager.instance.Blocks.Find((o) =>
                    {
                        return o.GetComponent<Block>().myBlockNumber == near.y * cell_size.x + near.x;
                    }
                    ).GetComponent<Block>());

            var exit_q = from near in exit_near
                         where near.isUnblock || near.BlockValue <= 0
                         select near;

            var enter_q = from near in enter_near
                          where near.isUnblock || near.BlockValue <= 0
                          select near;

            if (enter_q.Count() == enter_near.Count)
            {
                Block block = GameManager.instance.Blocks.Find((o) => { return o.GetComponent<Block>().myBlockNumber == (portal.enter_pos.y * cell_size.y + portal.enter_pos.x); }).GetComponent<Block>();
                block.isPortal = false;
                block.BlockValue = 0;
                portal.enter_pos = new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y));
                block = GameManager.instance.Blocks.Find((o) => { return o.GetComponent<Block>().myBlockNumber == (portal.enter_pos.y * cell_size.y + portal.enter_pos.x); }).GetComponent<Block>();
                block.BlockValueTxt.text = "I";
                block.isPortal = true;
                block.isEnter = true;
                block.isExit = false;
            }
            if (exit_q.Count() == exit_near.Count)
            {
                Block block = GameManager.instance.Blocks.Find((o) => { return o.GetComponent<Block>().myBlockNumber == (portal.exit_pos.y * cell_size.y + portal.exit_pos.x); }).GetComponent<Block>();
                block.isPortal = false;
                block.BlockValue = 0;
                portal.exit_pos = new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y));
                block = GameManager.instance.Blocks.Find((o) => { return o.GetComponent<Block>().myBlockNumber == (portal.exit_pos.y * cell_size.y + portal.exit_pos.x); }).GetComponent<Block>();
                block.BlockValueTxt.text = "O";
                block.isPortal = true;
                block.isEnter = false;
                block.isExit = true;

            }
        }
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
        if (DonDestroy.instance.ModeSelect == 2)
        {
            foreach (var portal in portals)
            {
                Block enter = GameManager.instance.Blocks.Find((o) => { return o.GetComponent<Block>().myBlockNumber == (portal.enter_pos.y * cell_size.y + portal.enter_pos.x); })?.GetComponent<Block>();
                Block exit = GameManager.instance.Blocks.Find((o) => { return o.GetComponent<Block>().myBlockNumber == (portal.exit_pos.y * cell_size.y + portal.exit_pos.x); })?.GetComponent<Block>();

                enter.isEnter = false;
                exit.isExit = false;
                enter.isPortal = false;
                exit.isPortal = false;
            }
            portals.Clear();
            
            PortalCreate(new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)), new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)));
            PortalCreate(new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)), new Vector2Int(Random.Range(0, cell_size.x), Random.Range(0, cell_size.y)));
        }
        Logic(lastLogic);
        GameManager.instance.SetBlockValue();
        if (DonDestroy.instance.ModeSelect == 1)
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
