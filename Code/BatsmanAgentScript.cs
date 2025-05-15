using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BatsmanAgentScript : MonoBehaviour
{
    public string apiUrl = "http://127.0.0.1:5000/predict_batsman_shot"; // Flask batsman model API
    public Rigidbody batRigidbody;
    public float moveSpeed = 5f;

    void Start()
    {
        if (GameController.currentMode != "Batting")
        {
            Debug.Log("Not in Batting mode, skipping prediction.");
            return;
        }

        float speed = 120f;
        float length = 6f;
        float line = 2f;
        float angle = 30f;

        StartCoroutine(CallBatsmanModel(speed, length, line, angle));
    }

    IEnumerator CallBatsmanModel(float speed, float length, float line, float angle)
    {
        string jsonData = JsonUtility.ToJson(new InputData
        {
            speed = speed,
            length = length,
            line = line,
            angle = angle
        });

        UnityWebRequest www = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        // Updated error checking for all Unity versions
        #if UNITY_2020_1_OR_NEWER
        if (www.result != UnityWebRequest.Result.Success)
        #else
        if (www.isNetworkError || www.isHttpError)
        #endif
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            string response = www.downloadHandler.text;
            ShotPrediction prediction = JsonUtility.FromJson<ShotPrediction>(response);
            PerformShot(prediction.prediction);
        }
    }

    void PerformShot(string shot)
    {
        Debug.Log("Predicted Shot: " + shot);

        if (shot == "straight_drive")
        {
            batRigidbody.AddForce(Vector3.forward * moveSpeed, ForceMode.Impulse);
        }
        else if (shot == "pull_shot")
        {
            batRigidbody.AddForce(Vector3.left * moveSpeed, ForceMode.Impulse);
        }
        else if (shot == "cover_drive")
        {
            batRigidbody.AddForce((Vector3.forward + Vector3.left) * moveSpeed, ForceMode.Impulse);
        }
        // Add more shots as needed
    }

    [System.Serializable]
    public class InputData
    {
        public float speed;
        public float length;
        public float line;
        public float angle;
    }

    [System.Serializable]
    public class ShotPrediction
    {
        public string prediction;
    }
}