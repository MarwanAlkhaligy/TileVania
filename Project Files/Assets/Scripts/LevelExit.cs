using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float waitTime = 2f;
    [SerializeField] float levelExitSlowMoFactor  = 0.2f;
     private string sceneName;
    private void OnTriggerEnter2D(Collider2D other) {
         StartCoroutine(LoadNextLevel());
    }
    
    IEnumerator LoadNextLevel()
    {
        Time.timeScale = levelExitSlowMoFactor;
        yield return new WaitForSeconds(waitTime);
        Time.timeScale = 1f;
        Destroy(FindObjectOfType<ScenePersist>().gameObject);
        sceneName =  SceneManager.GetActiveScene().name;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex + 1 == 4) { Destroy(FindObjectOfType<GameSession>().gameObject); }
        if (FindObjectOfType<TimeRewinder>()) { Destroy(FindObjectOfType<TimeRewinder>().gameObject);}
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
