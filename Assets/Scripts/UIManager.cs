using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text DragText;
    void Start()
    {
    }

    void Update()
    {
        DragText.text = "Drag Count : " + GameManager.instance.DragCount.ToString();
    }

    public void ResetButton()
    {
        SceneManager.LoadScene("Ingame");
    }
}
