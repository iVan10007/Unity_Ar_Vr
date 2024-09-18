using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(requiredComponent:typeof(ARRaycastManager), requiredComponent2: typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{
	public GameObject prefab;
	private  ARRaycastManager raycastManager;
	private ARPlaneManager planeManager;
	private List<ARRaycastHit> hits = new List<ARRaycastHit>();

	private void Awake()
	{
		raycastManager = GetComponent<ARRaycastManager>();
		planeManager = GetComponent<ARPlaneManager>();
	}

	private void OnEnable()
	{
		EnhancedTouch.TouchSimulation.Enable();
		EnhancedTouch.EnhancedTouchSupport.Enable();
		EnhancedTouch.Touch.onFingerDown += FingerDown;
	}

	private void OnDisable()
	{
		EnhancedTouch.TouchSimulation.Disable();
		EnhancedTouch.EnhancedTouchSupport.Disable();
		EnhancedTouch.Touch.onFingerDown -= FingerDown;
	}

	private void FingerDown(EnhancedTouch.Finger finger)
	{
        if (finger.index != 0)
        {
			return;
        }
		if(raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
		{
			foreach (ARRaycastHit hit in hits)
			{
				Pose pose = hit.pose;
				GameObject obj = Instantiate(prefab, pose.position, pose.rotation);
			}
		}
    }
}
