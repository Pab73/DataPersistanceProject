using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick _brickPrefab;
    public int _lineCount = 6;
    public Rigidbody _ball;

    public Text _scoreText;
    public Text _highScoreText;
    public GameObject _gameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_PointsHighScore;
    
    private bool m_GameOver = false;
    private string _namePlyr;
    private string _highScoreNamePlyr;

    private void Awake()
    {
        LoadHighScoreWithName();
    }

    // Start is called before the first frame update
    void Start()
    {
        HighScoreText(_highScoreText, _highScoreNamePlyr, m_PointsHighScore);
        _namePlyr = NameManager.Instance.PlyrName;
        m_Points = 0;
        ScoreText(_scoreText, _namePlyr, m_Points);
        ManageBlocks();        
    }

    private void Update()
    {        
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                _ball.transform.SetParent(null);
                _ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {            
            SetHighScore(m_Points,m_PointsHighScore, _namePlyr, _highScoreNamePlyr, _highScoreText);            
            SaveHighScoreWithName(_highScoreNamePlyr, m_PointsHighScore);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
    void ManageBlocks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < _lineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(_brickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }
    void HighScoreText(Text highScoreText, string highScoreNamePlyr, int highPointsScore)
    {
        highScoreText.text = $"HighScore :{highScoreNamePlyr} {highPointsScore}";
    }
    void ScoreText(Text scoreText, string namePlyr, int points)
    {
        scoreText.text = $"Score:{namePlyr} {points}";
    }
    void SetHighScore(int points, int highScorePoints, string namePlyr, string highScoreNamePlyr, Text highScoreText)
    {
        if (points >= highScorePoints)
        {
            m_PointsHighScore = points;
            _highScoreNamePlyr = namePlyr;
        }
        highScoreText.text = $"HighScore :{highScoreNamePlyr} {highScorePoints}";
    }

    void AddPoint(int point)
    {
        m_Points += point;
        _scoreText.text = $"Score:{_namePlyr} {m_Points}";
    }

    public void GameOver()
    {        
        m_GameOver = true;
        _gameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public string HighScoreplyrNameSaved;
        public int HighScorePointsSaved;
    }
    public void SaveHighScoreWithName(string highScoreNamePlyr, int highScorePoints)
    {
        SaveData data = new SaveData();
        data.HighScoreplyrNameSaved = highScoreNamePlyr;
        data.HighScorePointsSaved = highScorePoints;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }
    public void LoadHighScoreWithName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_PointsHighScore = data.HighScorePointsSaved;
            _highScoreNamePlyr = data.HighScoreplyrNameSaved;
        }
    }
}
