using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour {
    public GameObject ballPrefab;
    private ARRaycastManager arRaycastManager;

    void Start() {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    void Update() {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            var touch = Input.GetTouch(0);
            var hits = new List<ARRaycastHit>();

            if (arRaycastManager.Raycast(touch.position, hits, 
                UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon)) {
                Instantiate(ballPrefab, hits[0].pose.position, Quaternion.identity);
            }
        }
    }
}