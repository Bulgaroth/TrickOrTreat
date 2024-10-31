using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject optionMenu, mainMenu;

    private bool _isOptionOpened = false;

    public void PlayGame()
    {
        SceneTransitionManager.LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void RestartScene()
    {
        SceneTransitionManager.LoadSceneWithTransition(SceneManager.GetActiveScene().buildIndex);
    }

    public void OpenCredits()
    {
        SceneTransitionManager.LoadSceneWithTransition("Credits");
        //SceneManager.LoadScene("Credits");
    }

    public void GoMenu()
    {
        SceneTransitionManager.LoadSceneWithTransition(0);
        //SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenCloseOptions()
    {
        _isOptionOpened = !_isOptionOpened;
        optionMenu.SetActive(_isOptionOpened);
        mainMenu.SetActive(!_isOptionOpened);
    }
}