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
    Text ScoreText;
    [SerializeField]
    Text BestScoreText;
    [SerializeField]
    Text XText;
    [SerializeField]
    GameObject ClearUI;
    [SerializeField]
    GameObject OptionPannel;
    [SerializeField]
    GameObject RankingPannel;
    [SerializeField]
    GameObject MenuPannel;
    private bool isClear = false;

    RectTransform rect;

    float ClearUISize = 0.1f;
    void Start()
    {
        rect = GetComponent<RectTransform>();

        if (SceneManager.GetActiveScene().name == "Title")
        {
            SaveObject.instance.LoadData();
            BestScoreText.text = SaveObject.instance.Score.ToString();
            SoundManager.Instance.Play("TitleBGM", SoundType.BGM);
        }
        else
        {
            SoundManager.Instance.Play("IngameBGM", SoundType.BGM);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Ingame")
        {
            DragText.text = "Drag Count : " + GameManager.instance.DragCount.ToString();
            ScoreText.text = "Score : " + GameManager.instance.Score.ToString();

            IngameClear();
        }

        if(SceneManager.GetActiveScene().name=="ModeSelect")
        {
            XText.text = DonDestroy.instance.cell_size.x.ToString() + " X " + DonDestroy.instance.cell_size.y.ToString();
        }
    }

    private void IngameClear()
    {
        if (GameManager.instance.Clear == true)
        {
            if (isClear == false)
            {
                SoundManager.Instance.Play("ClearEffect");
                isClear = true;
            }
            ClearUI.SetActive(true);
            SaveObject.instance.SaveData();// 점수 저장
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
        SoundManager.Instance.Play("MenuClick");
        OptionPannel.SetActive(true);
    }
    public void OptionButtonExit()
    {
        SoundManager.Instance.Play("UIClick");
        OptionPannel.SetActive(false);
    }

    public void RankingButton()
    {
        SoundManager.Instance.Play("MenuClick");
        RankingPannel.SetActive(true);
    }

    public void RankingExitButton()
    {
        SoundManager.Instance.Play("UIClick");
        RankingPannel.SetActive(false);
    }

    public void TitleButton()
    {
        SoundManager.Instance.Play("UIClick");
        SceneManager.LoadScene("Title");
    }

    public void ModeSelect()
    {
        SoundManager.Instance.Play("StartEffect");
        SceneManager.LoadScene("ModeSelect");
    }

    public void ResetButton()
    {
        SoundManager.Instance.Play("UIClick");
        SceneManager.LoadScene("Ingame");
    }

    public void EX()
    {
        SoundManager.Instance.Play("MenuClick");
        DonDestroy.instance.ModeSelect = 1;
        ResetButton();
    }
    public void Portal()
    {
        SoundManager.Instance.Play("MenuClick");
        DonDestroy.instance.ModeSelect = 2;
        ResetButton();
    }
    public void Turn()
    {
        SoundManager.Instance.Play("MenuClick");
        DonDestroy.instance.ModeSelect = 3;
        ResetButton();
    }

    public void OpenMenu()
    {
        SoundManager.Instance.Play("MenuClick");
        MenuPannel.SetActive(true);
    }

    public void CloseMenu()
    {
        SoundManager.Instance.Play("UIClick");
        MenuPannel.SetActive(false);
    }

    public void Restart()
    {
        SoundManager.Instance.Play("UIClick");
        Algorithm.Instance.ReRoll();
        MenuPannel.SetActive(false);
    }

    public void Left()
    {
        if (DonDestroy.instance.cell_size == new Vector2Int(7, 7))
        {
            DonDestroy.instance.cell_size = new Vector2Int(5, 5);
        }
        else if (DonDestroy.instance.cell_size == new Vector2Int(10, 10))
        {
            DonDestroy.instance.cell_size = new Vector2Int(7, 7);
        }
    }

    public void Right()
    {
        if (DonDestroy.instance.cell_size == new Vector2Int(5, 5))
        {
            DonDestroy.instance.cell_size = new Vector2Int(7, 7);
        }
        else if (DonDestroy.instance.cell_size == new Vector2Int(7, 7))
        {
            DonDestroy.instance.cell_size = new Vector2Int(10, 10);
        }
    }
}
