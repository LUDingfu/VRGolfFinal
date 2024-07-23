using UnityEngine;
using System.Collections;

public class HoleDetection : MonoBehaviour
{
    [SerializeField] private float timeToDrop = 0.1f;
    
    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("OnTriggerEnter called");
        GolfBallController golfBall = other.GetComponent<GolfBallController>();
        Debug.Log(golfBall.holeDetectionTrigger);
        if (golfBall != null && golfBall.holeDetectionTrigger)
        {
            Debug.Log("Ball Entered the Hole!");
            StartCoroutine(WaitForBallToStop(golfBall));
        }
    }

    private IEnumerator WaitForBallToStop(GolfBallController ball)
    {
        yield return new WaitForSeconds(timeToDrop);
        ball.Fall(true);
        Debug.Log("Ball has stopped. Now falling into the hole.");
    }
}