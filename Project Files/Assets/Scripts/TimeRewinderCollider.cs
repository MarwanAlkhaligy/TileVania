using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewinderCollider : MonoBehaviour
{
    [SerializeField] float timeOfRewinding = 3f;
    TimeRewinder timeRewinder;
    bool isChecked = false;
    void Start()
    {
        timeRewinder = FindObjectOfType<TimeRewinder>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(!isChecked)
        {
             isChecked = true;
             timeRewinder.StartRewinding();
             StartCoroutine(TimeOfRewinding());
             
        }
       
    }
    private IEnumerator  TimeOfRewinding()
    {
        yield return new WaitForSeconds(timeOfRewinding);
        timeRewinder.StopRewinding();
    }
}
