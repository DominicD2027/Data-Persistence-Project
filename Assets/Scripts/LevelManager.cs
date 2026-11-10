using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Gameplay Elements")]
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    [Header("UI Text Elements")]
    public TMP_Text ScoreText;
    public TMP_Text BestScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;
    private InputAction m_LaunchAction;

    void Awake()
    {
        m_LaunchAction = new InputAction(
            "Launch",
            InputActionType.Button,
            "<Keyboard>/space"
        );
    }

    void OnEnable()
    {
        m_LaunchAction.Enable();
    }

    void OnDisable()
    {
        m_LaunchAction.Disable();
    }

    void OnDestroy()
    {
        m_LaunchAction.Dispose();
    }

    void Start()
    {
        m_Points = 0;

        UpdateScoreDisplay();
        UpdateBestScoreDisplay();
        InitializeBricks();

        if (GameOverText != null)
        {
            GameOverText.SetActive(false);
        }
    }

    void Update()
    {
        // If the game is over, Space restarts the level
        if (m_GameOver)
        {
            if (m_LaunchAction.WasPressedThisFrame())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            return;
        }

        // Launch the ball with Space
        if (!m_Started && m_LaunchAction.WasPressedThisFrame())
        {
            m_Started = true;

            if (Ball != null)
            {
                Ball.transform.SetParent(null);

                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
    }

    private void InitializeBricks()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = { 1, 1, 2, 2, 5, 5 };

        for (int i = 0; i < LineCount; ++i)
        {
            for (int j = 0; j < perLine; ++j)
            {
                var brick = Instantiate(BrickPrefab, transform);

                brick.transform.position = new Vector3(
                    (j - perLine / 2) * step + 0.3f,
                    2.0f + i * 0.3f,
                    0
                );

                brick.PointValue = pointCountArray[i % pointCountArray.Length];
            }
        }
    }

    public void AddPoint(int points)
    {
        if (m_GameOver)
            return;

        m_Points += points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (ScoreText != null)
        {
            string currentPlayer = "Player";

            if (MainManager.Instance != null)
            {
                currentPlayer = MainManager.Instance.playerName;
            }

            ScoreText.text = $"{currentPlayer} Score: {m_Points}";
        }
    }

    private void UpdateBestScoreDisplay()
    {
        if (BestScoreText != null)
        {
            if (MainManager.Instance != null)
            {
                BestScoreText.text =
                    $"Best Score: {MainManager.Instance.highScorePlayer} : {MainManager.Instance.highScore}";
            }
            else
            {
                BestScoreText.text = "Best Score: Nobody : 0";
            }
        }
    }

    public void GameOver()
    {
        if (m_GameOver)
            return;

        m_GameOver = true;

        if (GameOverText != null)
        {
            GameOverText.SetActive(true);
        }

        if (MainManager.Instance != null)
        {
            MainManager.Instance.SaveNewHighScore(m_Points);
            UpdateBestScoreDisplay();
        }
    }
}