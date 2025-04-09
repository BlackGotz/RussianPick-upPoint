using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameObject _loadWindow;
    // Start is called before the first frame update
    private void Start()
    {
        _loadWindow.SetActive(false);
    }
    public void ChangeScene()
    {
        _loadWindow.SetActive(true);
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Exit()
    {
        Application.Quit();
    }

}
