using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    [SerializeField] GameObject vfxLeft;
    [SerializeField] GameObject vfxRight;
    public GameObject hoverPanel;

    public void PlayGame(){
        StartCoroutine(WaitCoroutine());
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
