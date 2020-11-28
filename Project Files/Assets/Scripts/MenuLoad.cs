using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuLoad : MonoBehaviour
{
   public void StartFirstLevel()
   {
       SceneManager.LoadScene(1);
   }
   public void MainMenu()
   {
        if (FindObjectOfType<GameSession>()) {
           Destroy(FindObjectOfType<GameSession>().gameObject);
        } 
        SceneManager.LoadScene(0);
   }
   public void QuitGame()
   {
       Application.Quit();
   }
   
}
