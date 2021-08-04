using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioObject : MonoBehaviour
{
    public SoundType sound_type;
    public Slider slider;
    public Slider.SliderEvent slider_event;

    private void Awake()
    {
        slider.onValueChanged = slider_event;
        slider_event.AddListener((o) => { SoundManager.Instance.audioSources[(int)sound_type].volume = o; });
    }
}
