using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
public class Player : MonoBehaviour
{

    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed= 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(25f, 25f);
    [SerializeField] float animationReversingSpeed = 0.5f;
    [SerializeField] float delayDeathTime = 2f;
    [SerializeField][Range(0f,3f)] float volumeOfDeath = 1f;
    [SerializeField][Range(0f,3f)] float volumeOfJump = 1f;
    [SerializeField] AudioClip deathAudio;
    [SerializeField] AudioClip jumpAudio;
    
 

    private float startGravity;
    private bool isAlive = true;
    private bool isDied = false;

    TimeRewinder timeRewinder;

    Rigidbody2D myRigidbody; 
    Animator animator;
    CapsuleCollider2D collider;
    BoxCollider2D myFeet;


    void Start()
    {
        timeRewinder = FindObjectOfType<TimeRewinder>();
        myRigidbody = GetComponent<Rigidbody2D>();
        startGravity = myRigidbody.gravityScale;
        animator = GetComponent<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        myFeet = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
         PlayerRewinding();
         if(!isAlive || timeRewinder.GetIsRewinding()){return;}
         Run();
         jump();
         ClimbLadder();
         FlipSprite();
         PlayerDealth();
    }

    private void PlayerRewinding()
    {
    
            if(timeRewinder.GetIsRewinding()){
                if(!isAlive){
                    isAlive = true;
                    animator.SetTrigger("returnFromDealth");
                }  
            }
            animator.SetBool("Rewinding",timeRewinder.GetIsRewinding());
        
        
    }
    

    void Run()
    {
         float controlflow = CrossPlatformInputManager.GetAxis("Horizontal");
         Vector2 playerVelocity = new Vector2(controlflow * runSpeed,myRigidbody.velocity.y);
         myRigidbody.velocity = playerVelocity;
         bool isMoving = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
         animator.SetBool("Running",isMoving);
       
    }

    private void ClimbLadder()
    {
        if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Climb"))){
            myRigidbody.gravityScale = startGravity;
            animator.SetBool("Climbing", false);
            return;
        }
        float controlThrow = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, controlThrow * climbSpeed );
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("Climbing",playerHasVerticalSpeed);

    }
   

    private void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }
    private void jump()
    {
       if(!myFeet.IsTouchingLayers(LayerMask.GetMask("Ground"))){return;}
       if(Input.GetButtonDown("Jump")){
           AudioSource.PlayClipAtPoint(jumpAudio,transform.position, volumeOfDeath);
           Vector2 jumpToAdd = new Vector2(0f,jumpSpeed);
           myRigidbody.velocity += jumpToAdd; 
        }

    }
    private void PlayerDealth()
    {
        if(collider.IsTouchingLayers(LayerMask.GetMask("Enemy","Hazards"))){
            StartCoroutine(Die());
        }
    }
    
    private IEnumerator Die()
    {
           isAlive = false;
           animator.SetTrigger("Die");
           AudioSource.PlayClipAtPoint(deathAudio,transform.position, volumeOfDeath);
           yield return new WaitForSeconds(delayDeathTime);
           if (!isAlive){
              timeRewinder.ClearAll();
              FindObjectOfType<GameSession>().ProcessPlayerDealth(); 
          }   
        }
    
   
    
       
    }
   
    



   
