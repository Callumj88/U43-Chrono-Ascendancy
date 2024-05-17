using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        // Once the scene is loaded, reset the game state

        GameManager.Instance.ResetGameState();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
