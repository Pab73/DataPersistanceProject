using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    private int m_PointsHighScore;
    
    private bool m_GameOver = false;
    private string namePlyr;
    private string highScoreNamePlyr;

    private void Awake()
    {
        LoadHighScoreWithName();
    }

    // Start is called before the first frame update
    void Start()
    {
        HighScoreText.text = $"HighScore :{highScoreNamePlyr} {m_PointsHighScore}";
        namePlyr = NameManager.Instance.plyrName;
        m_Points = 0;
        ScoreText.text = $"Score:{namePlyr} {m_Points}";
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
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

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {            
            SetHighScore();
            HighScoreText.text = $"HighScore :{highScoreNamePlyr} {m_PointsHighScore}";
            SaveHighScoreWithName();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void SetHighScore()
    {
        if(m_Points >= m_PointsHighScore)
        {
            m_PointsHighScore = m_Points;
            highScoreNamePlyr = namePlyr;            
        }      
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score:{namePlyr} {m_Points}";
    }

    public void GameOver()
    {        
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public string HighScoreplyrNameSaved;
        public int HighScorePointsSaved;
    }
    public void SaveHighScoreWithName()
    {
        SaveData data = new SaveData();
        data.HighScoreplyrNameSaved = highScoreNamePlyr;
        data.HighScorePointsSaved = m_PointsHighScore;

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
            highScoreNamePlyr = data.HighScoreplyrNameSaved;
        }
    }
}
