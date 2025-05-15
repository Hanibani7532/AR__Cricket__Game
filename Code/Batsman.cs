using UnityEngine;

public class Batsman : MonoBehaviour
{
    public Animator anim; // Optional agar animation use karo
    public GameObject bat;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Debug.Log("Ball Hit!");
            // Optional: play animation
            // anim.SetTrigger("Hit");
        }
    }
}
