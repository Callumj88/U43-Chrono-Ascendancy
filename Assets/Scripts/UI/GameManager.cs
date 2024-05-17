using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI spawnerCountText;
    public TextMeshProUGUI killCounterText;
    public TextMeshProUGUI playerHealthText;
    public GameObject winPanelPrefab;
    public GameObject gameOverScreenPrefab;
    public float invincibilityDuration = 0.5f;

    private int spawnerCount;
    private int killCount;
    private float playerHealth;
    private GameObject winPanel;
    private GameObject gameOverScreen;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            InstantiateUIElements();
            ReassignReferences();
            ResetGameState();
        }
    }

    void InstantiateUIElements()
    {
        if (winPanelPrefab != null && winPanel == null)
        {
            winPanel = Instantiate(winPanelPrefab);
            winPanel.name = "WinPanel";
            winPanel.SetActive(false);
        }

        if (gameOverScreenPrefab != null && gameOverScreen == null)
        {
            gameOverScreen = Instantiate(gameOverScreenPrefab);
            gameOverScreen.name = "GameOverPanel";
            gameOverScreen.SetActive(false);
        }
    }

    public void ResetGameState()
    {
        playerHealth = FindObjectOfType<PlayerMovement>().playerStats.MaxHealth;
        killCount = 0;
        spawnerCount = FindObjectsOfType<MonsterSpawner>().Length;
        UpdateSpawnerCountUI();
        UpdateKillCounterUI();
        UpdatePlayerHealthUI();

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    void ReassignReferences()
    {
        playerHealthText = GameObject.Find("PlayerHealthText")?.GetComponent<TextMeshProUGUI>();
        spawnerCountText = GameObject.Find("SpawnerCountText")?.GetComponent<TextMeshProUGUI>();
        killCounterText = GameObject.Find("KillCounterText")?.GetComponent<TextMeshProUGUI>();
    }

    public void AddSpawner()
    {
        spawnerCount++;
        UpdateSpawnerCountUI();
    }

    public void RemoveSpawner()
    {
        spawnerCount--;
        UpdateSpawnerCountUI();

        if (spawnerCount <= 0)
        {
            StartCoroutine(HandleWinCondition());
        }
    }

    public void IncrementKillCount()
    {
        killCount++;
        UpdateKillCounterUI();
    }

    public void TakeDamage(float damage)
    {
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null && !player.IsInvincible)
        {
            playerHealth -= damage;
            StartCoroutine(InvincibilityCoroutine());
            if (playerHealth <= 0)
            {
                GameOver();
            }
            UpdatePlayerHealthUI();
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.IsInvincible = true;
            yield return new WaitForSeconds(invincibilityDuration);
            player.IsInvincible = false;
        }
    }

    void UpdatePlayerHealthUI()
    {
        if (playerHealthText != null)
        {
            playerHealthText.text = playerHealth + "HP";
        }
    }

    public void GameOver()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
            StartCoroutine(GameOverCoroutine());
        }
    }

    IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Menu");
    }

    void UpdateSpawnerCountUI()
    {
        if (spawnerCountText != null)
        {
            spawnerCountText.text = "Spawners Left: " + spawnerCount;
        }
    }

    void UpdateKillCounterUI()
    {
        if (killCounterText != null)
        {
            killCounterText.text = "Kills: " + killCount;
        }
    }

    IEnumerator HandleWinCondition()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Menu");
    }
}
