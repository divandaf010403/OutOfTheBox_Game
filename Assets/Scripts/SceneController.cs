using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    //public string EscapeScene;
    //public bool isEscapeForQuit = false;

    public void Pause()
    {
        
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void LoadScene(string scene) {  
        SceneManager.LoadScene(scene); 
    }

    public void ExitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            pauseMenu.transform.position = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            Time.timeScale = 0f;
            Cursor.visible = true;
        }
    }
}