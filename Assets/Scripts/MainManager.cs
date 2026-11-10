using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance { get; private set; }

    [Header("Player Data")]
    public string playerName = "Player";
    public int highScore = 0;
    public string highScorePlayer = "Nobody";

    [Header("Menu UI References")]
    public TMP_InputField nameField;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScorePlayer = PlayerPrefs.GetString("HighScorePlayer", "Nobody");
    }

    public void SaveNameAndStartGame()
    {
        playerName = nameField != null &&
                     !string.IsNullOrWhiteSpace(nameField.text)
                     ? nameField.text.Trim()
                     : "Player";

        SceneManager.LoadScene("main");
    }

    public void SaveNewHighScore(int score)
    {
        if (score > highScore)
        {
            highScore = score;
            highScorePlayer = playerName;

            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.SetString("HighScorePlayer", highScorePlayer);
            PlayerPrefs.Save();
        }
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}