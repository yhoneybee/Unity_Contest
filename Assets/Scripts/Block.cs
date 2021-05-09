using UnityEngine;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    int BlockValue = 5;//Random.Range(0, 6);
    public bool Minus;
    Text BlockTxt;

    void Start()
    {
        BlockTxt = GetComponentInChildren<Text>();
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
                Debug.Log("�巡�� �� �߰�");
            }
        }
    }
    public void PointerDown()
    {
        FindObjectOfType<DragManager>().isClick = true;
        FindObjectOfType<DragManager>().isTxt = false;
        Debug.Log("�巡�� ����");
        FindObjectOfType<DragManager>().DragObjValue = 0;

        if (Minus == false)
        {
            FindObjectOfType<DragManager>().DragObjValue++;
            Minus = true;
            Debug.Log("�巡�� �� �߰�");
        }
        //FindObjectOfType<DragManager>().line.SetPosition(0, gameObject.transform.position);
    }

    public void PointerUp()
    {
        FindObjectOfType<DragManager>().isClick = false;
        FindObjectOfType<DragManager>().isTxt = true;

        if (FindObjectOfType<DragManager>().DragObjValue == 1)
        {
            FindObjectOfType<DragManager>().DragObjValue = 0;
        }
        Debug.Log("�巡�� ���");
        //FindObjectOfType<DragManager>().line.SetPosition(1, gameObject.transform.position);
    }
}
