using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerScript : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject mainMenuUI;
    public GameObject controlsUI;
    public GameObject creditsUI;
    public bool isMenu;
    private bool paused;
    private Scene scene;

    private void Awake()
    {
        Time.timeScale = 1;
        scene = SceneManager.GetActiveScene();

        if (isMenu)
        {
            Cursor.lockState = CursorLockMode.Confined;
            mainMenuUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMenu)
            return;

        if (!paused && Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 0;
            paused = true;
            pauseUI.SetActive(true);
        } else if (paused && Input.GetKeyDown(KeyCode.Escape)) {
            Time.timeScale = 1;
            paused = false;
            pauseUI.SetActive(false);
        }
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(scene.buildIndex + 1);
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(scene.buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowMenu()
    {
        mainMenuUI.SetActive(true);
        controlsUI.SetActive(false);
        creditsUI.SetActive(false);
    }

    public void ShowControls()
    {
        mainMenuUI.SetActive(false);
        controlsUI.SetActive(true);
        creditsUI.SetActive(false);
    }

    public void ShowCredits()
    {
        mainMenuUI.SetActive(false);
        controlsUI.SetActive(false);
        creditsUI.SetActive(true);
    }
}
