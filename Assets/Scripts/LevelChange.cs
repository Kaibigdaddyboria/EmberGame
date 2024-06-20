using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public int sceneNum;

    // Check if the object that entered the trigger is tagged as Player and loads the scene dpending on the scene number
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadSceneAsync(sceneNum);
        }
    }
    // A function to be referenced by a button
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    // Gets the sceneName so that SceneManagement
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
