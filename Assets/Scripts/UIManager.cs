using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text DragText;
    [SerializeField]
    GameObject ClearUI;

    RectTransform rect;

    float ClearUISize = 0.1f;
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        DragText.text = "Drag Count : " + GameManager.instance.DragCount.ToString();

        if (GameManager.instance.Clear == true)
        {
            ClearUI.SetActive(true);
            //if (ClearUISize < 1)
                //ClearUISize += 0.1f;
            //rect.localScale = new Vector3(ClearUISize, ClearUISize, 1);
        }
        else
        {
            ClearUI.SetActive(false);
        }
    }

    public void ResetButton()
    {
        SceneManager.LoadScene("Ingame");
    }
}
