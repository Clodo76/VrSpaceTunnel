using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RxTerrain : MonoBehaviour {

	public GameObject objectDecor;
	public GameObject objectPlayer;	
	public Vector3 rangeDistance;
	public float rangeDistanceMin;
	public float rangeScale;
	public int nMax = 300;
	
	private List<GameObject> objects = new List<GameObject>();
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		Check();
	}

	Vector3 RandomVector(float vMin, float vMax)
	{
		return new Vector3(
			UnityEngine.Random.Range(vMin,vMax),
			UnityEngine.Random.Range(vMin,vMax),
			UnityEngine.Random.Range(vMin,vMax)
			);
	}

	void Move(GameObject cloneObject)
	{
		float angle = UnityEngine.Random.Range(0, 360);
		float distance = UnityEngine.Random.Range(rangeDistanceMin, rangeDistance.x);
		float z = UnityEngine.Random.Range(-rangeDistance.z, rangeDistance.z);

		Vector3 position = new Vector3(
			Mathf.Sin(Mathf.Deg2Rad * angle) * distance,
			Mathf.Cos(Mathf.Deg2Rad * angle) * distance,
			z
			);

		if (objects.Count == nMax)
			position.z = objectPlayer.transform.position.z + rangeDistance.z;

		cloneObject.transform.position = position;
		cloneObject.transform.rotation = Quaternion.Euler(RandomVector(0, 360));

		float size = UnityEngine.Random.Range(1, rangeScale);
		cloneObject.transform.localScale = new Vector3(size, size, size);
	}

	void Check()
	{
		for (; ; )
		{
 			if (objects.Count < nMax)
			{
				GameObject cloneObject = GameObject.Instantiate(objectDecor) as GameObject;
				Move(cloneObject);
				cloneObject.transform.parent = transform;
				cloneObject.SetActive(true);

				objects.Add(cloneObject);				

				continue;
			}
			
			for (int c = 0; c < objects.Count; c++)
			{
				if (objectPlayer.transform.position.z - objects[c].transform.position.z > rangeDistance.z)
				{
					Move(objects[c]);
					//Debug.Log("Destroy");
					//GameObject.DestroyImmediate(objects[c]);
					//objects.RemoveAt(c);
					//break;
				}

				continue;
			}

			break;
		}
	}
}
