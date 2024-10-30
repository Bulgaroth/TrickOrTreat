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
        // Launch the first scene after the menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void GoMenu()
    {
        SceneManager.LoadScene(0);
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