using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainWindow;
    [SerializeField] GameObject optionsWindow;
    [SerializeField] GameObject howToWindow;
    [SerializeField] GameObject vfxLeft;
    [SerializeField] GameObject vfxRight;
    
    public GameObject hoverPanel;

    public void PlayGame(){
        StartCoroutine(WaitCoroutine());
    }
    public void ShowMain(){
        optionsWindow.SetActive(false);
        howToWindow.SetActive(false);
        mainWindow.SetActive(true);
    }

    public void ShowOptions(){
        mainWindow.SetActive(false);
        optionsWindow.SetActive(true);
        
    }

    public void ShowHowto(){
        mainWindow.SetActive(false);
        optionsWindow.SetActive(false);
        howToWindow.SetActive(true);
    }

    public void Quit(){
        // Use for actual build
        //Application.Quit();

        // Use for testing in editor
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void TriggerVFX(){
        vfxLeft.SetActive(true);
        vfxRight.SetActive(true);
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadSceneAsync(1);
    }
}
