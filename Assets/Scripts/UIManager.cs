using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text DragText;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        DragText.text = "Drag Count : " + GameManager.instance.DragCount.ToString();
    }

    public void ResetButton()
    {
        SceneManager.LoadScene("Ingame");
    }
}
