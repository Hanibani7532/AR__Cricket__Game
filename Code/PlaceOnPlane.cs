using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PlaceOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                if (spawnedObject == null)
                {
                    // Yahan hum thoda sa ground upar place kar rahe hain (5 cm)
                    spawnedObject = Instantiate(objectToPlace, hitPose.position + new Vector3(0, 0.05f, 0), hitPose.rotation);
                }
                else
                {
                    spawnedObject.transform.position = hitPose.position + new Vector3(0, 0.05f, 0);
                }
            }
        }
    }
}
