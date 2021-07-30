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
    [SerializeField]
    GameObject OptionPannel;
    [SerializeField]
    GameObject MenuPannel;

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
        OptionPannel.SetActive(true);
    }   
    public void OptionButtonExit()
    {
        OptionPannel.SetActive(false);
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

    public void ModeSelect()
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

    public void OpenMenu()
    {
        MenuPannel.SetActive(true);
    }
    
    public void CloseMenu()
    {
        MenuPannel.SetActive(false);
    }

    public void Restart()
    {
        Algorithm.Instance.ReRoll();
        MenuPannel.SetActive(false);
    }
}
