using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using TMPro;

public class ChickenMinigame : MonoBehaviour
{
    public TerminalController terminalController;
    public RectTransform playArea; // The UI panel acting as the screen

    [Header("Game Settings")]
    public float playerSpeed = 400f;
    public float bulletSpeed = 600f;
    public float enemySpeed = 150f;
    public float shootCooldown = 0.5f;

    private GameObject player;
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> bullets = new List<GameObject>();

    [Header("Level 4 Variables")]
    public Sprite rewardSprite;
    public TMP_Text congratulationsText;
    private GameObject rewardObject;

    [Header("Textures")]
    public Sprite playerSprite;
    public Sprite bossSprite; // Keeps the static sprite as a fallback
    public Sprite[] enemyIdleFrames;
    public Sprite[] bossIdleFrames; // Added for the boss animation

    [Header("Animation Settings")]
    public float animationSpeed = 0.2f; // Time in seconds between each frame
    private float animationTimer = 0f;
    private int currentFrame = 0;


    private bool isPlaying = false;
    private int currentLevel = 1;
    private float lastShootTime = 0f;
    private int enemyDirection = 1;
    private int bossHp = 10;

    private float levelStartTime = 0f;

    void Update()
    {
        if (!isPlaying || Keyboard.current == null) return;

        HandlePlayerMovement();
        HandleShooting();
        MoveBullets();
        MoveEnemies();
        AnimateEnemies(); // Added animation call
        CheckCollisions();
        CheckWinLossConditions();
    }

    public void StartGame()
    {
        playArea.gameObject.SetActive(true);
        Canvas.ForceUpdateCanvases();

        // Ensure text is hidden at the start of level 1
        if (congratulationsText != null)
            congratulationsText.gameObject.SetActive(false);

        currentLevel = 1;
        isPlaying = true;
        levelStartTime = Time.time;
        SetupLevel();
    }

    private void SetupLevel()
    {
        ClearEntities();
        CreatePlayer();

        if (currentLevel == 1)
        {
            SpawnEnemies(1, 5);
            Debug.Log("Level 1 started.");
        }
        else if (currentLevel == 2)
        {
            SpawnEnemies(2, 6);
            Debug.Log("Level 2 started.");
        }
        else if (currentLevel == 3)
        {
            SpawnBoss();
            Debug.Log("Boss Level started.");
        }
        else if (currentLevel == 4)
        {
            SpawnReward(); // Triggers the new level 4 state
            Debug.Log("Level 4 started.");
        }
    }

    private void SpawnReward()
    {
        // Creates the object box
        rewardObject = CreateUIElement("Reward", new Vector2(100, 500), Color.yellow);
        RectTransform rect = rewardObject.GetComponent<RectTransform>();
        Image img = rewardObject.GetComponent<Image>();

        if (rewardSprite != null)
        {
            img.sprite = rewardSprite;
        }

        // Anchor it to the bottom so the math aligns with the player
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);

        // Place it exactly halfway up the screen
        rect.anchoredPosition = new Vector2(0, playArea.rect.height / 2f);

        // Turn on the Congratulations text
        if (congratulationsText != null)
        {
            congratulationsText.gameObject.SetActive(true);
        }
    }

    private void AnimateEnemies()
    {
        // Check if we have any frames to animate
        bool hasEnemyFrames = enemyIdleFrames != null && enemyIdleFrames.Length > 0;
        bool hasBossFrames = bossIdleFrames != null && bossIdleFrames.Length > 0;

        if (!hasEnemyFrames && !hasBossFrames) return;

        animationTimer += Time.deltaTime;

        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame++; // Advance the frame counter

            foreach (var enemy in enemies)
            {
                // Animate standard enemies
                if (enemy.name == "Enemy" && hasEnemyFrames)
                {
                    // The % operator safely loops the index back to 0 when it reaches the end
                    int frameIndex = currentFrame % enemyIdleFrames.Length;
                    enemy.GetComponent<Image>().sprite = enemyIdleFrames[frameIndex];
                }
                // Animate the boss
                else if (enemy.name == "Boss" && hasBossFrames)
                {
                    int frameIndex = currentFrame % bossIdleFrames.Length;
                    enemy.GetComponent<Image>().sprite = bossIdleFrames[frameIndex];
                }
            }
        }
    }

    private void HandlePlayerMovement()
    {
        Vector2 pos = player.GetComponent<RectTransform>().anchoredPosition;

        if (Keyboard.current.leftArrowKey.isPressed)
            pos.x -= playerSpeed * Time.deltaTime;
        if (Keyboard.current.rightArrowKey.isPressed)
            pos.x += playerSpeed * Time.deltaTime;

        if (Keyboard.current.upArrowKey.isPressed)
            pos.y += playerSpeed * Time.deltaTime;
        if (Keyboard.current.downArrowKey.isPressed)
            pos.y -= playerSpeed * Time.deltaTime;

        float halfWidth = playArea.rect.width / 2f - 25f;
        pos.x = Mathf.Clamp(pos.x, -halfWidth, halfWidth);

        float bottomLimit = 50f;

        // If enemies are alive OR we are on Level 4, lock the player inside the screen
        if (enemies.Count > 0 || currentLevel == 4)
        {
            float topLimit = playArea.rect.height - 50f;
            pos.y = Mathf.Clamp(pos.y, bottomLimit, topLimit);
        }
        else
        {
            // If enemies are dead and it's NOT Level 4, let them fly off the top
            pos.y = Mathf.Max(pos.y, bottomLimit);
        }

        player.GetComponent<RectTransform>().anchoredPosition = pos;
    }

    private void HandleShooting()
    {
        if (Keyboard.current.spaceKey.isPressed && Time.time > lastShootTime + shootCooldown)
        {
            CreateBullet();
            lastShootTime = Time.time;
        }
    }

    private void MoveBullets()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            RectTransform rb = bullets[i].GetComponent<RectTransform>();
            rb.anchoredPosition += new Vector2(0, bulletSpeed * Time.deltaTime);

            // Now checks against the full height of the play area since the anchor is at the bottom
            if (rb.anchoredPosition.y > playArea.rect.height)
            {
                Destroy(bullets[i]);
                bullets.RemoveAt(i);
            }
        }
    }

    private void MoveEnemies()
    {
        bool hitEdge = false;
        foreach (var enemy in enemies)
        {
            RectTransform re = enemy.GetComponent<RectTransform>();
            re.anchoredPosition += new Vector2(enemySpeed * enemyDirection * Time.deltaTime, 0);

            float halfWidth = playArea.rect.width / 2f - re.rect.width / 2f;
            if (re.anchoredPosition.x > halfWidth || re.anchoredPosition.x < -halfWidth)
            {
                hitEdge = true;
            }
        }

        if (hitEdge)
        {
            enemyDirection *= -1;
            foreach (var enemy in enemies)
            {
                RectTransform re = enemy.GetComponent<RectTransform>();
                re.anchoredPosition += new Vector2(0, -30f); // Move down
            }
        }
    }

    private void CheckCollisions()
    {
        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                if (bullets.Count <= i || enemies.Count <= j) continue;

                if (Overlaps(bullets[i].GetComponent<RectTransform>(), enemies[j].GetComponent<RectTransform>()))
                {
                    Destroy(bullets[i]);
                    bullets.RemoveAt(i);

                    if (currentLevel == 3) // Boss logic
                    {
                        bossHp--;
                        if (bossHp <= 0)
                        {
                            Destroy(enemies[j]);
                            enemies.RemoveAt(j);
                        }
                    }
                    else // Normal enemy logic
                    {
                        Destroy(enemies[j]);
                        enemies.RemoveAt(j);
                    }
                    break;
                }
            }
        }
    }

    private void CheckWinLossConditions()
    {
        if (Time.time < levelStartTime + 0.5f) return;

        // Level 4 condition: Touch the reward to win
        if (currentLevel == 4)
        {
            if (rewardObject != null && Overlaps(player.GetComponent<RectTransform>(), rewardObject.GetComponent<RectTransform>()))
            {
                // --- NEW CODE: Change the planet to Egg when you win ---
                if (PlanetManager.Instance != null)
                {
                    PlanetManager.Instance.ShowPlanet("Egg");
                }
                // -------------------------------------------------------

                EndGame("YOU WIN! SYSTEM SECURED.");
            }
            return; // Stop checking for enemies since there are none
        }

        // Win condition: All enemies destroyed AND player flies off screen
        if (enemies.Count == 0)
        {
            float topOfScreen = playArea.rect.height + 100f;

            if (player.GetComponent<RectTransform>().anchoredPosition.y > topOfScreen)
            {
                if (currentLevel < 4)
                {
                    currentLevel++;
                    levelStartTime = Time.time;
                    SetupLevel();
                }
            }
        }

        // Loss condition: Enemies reach bottom
        float bottomLimit = -playArea.rect.height / 2f + 50f;
        foreach (var enemy in enemies)
        {
            if (enemy.GetComponent<RectTransform>().anchoredPosition.y < bottomLimit)
            {
                EndGame("GAME OVER. TERMINAL COMPROMISED.");
                break;
            }
        }
    }

    private void EndGame(string message)
    {
        isPlaying = false;
        ClearEntities();
        playArea.gameObject.SetActive(false);

        // Ensure text is hidden when the game ends
        if (congratulationsText != null)
            congratulationsText.gameObject.SetActive(false);

        // Reactivate terminal UI
        terminalController.outputText.gameObject.SetActive(true);
        terminalController.inputField.gameObject.SetActive(true);

        terminalController.outputText.text += $"\n<color=yellow>{message}</color>\n";

        terminalController.inputField.ActivateInputField();
        terminalController.inputField.Select();

        Debug.Log("Minigame ended.");
    }

    // --- HELPER METHODS FOR PROCEDURAL GENERATION ---

    private void CreatePlayer()
    {
        // Changed the size to 50x100 to give the rocket more vertical room
        player = CreateUIElement("Player", new Vector2(50, 100), Color.white);
        RectTransform rect = player.GetComponent<RectTransform>();
        Image img = player.GetComponent<Image>();

        if (playerSprite != null)
        {
            img.sprite = playerSprite;
            // This tells Unity to keep the original proportions and prevents stretching
            img.preserveAspect = false;
        }

        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.anchoredPosition = new Vector2(0, 50f);
    }

    private void CreateBullet()
    {
        GameObject bullet = CreateUIElement("Bullet", new Vector2(10, 20), Color.green);
        RectTransform rect = bullet.GetComponent<RectTransform>();

        // Match the player's anchors (Bottom-Center) so the math aligns perfectly
        rect.anchorMin = new Vector2(0.5f, 0f);
        rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);

        // Spawn the bullet exactly 50 pixels above the player's current position
        rect.anchoredPosition = player.GetComponent<RectTransform>().anchoredPosition + new Vector2(0, 50f);

        bullets.Add(bullet);
    }

    private void SpawnEnemies(int rows, int cols)
    {
        float startX = -150f;
        float startY = playArea.rect.height / 2f - 50f;

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                // Changed the size from (40, 40) to (40, 80) to stretch them vertically
                GameObject enemy = CreateUIElement("Enemy", new Vector2(35, 200), Color.white);

                // Apply the first frame of the animation if available
                if (enemyIdleFrames != null && enemyIdleFrames.Length > 0)
                {
                    enemy.GetComponent<Image>().sprite = enemyIdleFrames[0];
                }
                else
                {
                    // Fallback to red color if no sprites are assigned
                    enemy.GetComponent<Image>().color = Color.red;
                }

                // Increased the vertical spacing from 60f to 100f so the taller enemies do not overlap
                enemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(startX + (c * 60f), startY - (r * 100f));
                enemies.Add(enemy);
            }
        }
    }

    private void SpawnBoss()
    {
        bossHp = 10;

        GameObject boss = CreateUIElement("Boss", new Vector2(150, 600), Color.white);
        Image img = boss.GetComponent<Image>();

        // Prevents the boss image from squashing/stretching
        img.preserveAspect = false;

        // Apply the first frame of the animation if available
        if (bossIdleFrames != null && bossIdleFrames.Length > 0)
        {
            img.sprite = bossIdleFrames[0];
        }
        else if (bossSprite != null)
        {
            // Fallback to static sprite
            img.sprite = bossSprite;
        }
        else
        {
            // Fallback to purple color
            img.color = new Color(0.5f, 0f, 0.5f);
        }

        boss.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, playArea.rect.height / 2f - 100f);
        enemies.Add(boss);
    }

    private GameObject CreateUIElement(string name, Vector2 size, Color color)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(playArea, false);
        Image img = obj.AddComponent<Image>();
        img.color = color;

        RectTransform rect = obj.GetComponent<RectTransform>();

        // Force anchors to the center so positioning math works correctly
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);

        rect.sizeDelta = size;
        return obj;
    }

    private void ClearEntities()
    {
        if (player != null) Destroy(player);
        if (rewardObject != null) Destroy(rewardObject); // Clean up the reward object
        foreach (var e in enemies) Destroy(e);
        foreach (var b in bullets) Destroy(b);
        enemies.Clear();
        bullets.Clear();
    }

    private bool Overlaps(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] corners1 = new Vector3[4];
        Vector3[] corners2 = new Vector3[4];
        rect1.GetWorldCorners(corners1);
        rect2.GetWorldCorners(corners2);

        Rect r1 = new Rect(corners1[0].x, corners1[0].y, corners1[2].x - corners1[0].x, corners1[2].y - corners1[0].y);
        Rect r2 = new Rect(corners2[0].x, corners2[0].y, corners2[2].x - corners2[0].x, corners2[2].y - corners2[0].y);

        return r1.Overlaps(r2);
    }
}