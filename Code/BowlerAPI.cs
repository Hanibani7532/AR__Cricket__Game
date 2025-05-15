using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class BowlerAPI : MonoBehaviour
{
    // Flask API URL (Localhost pe chal raha hai toh 127.0.0.1 use karenge)
    private string apiUrl = "http://127.0.0.1:5000/predict"; 

    // Aapka model prediction lene ka function
    public void GetBowlerPrediction(string previousShot, string previousLength, string previousLine, float previousSpeed)
    {
        StartCoroutine(SendPredictionRequest(previousShot, previousLength, previousLine, previousSpeed));
    }

    // UnityWebRequest ke through Flask API ko call karte hain
   IEnumerator SendPredictionRequest(string previousShot, string previousLength, string previousLine, float previousSpeed)
{
    WWWForm form = new WWWForm();
    form.AddField("previous_shot", previousShot);
    form.AddField("previous_length", previousLength);
    form.AddField("previous_line", previousLine);
    form.AddField("previous_speed", previousSpeed.ToString());

    UnityWebRequest www = UnityWebRequest.Post(apiUrl, form);
    yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
    if (www.result == UnityWebRequest.Result.Success)
#else
    if (!www.isNetworkError && !www.isHttpError)
#endif
    {
        Debug.Log("Response: " + www.downloadHandler.text);
    }
    else
    {
        Debug.LogError("Request failed: " + www.error);
    }
}

}
