using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BlockS : MonoBehaviour
{
    int BlockValue = 5;
    Image img;
    RectTransform rt;

    private void Awake()
    {
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    void Start()
    {
        GameManager.instance.Blocks.Add(gameObject);
    }

    void Update()
    {
        if(GameManager.instance.isClick==false)
        {
            img.color = new Color(1, 1, 1);
        }
    }

    public void PointerUp()
    {
        GameManager.instance.isClick = false;

        GameManager.instance.BlockPosition.Clear();
    }

    public void PointerDown()
    {
        if (BlockValue != 0)
        {
            GameManager.instance.isClick = true;
            img.color = new Color(0.9f, 0.9f, 0.9f);

            GameManager.instance.BlockPosition.Add(gameObject);
        }
    }

    public void Drag()
    {
        if (GameManager.instance.isClick == true)
        {
            Vector2 LastAPosition = GameManager.instance.BlockPosition.Last().GetComponent<RectTransform>().anchoredPosition;

            if (Mathf.Abs(LastAPosition.x - gameObject.GetComponent<RectTransform>().anchoredPosition.x) > 80
                || Mathf.Abs(LastAPosition.y - gameObject.GetComponent<RectTransform>().anchoredPosition.y) > 80)
            {
                return;
            }

            if (LastAPosition.x != gameObject.GetComponent<RectTransform>().anchoredPosition.x &&
                LastAPosition.y != gameObject.GetComponent<RectTransform>().anchoredPosition.y)
            {
                return;
            }

            foreach (var GameObj in GameManager.instance.BlockPosition)
            {
                if (GameObj == gameObject)
                {
                    GameManager.instance.SameBlock = true;
                    break;
                }
                else
                {
                    GameManager.instance.SameBlock = false;
                }
            }
            if (GameManager.instance.SameBlock == false)
            {
                GameManager.instance.BlockPosition.Add(gameObject);
                img.color = new Color(0.9f, 0.9f, 0.9f);
            }
        }
    }
}
