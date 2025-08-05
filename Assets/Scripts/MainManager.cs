using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    // Singleton instance
    public static MainManager Instance { get; private set; }

    // Game objects and prefabs
    public Brick BrickPrefab;
    public Rigidbody Ball;
    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;

    // Game settings
    public int LineCount = 6;

    // Game state
    private bool m_Started = false;
    private bool m_GameOver = false;
    private int m_Points;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeGame();
    }

    private void InitializeGame()
    {
        // Set player name from DataManager or default
        string currentPlayer = DataManager.Instance?.playerName ?? "Player";
        UpdateBestScoreDisplay();

        // Initialize brick layout
        SetupBricks();
    }

    private void SetupBricks()
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
    }

    private void Update()
    {
        if (!m_Started && Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
        else if (m_GameOver && Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }

    private void StartGame()
    {
        m_Started = true;
        float randomDirection = Random.Range(-1.0f, 1.0f);
        Vector3 forceDir = new Vector3(randomDirection, 1, 0).normalized;

        Ball.transform.SetParent(null);
        Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"{DataManager.Instance.playerName}: {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // Check and update high score
        if (m_Points > DataManager.Instance.bestScore)
        {
            DataManager.Instance.bestScore = m_Points;
            DataManager.Instance.bestPlayer = DataManager.Instance.playerName;
            DataManager.Instance.SaveHighScore();
            UpdateBestScoreDisplay();
        }
    }

    private void UpdateBestScoreDisplay()
    {
        if (BestScoreText != null)
        {
            BestScoreText.text = $"Best: {DataManager.Instance.bestPlayer} - {DataManager.Instance.bestScore}";
        }
    }

    // Removed SaveData class and file operations - moved to DataManager
}