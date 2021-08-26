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
        int count = 0;
        SoundManager.Instance.Play("SkillClick");

        if (GameManager.instance.ReRollCount < 3)
        {
            List<int> TempValue = new List<int>();
            int cell_size_xy = Algorithm.Instance.cell_size.x * Algorithm.Instance.cell_size.y;
            int RandomBlcokSelect;

            for (int i = 0; i < cell_size_xy; i++) // TempValue 배열에 0번 블럭부터 생성된 블럭의 수만큼 넣음
            {
                Block block = GameManager.instance.Blocks[i].GetComponent<Block>();
                if (block.isUnblock == false && block.BlockValue > 0 && block.isPortal == false)
                {
                    TempValue.Add(GameManager.instance.Blocks[i].GetComponent<Block>().BlockValue);
                }
                else
                {
                    count++;
                }
            }


            for (int i = 0; i < cell_size_xy - count; i++) // TempValue 배열에 0번 블럭부터 생성된 블럭의 수만큼 넣음
            {
                RandomBlcokSelect = Random.Range(0, TempValue.Count);
                GameManager.instance.Blocks[i].GetComponent<Block>().BlockValue = TempValue[RandomBlcokSelect];
                TempValue.Remove(TempValue[RandomBlcokSelect]);
            }

            GameManager.instance.ReRollCount++;
        }
    }
}
