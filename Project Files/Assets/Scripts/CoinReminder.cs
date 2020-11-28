using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinReminder : MonoBehaviour
{
     int startSceneIndex;
    private void Awake() {
        int numOfScenePersist = FindObjectsOfType<CoinReminder>().Length;

        if(numOfScenePersist < 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
   
        startSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if(currentSceneIndex != startSceneIndex)
        {
            Destroy(gameObject);
        }
        
    }
}
