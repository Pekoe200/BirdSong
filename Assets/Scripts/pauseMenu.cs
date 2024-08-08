using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseWindow;

    public void Pause(){
        pauseWindow.SetActive(true);
        Time.timeScale = 0;
    }

    public void Resume(){
        pauseWindow.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart(){
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Quit(){
        //close the app.
    }
}
