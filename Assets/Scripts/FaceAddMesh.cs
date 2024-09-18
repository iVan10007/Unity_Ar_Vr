using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;

public class FaceAddMesh : MonoBehaviour
{
	[SerializeField] ARFace face = null;
	[FormerlySerializedAs("indices")][SerializeField] int[] mousePointsIndices = new[] { 1, 14, 78, 292 };
	[SerializeField] float pointerScale = 0.01f;
	[SerializeField] GameObject optionlPointerPrefab = null;

	readonly Dictionary<int, Transform> pointers = new Dictionary<int, Transform>();

	private void Awake()
	{
		face.updated += delegate
		{
			for (var i = 0; i < mousePointsIndices.Length; i++)
			{
				var vertexIndex = mousePointsIndices[i];
				var pointer = getPointer(i);
				pointer = face.transform.TransformPoint(face.vertices[vertexIndex]);
			}
		};
	}

	private object getPointer(int id)
	{
		if(pointers.TryGetValue(id, out var existing))
		{
			return existing;
		}
		else
		{
			var newPointer = createNewPointer();
			pointers[id] = newPointer;
			return newPointer;
		}
	}

	private Transform createNewPointer()
	{
		var result = instantiatePointer();
		return result;
	}

	private Transform instantiatePointer()
	{
		if(optionlPointerPrefab != null)
		{
			return Instantiate(optionlPointerPrefab).transform;
		}
		else
		{
			var result = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
			result.localScale = Vector3.one * pointerScale;
			return result;
		}
	}
}
