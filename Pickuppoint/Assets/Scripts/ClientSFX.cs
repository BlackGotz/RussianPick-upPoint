using UnityEngine;

[System.Serializable]
public class ClientSFX : MonoBehaviour
{
    [Header("Звуки клиента")]
    public AudioClip happySound;
    public AudioClip angrySound;
    public AudioClip orderSound;

    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
            Debug.LogError("Нет AudioSource на SFXManager!");
    }

    public void PlaySound(ClientSoundType soundType)
    {
        AudioClip clip = soundType switch
        {
            ClientSoundType.Happy => happySound,
            ClientSoundType.Angry => angrySound,
            ClientSoundType.Order => orderSound,
            _ => null
        };

        if (clip != null)
            _audioSource.PlayOneShot(clip);
    }
}

public enum ClientSoundType { Happy, Angry, Order }