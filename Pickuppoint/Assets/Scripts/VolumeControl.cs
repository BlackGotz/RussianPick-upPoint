using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer; // Перетаскиваем MainMixer
    public Slider musicSlider;
    public Slider sfxSlider;

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

        _tempMusicVolume = _lastSavedMusicVolume;
        _tempSFXVolume = _lastSavedSFXVolume;

        // Применяем сохранённую громкость
        ApplyMusicVolume(_lastSavedMusicVolume);
        ApplySFXVolume(_lastSavedSFXVolume);
    }

    // Вызывается при изменении слайдера музыки
    public void OnMusicSliderChanged(float value)
    {
        _tempMusicVolume = value;
        ApplyMusicVolume(value); // Временное применение
    }

    // Вызывается при изменении слайдера SFX
    public void OnSFXSliderChanged(float value)
    {
        _tempSFXVolume = value;
        ApplySFXVolume(value); // Временное применение
    }

    // Сохраняет настройки (вызывается при нажатии "Сохранить")
    public void SaveSettings()
    {
        _lastSavedMusicVolume = _tempMusicVolume;
        _lastSavedSFXVolume = _tempSFXVolume;

        PlayerPrefs.SetFloat("MusicVolume", _lastSavedMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", _lastSavedSFXVolume);
        PlayerPrefs.Save();

        Debug.Log("Настройки сохранены!");
    }

    // Откатывает настройки (вызывается при нажатии "Назад")
    public void RevertSettings()
    {
        _tempMusicVolume = _lastSavedMusicVolume;
        _tempSFXVolume = _lastSavedSFXVolume;

        musicSlider.value = _lastSavedMusicVolume;
        sfxSlider.value = _lastSavedSFXVolume;

        ApplyMusicVolume(_lastSavedMusicVolume);
        ApplySFXVolume(_lastSavedSFXVolume);

        Debug.Log("Изменения отменены!");
    }

    // Применяет громкость музыки (логарифмическая шкала)
    private void ApplyMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
    }

    // Применяет громкость SFX (логарифмическая шкала)
    private void ApplySFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
}