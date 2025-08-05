using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuUIHandler : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInput;
    public TMP_Text bestScoreText;

    private void Start()
    {
        // Initialize input field with default name if available
        if (DataManager.Instance != null)
        {
            nameInput.text = DataManager.Instance.playerName;
        }

        // Display best score information
        UpdateBestScoreDisplay();
    }

    private void UpdateBestScoreDisplay()
    {
        if (DataManager.Instance != null && bestScoreText != null)
        {
            DataManager.Instance.LoadHighScore();
            bestScoreText.text = $"Best Score: {DataManager.Instance.bestPlayer} - {DataManager.Instance.bestScore}";
        }
    }

    public void StartGame()
    {
        // Validate and save player name
        if (string.IsNullOrWhiteSpace(nameInput.text))
        {
            nameInput.text = "Player"; // Default name
        }

        if (DataManager.Instance != null)
        {
            DataManager.Instance.playerName = nameInput.text.Trim();
        }

        // Load game scene
        SceneManager.LoadScene(1); // Or use your exact game scene name
    }

    public void QuitGame()
    {
        // Save data before quitting if needed
        if (DataManager.Instance != null)
        {
            DataManager.Instance.SaveHighScore();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        Debug.Log("Game quit requested");
    }
}

