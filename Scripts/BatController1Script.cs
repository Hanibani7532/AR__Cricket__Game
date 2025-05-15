using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatControllerScript : MonoBehaviour
{
    public static BatControllerScript instance;

    public GameObject ball; // the ball gameObject
    public float batSpeed; // the bat's speed
    public float batElevation; // the bat's elevation angle i.e. the bat's x rotation axis 
    public float boundaryPointX; // max x value the bat can cover

    public float batsmanReachLimitMin; // the ball can be hit once it is inside this limit
    public float batsmanReachLimitMax; // the ball cannot be hit once it gets outside this limit
    public Vector3 ballsPositionAtHit; // the balls position when it gets hit by the bat

    private bool isBatSwinged; // has the bat swinged
    private Vector3 defaultPosition; // bat's default beginning position

    public float BatSpeed { set { batSpeed = value; } }
    public bool IsBatSwinged { set { isBatSwinged = value; } get { return isBatSwinged; } }

    public float BatElevation
    {
        set
        {
            batElevation = value;
            transform.rotation = Quaternion.Euler(batElevation, transform.rotation.y, transform.rotation.z); // update the bats rotation to match the elevation
        }
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        defaultPosition = transform.position; // set defaultPosition to the bats beginning position
		Debug.Log("👀 BatControllerScript Start(), BallControllerScript.instance: " + (BallControllerScript.instance == null ? "NULL" : "SET"));

    }

    void Update()
    {
        // Null checks add kiye gaye hain
        if (BallControllerScript.instance != null && ball != null)
        {
            // Agar bat swing nahi hui, ball throw hui hai aur bat range ke andar hai
            if (!isBatSwinged && BallControllerScript.instance.IsBallThrown && ball.transform.position.z <= batsmanReachLimitMax)
            {
                transform.position = new Vector3(ball.transform.position.x,
                    transform.position.y,
                    transform.position.z);
            }
        }
        else
        {
            Debug.LogWarning("Ball ya BallControllerScript null hai!");
        }

        // Bat ki position ko clamp karna (pitch ke width ke andar)
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -boundaryPointX, boundaryPointX), transform.position.y, transform.position.z);

        // Agar bat swing hui ho aur ball ko hit kiya gaya ho, toh bat ki position ko update karte hain
        if (IsBatSwinged && BallControllerScript.instance != null && BallControllerScript.instance.IsBallHit)
        {
            transform.position = Vector3.MoveTowards(transform.position, ballsPositionAtHit, Time.deltaTime * 20);
        }
    }

    // Method to hit the ball
    public void HitTheBall(float dragAngle)
    {
        // if the ball is inside the bats hit range then hit the ball
        if (ball.transform.position.z >= batsmanReachLimitMin && ball.transform.position.z <= batsmanReachLimitMax)
        {
           // AudioManagerScript.instance.PlayBatHitAudio(); // play the bat hit sound
            ballsPositionAtHit = ball.transform.position; // set the ballsHitPosition to the balls position at the time of hit
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, dragAngle, transform.rotation.eulerAngles.z); // change rotation of the bat on the y axis to the swipe dragAngle

            // Call the HitBall function of the BallControllerScript and pass it the forward direction of 
            // the bat's transform and the bat's speed
            BallControllerScript.instance.HitTheBall(transform.forward, batSpeed);
        }
    }

    // Method to reset the bat
    public void ResetBat()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 0, 0);
        transform.position = defaultPosition;
        isBatSwinged = false;
    }

    // Add these methods at the end of your BatControllerScript

public void PlayStraightDrive()
{
    transform.localRotation = Quaternion.Euler(0, 0, 0);
}

public void PlayPullShot()
{
    transform.localRotation = Quaternion.Euler(0, -45, 0);
}

public void PlayCoverDrive()
{
    transform.localRotation = Quaternion.Euler(0, 45, 0);
}

}
