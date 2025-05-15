using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class HTTPRequestHandler : MonoBehaviour
{
    private string apiUrl = "http://127.0.0.1:5000/predict_batsman";

    void Start()
    {
        StartCoroutine(GetPrediction(50, 30, 15, 10));
    }

    IEnumerator GetPrediction(float speed, float angle, float length, float line)
    {
        string url = apiUrl + "?speed=" + speed + "&angle=" + angle + "&length=" + length + "&line=" + line;

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            // For Unity 2017 compatibility (pre-2020 versions)
            #if !UNITY_2020_1_OR_NEWER
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Prediction: " + www.downloadHandler.text);
            }
            #else
            // For Unity 2020.1 and newer
            if (www.result == UnityWebRequest.Result.ConnectionError || 
                www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                Debug.Log("Prediction: " + www.downloadHandler.text);
            }
            #endif
        }
    }
}