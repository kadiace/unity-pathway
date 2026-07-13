using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro; // MIGRATED: New Input System namespace

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text RankingText;
    public GameObject GameOverText;
    public TMP_InputField BestScoreInput;
    public GameObject BestScore;

    private bool m_Started = false;
    private bool m_newScore = false;
    private int m_Points;

    private bool m_GameOver = false;

    // MIGRATED: InputAction replaces Input.GetKeyDown(KeyCode.Space)
    private InputAction m_LaunchAction;

    // MIGRATED: bind the Space key as a button action
    void Awake()
    {
        m_LaunchAction = new InputAction("Launch", InputActionType.Button, "<Keyboard>/space");
    }

    // MIGRATED: enable the action while the component is active
    void OnEnable()
    {
        m_LaunchAction.Enable();
    }

    // MIGRATED: disable the action when the component is inactive
    void OnDisable()
    {
        m_LaunchAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        SaveData data = GetBestScoreData();
        data ??= new()
        {
            name = "None",
            point = 0
        };
        SetRankingText(data.name, data.point);
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (m_newScore)
            {
                if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
                {
                    SaveBestScoreData(BestScoreInput.text, m_Points);

                    SaveData data = GetBestScoreData();
                    if (data == null)
                        return;

                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
            else
            {
                if (m_LaunchAction.WasPressedThisFrame()) // MIGRATED: was Input.GetKeyDown(KeyCode.Space)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        SaveData data = GetBestScoreData();
        if (data == null)
            return;
        m_newScore = data != null && data.point < m_Points;
        if (m_newScore)
            BestScore.SetActive(true);
        else
            GameOverText.SetActive(true);
    }

    private SaveData GetBestScoreData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (!File.Exists(path))
        {
            return null;
        }
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    private void SaveBestScoreData(string name, int point)
    {
        SaveData data = new()
        {
            name = name,
            point = point
        };
        string newJson = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", newJson);
    }

    private void SetRankingText(string name, int point)
    {
        RankingText.text = $"Best Score : {name} : {point}";
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int point;
    }
}
