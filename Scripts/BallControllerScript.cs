using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallControllerScript : MonoBehaviour 
{
    public static BallControllerScript instance;

    public Vector3 defaultPosition;
    public GameObject marker;
    public float ballSpeed;
    public GameObject ball;
    public float bounceScalar;
    public float spinScalar;
    public float realWorldBallSpeed;
    public GameObject trajectoryHolder;
    public int ballType;

    private float angle;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private Vector3 direction;
    private Rigidbody rb;
    private float spinBy;

    private bool firstBounce;
    private bool isBallThrown;
    private bool isBallHit;
    private bool isTrajectoryEnabled;

    // Properties
    public float BallSpeed { set { ballSpeed = value; } }
    public int BallType { set { ballType = value; } }
    public bool IsBallThrown { get { return isBallThrown; } }
    public bool IsTrajectoryEnabled { set { isTrajectoryEnabled = value; } get { return isTrajectoryEnabled; } }
    public bool IsBallHit { get { return isBallHit; } }

    void Awake()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing from ball!");
            return;
        }

        if (marker == null)
        {
            Debug.LogError("Marker GameObject not assigned!");
        }

        if (trajectoryHolder == null)
        {
            Debug.LogError("TrajectoryHolder GameObject not assigned!");
        }
    }

    void Start()
    {
        defaultPosition = transform.position;
        startPosition = transform.position;
        rb.useGravity = false;
    }

    void Update()
    {
        if (isTrajectoryEnabled && rb != null && rb.velocity.magnitude > 0 && trajectoryHolder != null)
        {
            GameObject trajectoryBall = Instantiate(ball, transform.position, Quaternion.identity) as GameObject;
            trajectoryBall.transform.SetParent(trajectoryHolder.transform);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision == null || collision.gameObject == null) return;

        if (!isBallHit && collision.gameObject.CompareTag("Ground"))
        {
            switch(ballType)
            {
                case 0:
                    spinBy = direction.x;
                    break;
                case 1:
                    spinBy = spinScalar / ballSpeed;
                    break;
                case 2:
                    spinBy = -spinScalar / ballSpeed;
                    break;
            }

            if (!firstBounce)
            {
                firstBounce = true;
                rb.useGravity = true;
                direction = new Vector3(spinBy, -direction.y * (bounceScalar * ballSpeed), direction.z); 
                direction = Vector3.Normalize(direction);
                angle = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg;
                rb.AddForce(direction * ballSpeed, ForceMode.Impulse);

                if (CanvasManagerScript.instance != null)
                {
                    CanvasManagerScript.instance.UpdateBallsBounceAngleUI(angle);
                }
            }
        }

        if (collision.gameObject.CompareTag("Stump"))
        {
            Rigidbody stumpRb = collision.gameObject.GetComponent<Rigidbody>();
            if (stumpRb != null)
            {
                stumpRb.useGravity = true;
            }
            
            if (AudioManagerScript.instance != null)
            {
                AudioManagerScript.instance.PlayBatHitAudio();
            }
        }
    }

    public void ThrowBall()
    {
        if (!IsBallThrown && marker != null)
        {
            isBallThrown = true;
            if (CanvasManagerScript.instance != null)
            {
                CanvasManagerScript.instance.EnableBatSwipePanel();
            }
            targetPosition = marker.transform.position;
            direction = Vector3.Normalize(targetPosition - startPosition);
            rb.AddForce(direction * ballSpeed, ForceMode.Impulse);
        }
    }

    public void HitTheBall(Vector3 hitDirection, float batSpeed)
    {
        if (CameraControllerScript.instance != null)
        {
            CameraControllerScript.instance.IsBallHit = true;
        }
        isBallHit = true;
        rb.velocity = Vector3.zero;
        direction = Vector3.Normalize(hitDirection);
        float hitSpeed = (ballSpeed / 2) + batSpeed;
        rb.AddForce(-direction * hitSpeed, ForceMode.Impulse);
        
        if (!firstBounce)
        {
            rb.useGravity = true;
        }
    }

    public void SwitchSide()
    {
        transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
        defaultPosition = transform.position;
        startPosition = transform.position;
    }

    public void ResetBall()
    {
        firstBounce = false;
        isBallHit = false;
        isBallThrown = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        transform.position = defaultPosition;

        if (trajectoryHolder != null)
        {
            int childCount = trajectoryHolder.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                Destroy(trajectoryHolder.transform.GetChild(i).gameObject);
            }
        }
    }
}