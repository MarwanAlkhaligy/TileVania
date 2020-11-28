using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class KeyFrame {

    public Vector3 Velocity { get; set; }
    public Vector3 Position { get; set; }
    public float Scale { get; set; }

    public KeyFrame(Vector3 velocity, Vector3 position, float scale) {
        Velocity = velocity;
        Position = position;
        Scale = scale;
    }
}


public class TimeRewinder : MonoBehaviour
{
    
    [SerializeField] int listSize = 100;
    [SerializeField] int keyframe = 5;
    [SerializeField] float timeOfRewinding = 5f;
    [SerializeField] GameObject postProcessVolume;
    [SerializeField][Range(0f,3f)] float volumeOfRewinding = 1f;
    private AudioSource audioSource;

    Player player;

    private int reverseCounter = 0;
    private int frameCounter = 0;
    private bool isRewinding = false;

    private List<KeyFrame> playerInfo ;
    private Vector3 currentPos;
    private Vector3 previousPos;
    private Vector3 currentVelocity;
    private Vector3 previousVelocity;
    private float currentScale;
    private bool firstRun = false;
    //private Camera camera;
    GameSession gameSession;
    void Start()
    {
        player = FindObjectOfType<Player>();
        playerInfo = new List<KeyFrame>();
        audioSource = GetComponent<AudioSource>();
        //camera = Camera.main;
        gameSession = GameSession.gameSessionInstance;
    }
    void Update()
    {
        DoRewinding();
    }

    private void DoRewinding()
    {
      
          if(Input.GetButtonDown("Fire1") ){
             if(gameSession.GetTimeRewinderScore() > 0){
                gameSession.SubFromTimeRewinderScore();
                StartRewinding();

                
             }
          }else if(Input.GetButtonUp("Fire1")) {
                StopRewinding();
                firstRun = true;
                
         }
      
        
    }

   
    void FixedUpdate()
    {
        if (!isRewinding) {
            if (frameCounter < keyframe){
                frameCounter += 1;
            } else {
                frameCounter = 0;
                Record();
            }        
        } else {
            TimeRewinding();
        }
    }
    
    public void StartRewinding()
    {
        isRewinding = true;
        audioSource.Play();
        postProcessVolume.SetActive(true);
    }
    public void StopRewinding()
    {
         isRewinding = false;
         audioSource.Stop();
         postProcessVolume.SetActive(false);
    }

    private void Record(){
                 playerInfo.Add(new KeyFrame(player.GetComponent<Rigidbody2D>().velocity,
                                             player.transform.position,
                                             player.transform.localScale.x));
        
    }
    
    private void TimeRewinding()
    {
        
        if (playerInfo.Count > listSize) {
            playerInfo.RemoveAt(0);
        }
        if (reverseCounter > 0) {
            reverseCounter -= 1;
        } else {
            reverseCounter = keyframe;
            RestorePos();
        }
        if (firstRun) {
            firstRun = false;
            RestorePos();
        }
        float interplotion = (float)reverseCounter / (float)keyframe;
        player.transform.position = Vector3.Lerp(previousPos, currentPos, interplotion);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.Lerp(previousVelocity, currentVelocity, interplotion);
        player.transform.localScale = new Vector2(currentScale,1f);     
    }
    private void RestorePos(){
        int currentPosIndex = playerInfo.Count - 1;
        int previousPosIndex = playerInfo.Count - 2;
         if (previousPosIndex >= 0 ) {
            currentPos = playerInfo[currentPosIndex].Position;
            previousPos = playerInfo[previousPosIndex].Position;
            currentVelocity = playerInfo[currentPosIndex].Velocity;
            previousVelocity = playerInfo[previousPosIndex].Velocity;
            currentScale = playerInfo[currentPosIndex].Scale;
            playerInfo.RemoveAt(currentPosIndex);
        }else{
            StopRewinding();
            return;
        } 
    }
    public bool GetIsRewinding()
    {
        return isRewinding;
    }
    
    public void SetIsRewinding(bool rewinding)
    {
        isRewinding = rewinding;
    }
    public void ClearAll()
    {
         playerInfo.Clear();
    }
    private IEnumerator  TimeOfRewinding()
    {
        yield return new WaitForSeconds(timeOfRewinding);
        StopRewinding();
    }
}
