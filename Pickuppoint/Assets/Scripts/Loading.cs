using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public int sceneId;
    public Image progress;

    private void OnEnable()
    {
        StartCoroutine(loadLevel());
    }

    IEnumerator loadLevel()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        progress.fillAmount = operation.progress;
        yield return null;
    }
}