using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.IO;

class Data
{
    public int Score;
}
public class SLManager : MonoBehaviour
{
    List<Data> data = new List<Data>();
    [SerializeField] Text text;

    public void Save()
    {
        string jdata = JsonConvert.SerializeObject(data);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        string format = System.Convert.ToBase64String(bytes);

        File.WriteAllText(Application.dataPath + "/ValueText.json", format);
    }

    public void Load()
    {
        string jdata = File.ReadAllText(Application.dataPath + "/ValueText.json");
        byte[] bytes = System.Convert.FromBase64String(jdata);
        string reform = System.Text.Encoding.UTF8.GetString(bytes);
        text.text = data[0].Score.ToString();

        data = JsonConvert.DeserializeObject<List<Data>>(reform);
    }
}
