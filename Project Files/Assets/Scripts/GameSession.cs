using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameSession : MonoBehaviour
{
   [SerializeField] int playerLives = 3;
   [SerializeField] int score = 0;
   [SerializeField] int timeRewinderScore = 0;
   [SerializeField] Text liveText;
   [SerializeField] Text scoreText;
   [SerializeField] Text timeRewinderText;
   [SerializeField] float delayLoadTime = 1f;

   public static GameSession gameSessionInstance { get; set; }
   
   
   private void Awake() {
       int numberofGameSession = FindObjectsOfType<GameSession>().Length;

       if(numberofGameSession > 1){
           Destroy(gameObject);
      
       }else{
           gameSessionInstance = this;
           DontDestroyOnLoad(gameObject);
       }
   }
    void Start()
    {
        liveText.text = playerLives.ToString();
        scoreText.text = score.ToString();
        timeRewinderText.text = timeRewinderScore.ToString();
    }

    public void AddToScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }
    public void AddToTimeRewinderScore(int points)
    {
         timeRewinderScore += points;
         timeRewinderText.text =  timeRewinderScore.ToString();
    }
    public void SubFromTimeRewinderScore()
    {
         timeRewinderScore -= 1;
         timeRewinderText.text =  timeRewinderScore.ToString();
    }
    public int GetTimeRewinderScore(){ return timeRewinderScore; }


    public void ProcessPlayerDealth()
    {
        
        if(playerLives > 1)
        {
            TakeLive();
        }
        else{
            ResetGameSession();
        }
    }

    public void TakeLive()
    {
        playerLives--;
        var currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(DelayLoadTime());
        SceneManager.LoadScene(currentLevelIndex);
        liveText.text = playerLives.ToString();

    }
    
    private void ResetGameSession() {
        Destroy(gameObject);
        StartCoroutine(DelayLoadTime());
        SceneManager.LoadScene(0);
        

    }
    private IEnumerator DelayLoadTime()
    {
        yield  return new WaitForSeconds(delayLoadTime); 
    }
  
}
