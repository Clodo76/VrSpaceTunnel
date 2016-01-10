using UnityEngine;
using System.Collections;

public class RxTileAdjust : MonoBehaviour {

    public float factor = 2;
	// Use this for initialization
	void Start () {

        Vector3 s = new Vector3(1, 1, 1);
        s = new Vector3(transform.lossyScale.x, transform.lossyScale.y) * factor;
        
        GetComponent<Renderer>().material.SetTextureScale("_MainTex", s);
        //GetComponent<Renderer>().material.SetTextureScale("_MainText", s);
        GetComponent<Renderer>().material.SetTextureScale("_BumpMap", s);
        //GetComponent<Renderer>().material.SetTextureScale("_Cube", s);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
