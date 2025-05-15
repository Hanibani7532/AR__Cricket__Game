using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BatsmanPrediction : MonoBehaviour
{
    private const string apiUrl = "http://localhost:5000/predict";

    public void CallPredictionAPI()
    {
        StartCoroutine(SendPredictionRequest());
    }

    private IEnumerator SendPredictionRequest()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl);
        yield return webRequest.SendWebRequest();

        // Updated error handling for all Unity versions
        #if UNITY_2020_1_OR_NEWER
        // For Unity 2020.1 and newer
        if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
            webRequest.result == UnityWebRequest.Result.ProtocolError)
        #else
        // For older Unity versions (pre-2020)
        if (webRequest.isNetworkError || webRequest.isHttpError)
        #endif
        {
            Debug.LogError("Error: " + webRequest.error);
        }
        else
        {
            Debug.Log("Response: " + webRequest.downloadHandler.text);
        }
    }
}