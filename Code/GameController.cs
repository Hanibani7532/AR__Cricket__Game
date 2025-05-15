using UnityEngine;

public class GameController : MonoBehaviour
{
    public static string currentMode = "Bowling"; // Default

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentMode = "Batting";
            Debug.Log("Mode Changed: Batting");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            currentMode = "Bowling";
            Debug.Log("Mode Changed: Bowling");
        }
    }
}
