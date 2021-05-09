using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    int BlockValue;
    public bool Minus;
    Text BlockTxt;
    Image img;

    void Start()
    {
        img = GetComponent<Image>();
        BlockTxt = GetComponentInChildren<Text>();
        BlockValue = Random.Range(0, 6);
        BlockTxt.text = BlockValue.ToString();
    }

    void Update()
    {
        if (FindObjectOfType<DragManager>().isTxt == true)
        {
            if (Minus == true && FindObjectOfType<DragManager>().DragObjValue > 1)
            {
                if (BlockValue != 0)
                {
                    BlockValue--;
                }
                Minus = false;
            }
            img.color = new Color(1, 1, 1);
            BlockTxt.text = BlockValue.ToString();
        }
    }
    public void Drag()
    {
        if (GameObject.Find("Canvas").GetComponent<DragManager>().isClick == true)
        {
            if (Minus == false)
            {
                FindObjectOfType<DragManager>().DragObjValue++;
                Minus = true;
                if (BlockValue != 0)
                    img.color = new Color(0.9f, 0.9f, 0.9f);
                Debug.Log("드래그 블럭 추가");
            }
        }
    }
    public void PointerDown()
    {
        FindObjectOfType<DragManager>().isClick = true;
        FindObjectOfType<DragManager>().isTxt = false;
        Debug.Log("드래그 시작");
        FindObjectOfType<DragManager>().DragObjValue = 0;

        if (Minus == false)
        {
            FindObjectOfType<DragManager>().DragObjValue++;
            Minus = true;
            if (BlockValue != 0)
                img.color = new Color(0.9f, 0.9f, 0.9f);
            Debug.Log("드래그 블럭 추가");
        }
    }

    public void PointerUp()
    {
        FindObjectOfType<DragManager>().isClick = false;
        FindObjectOfType<DragManager>().isTxt = true;

        if (FindObjectOfType<DragManager>().DragObjValue == 1)
        {
            FindObjectOfType<DragManager>().DragObjValue = 0;
        }
        Debug.Log("드래그 취소");
    }
}
