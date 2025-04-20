using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer; // Перетаскиваем MainMixer
    public Slider musicSlider;
    public Slider sfxSlider;

    [SerializeField] private TMP_Text musicVolumeText;
    [SerializeField] private TMP_Text sfxVolumeText;

    // Временные переменные для хранения текущих (не сохранённых) значений
    private float _tempMusicVolume;
    private float _tempSFXVolume;

    // Последние сохранённые значения (для отката)
    private float _lastSavedMusicVolume;
    private float _lastSavedSFXVolume;

    private void Start()
    {
        // Загружаем сохранённые настройки
        _lastSavedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        _lastSavedSFXVolume = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        // Устанавливаем слайдеры и временные значения
        musicSlider.value = _lastSavedMusicVolume;
        sfxSlider.value = _lastSavedSFXVolume;

        // Обновление текста
        UpdateVolumeText(musicVolumeText, _lastSavedMusicVolume);
        UpdateVolumeText(sfxVolumeText, _lastSavedSFXVolume);

        // Применение громкости
        ApplyVolume("MusicVolume", _lastSavedMusicVolume);
        ApplyVolume("SFXVolume", _lastSavedSFXVolume);
    }

    // Вызывается при изменении слайдера музыки
    public void OnMusicSliderChanged(float value)
    {
        ApplyVolume("MusicVolume", value);
        UpdateVolumeText(musicVolumeText, value);
    }

    // Вызывается при изменении слайдера SFX
    public void OnSFXSliderChanged(float value)
    {
        ApplyVolume("SFXVolume", value);
        UpdateVolumeText(sfxVolumeText, value);
    }

    private void ApplyVolume(string mixerParam, float value)
    {
        if (value <= 0.001f) // Проверка на ~0
        {
            audioMixer.SetFloat(mixerParam, -80f); // Минимальная громкость в микшере
        }
        else
        {
            audioMixer.SetFloat(mixerParam, Mathf.Log10(value) * 20);
        }
    }

    private void UpdateVolumeText(TMP_Text textElement, float value)
    {
        if (textElement != null)
        {
            int percent = Mathf.RoundToInt(value * 100);
            textElement.text = $"{percent}%";
        }
    }

    // Сохраняет настройки (вызывается при нажатии "Сохранить")
    public void SaveSettings()
    {
        _lastSavedMusicVolume = musicSlider.value;
        _lastSavedSFXVolume = sfxSlider.value;

        PlayerPrefs.SetFloat("MusicVolume", _lastSavedMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", _lastSavedSFXVolume);
        PlayerPrefs.Save();

        Debug.Log("Настройки сохранены!");
    }

    // Откатывает настройки (вызывается при нажатии "Назад")
    public void RevertSettings()
    {
        musicSlider.value = _lastSavedMusicVolume;
        sfxSlider.value = _lastSavedSFXVolume;

        OnMusicSliderChanged(_lastSavedMusicVolume);
        OnSFXSliderChanged(_lastSavedSFXVolume);
    }
}