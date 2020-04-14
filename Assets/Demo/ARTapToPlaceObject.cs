using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;

public class ARTapToPlaceObject : MonoBehaviour
{
    //public GameObject objectToPlace;
    public GameObject placementIndicator;

    //private ARSessionOrigin arOrigin;
    private ARRaycastManager aRRaycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;

    void Start()
    {
        Debug.Log("@@@@@@@@@@@@@@@@ START @@@@@@@@@@@@@@@@@");
        //arOrigin = FindObjectOfType<ARSessionOrigin>();
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        Debug.Log("RayCastManager = " + aRRaycastManager.tag);
    }

    void Update()
    {
        Debug.Log("Update start");
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        //if (placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    PlaceObject();
        //}
    }

    //private void PlaceObject()
    //{
    //    Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    //}

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
            Debug.Log("placement indicator is valid");
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            Debug.Log("placement indicator is NOT valid");
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose()
    {
        if(Camera.current != null)
        {
            Debug.Log("Camera.current = " + Camera.current.ToString());
        } else
        {
            Debug.Log("Camera.current = null");
        }
        
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        Debug.Log("Screen center = " + screenCenter.ToString());

        var hits = new List<ARRaycastHit>();

        if (aRRaycastManager != null)
        {
            Debug.Log("aRRayCatManager is OK");
            aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
            //arOrigin.camer Raycast(screenCenter, hits, TrackableType.Planes);

            Debug.Log("Hits = " + hits.Count);

            placementPoseIsValid = hits.Count > 0;
            if (placementPoseIsValid)
            {
                placementPose = hits[0].pose;

                var cameraForward = Camera.current.transform.forward;
                var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
                placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            }
        } else
        {
            Debug.Log("aRRayCatManager is NULL");
        }
    }
}
