using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI volumeText;
    [SerializeField] AudioMixer mixer;
    private float saveVolume;
    int SaveQuality;
    public Slider slider;
    const string SVname = "savevolume", SQname = "savequality";
    
    void Start()
    {
        saveVolume = PlayerPrefs.GetFloat(SVname, 80);
        SaveQuality = PlayerPrefs.GetInt(SQname);
        mixer.SetFloat("Volume", saveVolume - 80);
        QualitySettings.SetQualityLevel(SaveQuality);
        volumeText.text = $"{saveVolume:0}%";
        slider.value = saveVolume;
    }
    public void SetVolume(float volume)
    {
        saveVolume = volume;
        volumeText.text = $"{volume:0}%";
        mixer.SetFloat("Volume", volume - 80);
        PlayerPrefs.SetFloat(SVname, saveVolume);
    }
    public void Quality(int qualityIdext)
    {
        SaveQuality = qualityIdext;
        PlayerPrefs.SetInt(SQname, SaveQuality);
        QualitySettings.SetQualityLevel(qualityIdext);
    }
}
