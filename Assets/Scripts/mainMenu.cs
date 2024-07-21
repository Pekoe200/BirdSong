using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject hoverPanel;

    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }
}
