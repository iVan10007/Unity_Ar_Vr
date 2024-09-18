using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracking : MonoBehaviour
{
	public GameObject[] placeablePrefabs;

	private Dictionary<string, GameObject> spawnedPrefabs = new Dictionary<string, GameObject>();
	private ARTrackedImageManager imageManager;

	private void Awake()
	{
		imageManager = FindObjectOfType<ARTrackedImageManager>();

		foreach(GameObject prefab in placeablePrefabs)
		{
			GameObject newPrefab = Instantiate(prefab, Vector3.zero, Quaternion.identity);
			newPrefab.name = prefab.name;
			spawnedPrefabs.Add(prefab.name, newPrefab);
		}
	}

	private void OnEnable()
	{
		imageManager.trackedImagesChanged += ImageChanged;
	}

	private void OnDisable()
	{
		imageManager.trackedImagesChanged -= ImageChanged;
	}

	private void ImageChanged(ARTrackedImagesChangedEventArgs args)
	{
		foreach(ARTrackedImage trackedImage in args.added)
		{
			UpdateImage(trackedImage);
		}

		foreach (ARTrackedImage trackedImage in args.updated)
		{
			UpdateImage(trackedImage);
		}

		foreach (ARTrackedImage trackedImage in args.removed)
		{
			spawnedPrefabs[trackedImage.name].SetActive(false);
		}
	}

	private void UpdateImage(ARTrackedImage trackedImage)
	{
		string name = trackedImage.referenceImage.name;
		Vector3 position = trackedImage.transform.position;

		GameObject prefab = spawnedPrefabs[name];
		prefab.transform.position = position;
		prefab.SetActive(true);

		foreach(GameObject go in spawnedPrefabs.Values)
		{
			if(go.name != name)
			{
				go.SetActive(false);
			}
		}
	}
}
