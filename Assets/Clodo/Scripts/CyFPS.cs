using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CyFPS : MonoBehaviour
{

	// Attach this to a GUIText to make a frames/second indicator.
	//
	// It calculates frames/second over each updateInterval,
	// so the display does not keep changing wildly.
	//
	// It is also fairly accurate at very low FPS counts (<10).
	// We do this not by simply counting frames per interval, but
	// by accumulating FPS for each frame. This way we end up with
	// correct overall FPS even if the interval renders something like
	// 5.5 frames.

	/*
	public float updateInterval = 0.5F;

	private float accum = 0; // FPS accumulated over the interval
	private int frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	*/

	// v2:
	public GameObject cameraReference;
	public float frequency = 0.5f;
	public bool show = false;
	public int FramesPerSec { get; protected set; }
	//public GUIStyle styleText;
	public UnityEngine.Canvas objectCanvas;
	public UnityEngine.UI.Text objectText;	

	private string text;

	void Start()
	{
		if (cameraReference != null)
		{
			objectCanvas.transform.SetParent(cameraReference.transform, false);
		}

		ShowChanged();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F11))
		{
			show = !show;
			ShowChanged();
		}
	}

	void ShowChanged()
	{
		if (show)
			StartCoroutine(FPS());
		objectCanvas.enabled = show;
		objectText.enabled = show;		
	}
	
	private IEnumerator FPS()
	{
		for (; ; )
		{
			// Capture frame-per-second
			int lastFrameCount = Time.frameCount;
			float lastTime = Time.realtimeSinceStartup;
			
			yield return new WaitForSeconds(frequency);
			//yield return StartCoroutine(CyUtils.WaitForRealSeconds(frequency));
 

			float timeSpan = Time.realtimeSinceStartup - lastTime;
			int frameCount = Time.frameCount - lastFrameCount;

			// Display it
			FramesPerSec = Mathf.RoundToInt(frameCount / timeSpan);
			//guiText.text = FramesPerSec.ToString() + " fps";
			text = FramesPerSec.ToString() + " fps";

			if (objectText.text != text)
			{
				objectText.text = text;
				if (FramesPerSec > 58)
					objectText.color = Color.green;
				else if (FramesPerSec > 30)
				{
					objectText.color = Color.yellow;
				}
				else
				{
					objectText.color = Color.red;
				}
			}

			if (show == false)
				break;
		}
	}
}