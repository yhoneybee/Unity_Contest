using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveObject : MonoBehaviour
{
    public static SaveObject instance;

    public int Score;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadData();
    }

    public void SaveData()
    {
        SaveData save = new SaveData();

        if (Score < GameManager.instance.Score)
            Score = GameManager.instance.Score;

        save.Score = Score;

        SaveManager.Save(save);
        Debug.Log($"[save]{save}");
    }

    public void LoadData()
    {
        SaveData save = SaveManager.Load();
        if(save==null)
        {
            Score = 0;
            SaveData save2 = new SaveData();
            save2.Score = Score;
            SaveManager.Save(save2);
            Debug.Log($"[save]{save}");
            return;
        }
        Score = save.Score;

        Debug.Log($"{save}");
    }
}
