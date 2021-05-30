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
        List<int> TempValue = new List<int>();
        int cell_size_xy = Algorithm.Instance.cell_size.x * Algorithm.Instance.cell_size.y;
        int RandomBlcokSelect;
        string DebugTemp = "";
        string DebugReroll = "";

        for (int i=0;i<cell_size_xy;i++) // TempValue �迭�� 0�� ������ ������ ���� ����ŭ ����
        {
            TempValue.Add(GameManager.instance.Blocks[i].GetComponent<Block>().BlockValue);
        }
        
        foreach(var item in TempValue)
        {
            DebugTemp += item.ToString() + "/";
        }

        for (int i = 0; i < cell_size_xy; i++) // TempValue �迭�� 0�� ������ ������ ���� ����ŭ ����
        {
            RandomBlcokSelect = Random.Range(0, TempValue.Count);
            GameManager.instance.Blocks[i].GetComponent<Block>().BlockValue = TempValue[RandomBlcokSelect];
            DebugReroll += TempValue[RandomBlcokSelect].ToString() + "/";
            TempValue.Remove(TempValue[RandomBlcokSelect]);
        }

        Debug.Log(DebugTemp);
        Debug.Log(DebugReroll);
    }
}
