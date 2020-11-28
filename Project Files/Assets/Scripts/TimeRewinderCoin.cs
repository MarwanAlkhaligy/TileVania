using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewinderCoin : MonoBehaviour
{
    [SerializeField] int coinScore = 100;
    [SerializeField] AudioClip pickCoin;
    [SerializeField][Range(0f,3f)] float volumeOfSound = 3f;
    bool isTriggered = false;
    private void OnTriggerEnter2D(Collider2D other) {
        AudioSource.PlayClipAtPoint(pickCoin, other.gameObject.transform.position,volumeOfSound);
        if(!isTriggered)
        {
            isTriggered = true;
            FindObjectOfType<GameSession>().AddToTimeRewinderScore(coinScore);
            Destroy(gameObject);

        }
        
    }
}
