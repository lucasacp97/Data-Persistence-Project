using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    // Singleton instance
    public static DataManager Instance { get; private set; }

    // Player data
    public string playerName = "Player";
    public string bestPlayer = "None";
    public int bestScore = 0;

    // File paths
    private string savePath;
    private const string SAVE_FILE = "/highscore.save";

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDataManager();
    }

    private void InitializeDataManager()
    {
        savePath = Application.persistentDataPath + SAVE_FILE;
        LoadHighScore();
    }

    [System.Serializable]
    private class SaveData
    {
        public string bestPlayer;
        public int bestScore;
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData
        {
            bestPlayer = this.bestPlayer,
            bestScore = this.bestScore
        };

        string json = JsonUtility.ToJson(data);

        try
        {
            File.WriteAllText(savePath, json);
            Debug.Log($"High score saved for {bestPlayer} with {bestScore} points");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save high score: {e.Message}");
        }
    }

    public void LoadHighScore()
    {
        if (!File.Exists(savePath)) return;

        try
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;

            Debug.Log($"Loaded high score: {bestPlayer} with {bestScore} points");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load high score: {e.Message}");
            ResetHighScore();
        }
    }

    public void ResetHighScore()
    {
        bestPlayer = "None";
        bestScore = 0;
        SaveHighScore();
    }

    // Optional: Add this for debugging
    private void OnApplicationQuit()
    {
        SaveHighScore();
    }
}

