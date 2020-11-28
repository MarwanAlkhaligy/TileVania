using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMneu : MonoBehaviour
{
    [SerializeField] GameObject EscapeMenucanvas;
    private bool GameIsPaused = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(GameIsPaused){
                Resume();
            }else{
                Pause();
            }
            
        } 
        
    }
    private void Pause()
    {
        GameIsPaused = true;
        Time.timeScale = 0f;
        EscapeMenucanvas.SetActive(true);
        
    }
    public void Resume()
    {
        GameIsPaused = false;
        Time.timeScale = 1f;
        EscapeMenucanvas.SetActive(false);
        

    }
    public void MainMenu()
    {
        Time.timeScale = 1f;
        if(FindObjectOfType<GameSession>())
        {
            Destroy(FindObjectOfType<GameSession>().gameObject);
        }
        SceneManager.LoadScene("Main Menu");
    }
    public void QuitGame()
        {
            Application.Quit();
        }
    

}
