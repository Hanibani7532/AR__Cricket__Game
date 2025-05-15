using UnityEngine;

public class BallBowlingScript : MonoBehaviour
{
    public Rigidbody rb;
    public float throwForce = 500f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ThrowBall()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * throwForce);
    }
}
