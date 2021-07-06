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
        if (SceneManager.GetActiveScene().name == "Ingame")
        {
            DragText.text = "Drag Count : " + GameManager.instance.DragCount.ToString();

            IngameClear();
        }

        else if (SceneManager.GetActiveScene().name == "Title")
        {
        }
    }

    private void IngameClear()
    {
        if (GameManager.instance.Clear == true)
        {
            ClearUI.SetActive(true);
        }
        else
        {
            ClearUI.SetActive(false);
        }
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("ModeSelect");
    }

    public void OptionButton()
    {
    }

    public void RankingButton()
    {
    }

    public void TitleButton()
    {
        SceneManager.LoadScene("Title");
    }

    public void Stage()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void Class()
    {
        SceneManager.LoadScene("ClassSelect");
    }

    public void ReturnModeSelect()
    {
        SceneManager.LoadScene("ModeSelect");
    }

    public void ResetButton()
    {
        SceneManager.LoadScene("Ingame");
    }

    public void EX()
    {
        DonDestroy.instance.ModeSelect = 1;
        ResetButton();
    }
    public void Portal()
    {
        DonDestroy.instance.ModeSelect = 2;
        ResetButton();
    }
    public void Turn()
    {
        DonDestroy.instance.ModeSelect = 3;
        ResetButton();
    }
}
