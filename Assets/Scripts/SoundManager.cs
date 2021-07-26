using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    BGM,
    EFFECT,
    END,
}

public class SoundManager : MonoBehaviour
{
    AudioSource[] audioSources = new AudioSource[(int)SoundType.END];
    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();

    public static SoundManager Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;

        GameObject sound = GameObject.Find("SoundManager");

        if (!sound)
        {
            name = "SoundManager";
            DontDestroyOnLoad(gameObject);

            string[] soundNames = System.Enum.GetNames(typeof(SoundType));

            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.SetParent(sound.transform);
            }

            audioSources[(int)SoundType.BGM].loop = true;
        }
    }


}
