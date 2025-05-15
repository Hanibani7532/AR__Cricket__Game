using UnityEngine;

public class BallController : MonoBehaviour
{
    public float speed = 10f;  // Ball ki speed
    public Rigidbody rb; // Rigidbody component

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody ko get karna
    }

    public void ThrowBall(Vector3 direction)
    {
        rb.velocity = direction * speed;  // Ball ko direction aur speed ke saath throw karna
    }
}
