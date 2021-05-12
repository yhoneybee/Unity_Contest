using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Algorithm : MonoBehaviour
{
    public static Algorithm Instance { get; set; }

    public Vector2Int cell_size;

    public List<List<int>> cell = new List<List<int>>();

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

    // 드레그 끝나고 불리는 함수
    private void OnDragEnd()
    {
        if (GameManager.instance.BlockPosition.Count > 1)
        {
            Debug.Log("DragEnd!");
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
        List<Block> after = new List<Block>();
        for (int x = LT.x; x <= RB.x; x++)
            before.Add(GameManager.instance.Blocks[cell_size.x * LT.y + x].GetComponent<Block>());
        for (int y = LT.y; y <= RB.y; y++)
            before.Add(GameManager.instance.Blocks[cell_size.x * y + RB.x].GetComponent<Block>());
        for (int x = RB.x; x >= LT.x; x--)
            before.Add(GameManager.instance.Blocks[cell_size.x * RB.y + x].GetComponent<Block>());
        for (int y = RB.y; y >= LT.y; y--)
            before.Add(GameManager.instance.Blocks[cell_size.x * y + LT.x].GetComponent<Block>());

        for (int i = 0; i < before.Count; i++)
        {
            after.Add(before[i + 1 == before.Count ? 0 : i + 1]);
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
        Debug.Log(debug);
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
