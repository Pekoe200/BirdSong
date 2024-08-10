using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTrigger : MonoBehaviour
{
    public sceneTransition sceneTransition;
    public string sceneName;
    public int sceneNo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //sceneTransition.FadeToScene(sceneName);
            SceneManager.LoadSceneAsync(sceneNo);
        }
    }

}
