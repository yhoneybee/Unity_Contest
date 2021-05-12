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
        Debug.Log(debug);
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
