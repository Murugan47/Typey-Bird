using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSlider : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Text volumeText;

    void Start()
    {
        volumeSlider.value = AudioListener.volume; // set slider to current volume
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
        volumeText.SetText("Volume:" + Mathf.FloorToInt(value * 100) + "%");

    }
}
