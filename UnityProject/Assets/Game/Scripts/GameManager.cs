using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static GameManager Instance
	{
		get;
		private set;
	}

	public static float totalTimePercentage = 1.0f;

	GameObject[] gameScenePrefabs = new GameObject[0];
	GameObject sceneInstance;
	SceneManager sceneManager;
	int playerLife = 3;

	void Awake()
	{
		Instance = this;
	}

	void OnDestroy()
	{
		Instance = null;
	}

	void Start()
	{
		totalTimePercentage = 1.0f;
		playerLife = 3;
		gameScenePrefabs = Resources.LoadAll<GameObject>("GameScenes");
		CreateRandomSceneInstance();
	}

	void CreateRandomSceneInstance()
	{
		if (sceneInstance != null)
		{
			sceneManager.SceneComplete -= HandleSceneComplete;
			sceneManager.SceneFailed -= HandleSceneFailed;
			Destroy(sceneInstance);
		}
		sceneInstance = null;
		sceneInstance = Instantiate(gameScenePrefabs[Random.Range(0, gameScenePrefabs.Length)], Vector3.zero, Quaternion.identity) as GameObject;
		sceneInstance.transform.parent = transform;
		sceneManager = sceneInstance.GetComponent<SceneManager>();
		sceneManager.SceneComplete += HandleSceneComplete;
		sceneManager.SceneFailed += HandleSceneFailed;
	}

	void HandleSceneFailed ()
	{
		playerLife -= 1;
		if (playerLife == 0)
		{
			UIManager.Instance.ShowResultScreen();
		}
		else
		{
			CreateRandomSceneInstance();
		}
	}

	void HandleSceneComplete ()
	{
		totalTimePercentage = Mathf.Clamp(totalTimePercentage *= 0.99f, 8, float.MaxValue);
		CreateRandomSceneInstance();
	}
}
