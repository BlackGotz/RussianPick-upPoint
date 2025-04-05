using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Loading : MonoBehaviour
{
    public int sceneId;
    public Image progress;
    public TextMeshProUGUI progressText;

    private void OnEnable()
    {
        StartCoroutine(loadLevel());
    }

    IEnumerator loadLevel()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        progress.fillAmount = operation.progress;
        progressText.text = "Загрузка... "+ operation.progress.ToString();
        yield return null;
    }
}