using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(requiredComponent: typeof(ARRaycastManager))]
public class ARPlanePrefab : MonoBehaviour
{
	public GameObject prefab;
	private ARRaycastManager raycastManager;
	private ARPlaneManager planeManager;
	private List<ARRaycastHit> hits = new List<ARRaycastHit>();
	private int placedPrefabCount;
	[SerializeField]
	private int maxPrefabSpawnCount = 0;
	private GameObject spawnedObject;
	public GameObject placeablePrefab;
	private List<GameObject> placedPrefabList = new List<GameObject>();

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

		if (raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
		{
			foreach (ARRaycastHit hit in hits)
			{
				Pose hitPose = hit.pose;
				if (placedPrefabCount < maxPrefabSpawnCount)
				{
					SpawnPrefab(hitPose);
				}
			}
		}
	}

	private void SpawnPrefab(Pose hitPose)
	{
		spawnedObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
		placedPrefabList.Add(spawnedObject);
		placedPrefabCount++;
	}

	public void SetPrefabType(GameObject prefabType)
	{
		placeablePrefab = prefabType;
	}
}
