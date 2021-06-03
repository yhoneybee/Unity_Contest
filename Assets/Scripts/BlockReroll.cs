using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BlockReroll : MonoBehaviour
{
    private void Start()
    {

    }
    public void Reroll()
    {
        if (GameManager.instance.ReRollCount < 3)
        {
            List<int> TempValue = new List<int>();
            List<GameObject> LockBlock = new List<GameObject>();
            int cell_size_xy = Algorithm.Instance.cell_size.x * Algorithm.Instance.cell_size.y;
            int RandomBlcokSelect;

            for (int i = 0; i < cell_size_xy; i++) // TempValue �迭�� 0�� ������ ������ ���� ����ŭ ����
            {
                if (GameManager.instance.Blocks[i].GetComponent<Block>().isUnblock == false)
                {
                    TempValue.Add(GameManager.instance.Blocks[i].GetComponent<Block>().BlockValue);
                }
            }


            for (int i = 0; i < cell_size_xy-3; i++) // TempValue �迭�� 0�� ������ ������ ���� ����ŭ ����
            {
                RandomBlcokSelect = Random.Range(0, TempValue.Count);
                GameManager.instance.Blocks[i].GetComponent<Block>().BlockValue = TempValue[RandomBlcokSelect];
                TempValue.Remove(TempValue[RandomBlcokSelect]);
            }

            GameManager.instance.ReRollCount++;
        }
    }
}
