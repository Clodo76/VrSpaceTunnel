using UnityEngine;
using System.Collections;

public class RxCollCheck : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
	{
        CxRunner.Instance.GameOver();

        if (other.gameObject.name.StartsWith("Prefab"))
        {
            /*
            Material mat = other.gameObject.GetComponent<Renderer>().material;

            
            //mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            //mat.SetInt("_ZWrite", 0);
            //mat.DisableKeyword("_ALPHATEST_ON");
            //mat.DisableKeyword("_ALPHABLEND_ON");
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
            mat.color = new Color(20, 0, 0, 0.2f);
            */

            Renderer renderer = other.gameObject.GetComponent<Renderer>();
            /*
            Material[] mats = renderer.materials;
            mats[0] = CxRunner.Instance.materialHitted;
            renderer.materials = mats;            
            */
            //renderer.materials = CxRunner.Instance.m_materials2;
            renderer.material = CxRunner.Instance.materialHitted;
        }
    }
}
