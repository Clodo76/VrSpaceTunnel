using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CxRunner : MonoBehaviour {

	public string status = "Welcome";

	public int lives = 5;

	public float lastDistance = 0;

	public static CxRunner Instance;
	public GameObject objectPlayer;
	public float speedFactor = 0.02f;
	public float speedMax = 5;
	public float distanceObstacleFactor = 0.1f;

	public OVRCameraRig ovrCameraRig;
	public GameObject objectRiftCenter;
	public GameObject objectRocks;
	public GameObject objectRecord;

    public GameObject objectPlayerStruct;
    public GameObject objectPlanes;
    //public GameObject objectHelp;
    //public GameObject objectWarning;

    public GameObject objectLife1;
    public GameObject objectLife2;
    public GameObject objectLife3;
    public GameObject objectLife4;
    public GameObject objectLife5;

    public Material materialHitted;
    //public GameObject objectMaterialHit;

    public UnityEngine.UI.Text txtDistanceGame;

	public UnityEngine.UI.Text txtDistance;
	public UnityEngine.UI.Text txtHighscore;
	public UnityEngine.UI.Text txtHelp;

	public GameObject objectRifTop;
	public GameObject objectRifBottom;
	public GameObject objectRifCurrent;
	public GameObject objectRifSuggest;

	public Text objectUiScore;

	//public CC_Grayscale effectGrayLeft;
	//public CC_Grayscale effectGrayRight;

	public float playerDistance = 0f;
	public float playerAccel = 0.001f;
	public AudioSource audioGameOver;
	public Vector3 headPunchVector = new Vector3(10, 0, 0);

	public GameObject preFab1;
	public GameObject preFab2;
	public GameObject preFab3;
	public GameObject preFab4;
	public GameObject preFab5;
	public GameObject preFab6;
	public GameObject preFab7;
	public GameObject preFab8;
	public GameObject preFab9;

	public List<GameObject> prefabs = new List<GameObject>();

	int cubesIndex = 0;
	public List<GameObject> cubes = new List<GameObject>();

	private Vector3 startRiftPosition;
    private int m_lives = 3;
    private float m_highScore;

    // Use this for initialization
    void Start () {

        Instance = this;

		SetQualityLevel(5);

		prefabs.Add(preFab1);
		prefabs.Add(preFab2);
		prefabs.Add(preFab3);
		prefabs.Add(preFab4);
		prefabs.Add(preFab5);
		prefabs.Add(preFab6);
		prefabs.Add(preFab7);
		prefabs.Add(preFab8);
		prefabs.Add(preFab9);

        /*
		// Build, temp
		for (int i = 0; i < 100; i++)
		{
			int c = UnityEngine.Random.Range(0, prefabs.Count);
			GameObject cloneObject = GameObject.Instantiate(prefabs[c]) as GameObject;
			cloneObject.transform.position = new Vector3(0, 0, 5 + i*5);
			cloneObject.transform.localScale = new Vector3(1, 1, 3);
			cloneObject.SetActive(true);
		}
		*/

        //objectAim1.SetActive(false);
        //objectAim2.SetActive(false);
        //objectWarning.SetActive(false);

        m_highScore = PlayerPrefs.GetFloat("highscore", 0);

        Welcome();
        
        //ResetOrientation();
    }

	// Update is called once per frame
	void Update()
	{
		if (cubes.Count < 20)
		{
			cubesIndex++;

			float size = cubesIndex * distanceObstacleFactor;
			
			//float distance = cubesIndex + cubesIndex * cubesIndex / distanceObstacleFactor;
			int c = UnityEngine.Random.Range(0, prefabs.Count);
			GameObject cloneObject = GameObject.Instantiate(prefabs[c]) as GameObject;
			cloneObject.transform.position = new Vector3(0, 0, cubesIndex * (3+size));
			cloneObject.transform.localScale = new Vector3(1, 1, (1.5f + size));
			cloneObject.SetActive(true);
			cloneObject.name = "cube_"+cubesIndex.ToString();
			cubes.Add(cloneObject);
			
		}

		if (cubes[0].transform.position.magnitude-objectPlayer.transform.position.magnitude < -50)
		{
			GameObject.DestroyImmediate(cubes[0]);
			cubes.RemoveAt(0);
		}
        
        lastDistance = objectPlayer.transform.position.z;
        if (lastDistance > m_highScore)
            m_highScore = lastDistance;

        if (status == "Welcome")
		{
			if (Input.GetButtonDown("Start"))
			{
				Play();
			}
		}

		if (status == "GameOver")
		{
			if (Input.GetButtonDown("Start"))
			{
				Welcome();
			}
		}
		
		if (Input.GetButtonDown("ResetOrientation"))
		{
			ResetOrientation();
		}

		if (status == "Play")
		{
			playerAccel += speedFactor * Time.deltaTime;

			/*
			if (playerAccel > speedMax)
				playerAccel = speedMax;
			*/

			playerDistance += playerAccel * Time.deltaTime;

			objectPlayer.transform.position += new Vector3(0, 0, playerDistance);            
        }
				
		if (status == "Welcome")
		{

            /*
			Vector3 aimPos = objectRiftCenter.transform.position;
			aimPos.z = 2.99f;
			objectAim1.transform.position = aimPos;
			 * */

            //Vector3 pos = objectRiftCenter.transform.localPosition + objectPlayer.transform.position;
            Vector3 pos = objectRiftCenter.transform.position;

            pos.z = 0.996f;
			pos.x = 0;
			objectRifCurrent.transform.position = pos;

			if( (pos.y > objectRifTop.transform.position.y) || (objectRifTop.transform.position == Vector3.zero) )
			{
				pos.z = 0.999f;
				objectRifTop.transform.position = pos;
                Debug.Log("RT:" + objectRifTop.transform.position.ToString());
			}

			if( (pos.y < objectRifBottom.transform.position.y) || (objectRifBottom.transform.position == Vector3.zero) )
			{
				pos.z = 0.998f;
				objectRifBottom.transform.position = pos;
			}

			Vector3 posSuggest = objectRifCurrent.transform.position;
			posSuggest.z = 0.997f;
			posSuggest.x = 0;
			posSuggest.y = (objectRifTop.transform.position.y + objectRifBottom.transform.position.y)/2;
			objectRifSuggest.transform.position = posSuggest;
		}

		/*
		bool showWarning = false;
		Vector3 deltaPosition = objectRiftCenter.transform.position - startRiftPosition;
		if (Mathf.Abs(deltaPosition.x) > 0.15f)
			showWarning = true;
		if (Mathf.Abs(deltaPosition.y) > 0.15f)
			showWarning = true;
		objectWarning.SetActive(showWarning);
		*/

		
		// For mouse only
		Vector3 currentRotation = objectPlayer.transform.rotation.eulerAngles;
		currentRotation.y += Input.GetAxis("Mouse X") * 3;
		currentRotation.x -= Input.GetAxis("Mouse Y") * 3;
		objectPlayer.transform.rotation = Quaternion.Euler(currentRotation);
		
		if (Input.GetButtonDown("Restart"))
			Application.LoadLevel(Application.loadedLevel);

		
		if (Input.GetKeyDown(KeyCode.Z))
		{
			//if (Input.GetKey(KeyCode.LeftShift))
			{
                m_highScore = 0;
				PlayerPrefs.SetFloat("highscore", 0);
				UpdateStatsAll();
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha0))
			SetQualityLevel(0);
		if (Input.GetKeyDown(KeyCode.Alpha1))
			SetQualityLevel(1);
		if (Input.GetKeyDown(KeyCode.Alpha2))
			SetQualityLevel(2);
		if (Input.GetKeyDown(KeyCode.Alpha3))
			SetQualityLevel(3);
		if (Input.GetKeyDown(KeyCode.Alpha4))
			SetQualityLevel(4);
		if (Input.GetKeyDown(KeyCode.Alpha5))
			SetQualityLevel(5);
		else if(Input.GetKeyDown(KeyCode.Comma))
			SetQualityLevel(QualitySettings.GetQualityLevel()-1);
		else if (Input.GetKeyDown(KeyCode.Period))
			SetQualityLevel(QualitySettings.GetQualityLevel() + 1);

		ovrCameraRig.transform.localPosition = new Vector3 (Input.GetAxis("Horizontal")/4, Input.GetAxis("Vertical")/4, ovrCameraRig.transform.localPosition.z);



        objectPlayerStruct.transform.position = new Vector3(0, 0, objectPlayer.transform.position.z);

        objectPlanes.transform.position = new Vector3(0, 0, Mathf.Round(objectPlayer.transform.position.z / 50) * 50);
    }

	public string DistanceToDesc(float d)
	{
		//string x = d.ToString("0.00") + " m";
		string x = d.ToString("0") + " m";
		return x;
	}

	public void Welcome()
	{
		status = "Welcome";

		UnityEngine.Random.seed = UnityEngine.Random.Range(0,1000);

		cubesIndex = 0;

		for (; cubes.Count > 0; )
		{
			GameObject.DestroyImmediate(cubes[0]);
			cubes.RemoveAt(0);
		}		
		playerAccel = 0.001f;
		playerDistance = 0;
		objectPlayer.transform.position = new Vector3(0f,0.5f,0f);

		m_lives = lives;
        lastDistance = 0;

		//effectGrayLeft.enabled = false;
		//effectGrayRight.enabled = false;

		//objectHelp.SetActive(true);
		//objectAim1.SetActive(true);
		//objectAim2.SetActive(true);
		objectRifSuggest.SetActive(true);
		objectRifCurrent.SetActive(true);
		objectRifTop.SetActive(true);
		objectRifBottom.SetActive(true);
				
		UpdateStatsAll();
	}

	public void Play()
	{
		GetComponent<AudioSource>().Play();

		status = "Play";

		//objectUiScore.enabled = true;
		//objectHelp.SetActive(false);
		//objectAim1.SetActive(false);
		//objectAim2.SetActive(false);
		objectRifSuggest.SetActive(false);
		objectRifCurrent.SetActive(false);
		objectRifTop.SetActive(false);
		objectRifBottom.SetActive(false);

		ResetOrientation();

        StartCoroutine(PlayRoutine());
    }

	public void UpdateStats()
	{
        txtDistance.text = DistanceToDesc(lastDistance);
        txtHighscore.text = DistanceToDesc(m_highScore);
        //txtDistanceGame.text = DistanceToDesc(objectPlayer.transform.position.z) + " - " + m_lives.ToString() + " left";
	}

	public void UpdateStatsAll()
	{
		UpdateStats();

		txtDistance.text = DistanceToDesc(lastDistance);
        txtHighscore.text = DistanceToDesc(m_highScore);

        objectRecord.transform.position = new Vector3 (1.5f, 0.5f, m_highScore);

		

		string helpMessage = "Stay relaxed and reset (R) the Rift.\nStill looking forward, move head up and down,\ncenter the yellow line over the green line,\nstart (SPACE), avoid obstacle by moving your\nhead slightly for better gameplay. You have 5 lives.\n\nF11 show the FPS. Must always be constant.\nX reset the entire game.\n0..5 set the quality, current: " + QualitySettings.GetQualityLevel().ToString() + "\nZ reset the highscore.\n\nEnjoy! - VrSpaceTunnel v1.2";

		txtHelp.text = helpMessage;

        UpdateLives();
	}

	private IEnumerator PlayRoutine()
	{
		for (; status == "Play"; )
		{
			UpdateStats();

			yield return new WaitForSeconds(0.1f);
		}
	}

    public void UpdateLives()
    {
        objectLife1.SetActive(m_lives >= 1);
        objectLife2.SetActive(m_lives >= 2);
        objectLife3.SetActive(m_lives >= 3);
        objectLife4.SetActive(m_lives >= 4);
        objectLife5.SetActive(m_lives >= 5);
    }

	public void GameOver()
	{
		if (status == "Play")
		{
			m_lives--;
			if (m_lives == 0)
			{
				GetComponent<AudioSource>().Stop();

				status = "GameOver";
				//effectGrayLeft.enabled = true;
				//effectGrayRight.enabled = true;
				//effectGrayLeft.amount = 1;
				//effectGrayRight.amount = 1;
                
				PlayerPrefs.SetFloat("highscore", m_highScore);
			    UpdateStatsAll();				
			}
			iTween.PunchRotation(ovrCameraRig.gameObject, headPunchVector, .5f);

            audioGameOver.Play();
            //AudioSource.PlayClipAtPoint(audioGameOver, objectPlayer.transform.position);            

            UpdateLives();
        }
	}

	public void ResetOrientation()
	{
		OVRManager.display.RecenterPose();
		startRiftPosition = objectRiftCenter.transform.position;

		objectRifTop.transform.position = Vector3.zero;
		objectRifBottom.transform.position = Vector3.zero;

        Debug.Log("ResetOrientation");
	}

	public void SetQualityLevel(int quality)
	{
		QualitySettings.SetQualityLevel(quality);

		objectRocks.SetActive(quality >=3);

		UpdateStatsAll();
	}
}
