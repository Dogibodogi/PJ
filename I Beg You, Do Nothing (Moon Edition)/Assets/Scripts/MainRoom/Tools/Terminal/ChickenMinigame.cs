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

    [Header("Textures")]
    public Sprite playerSprite;
    public Sprite bossSprite;

    // Array to hold the 3 animation frames
    public Sprite[] enemyIdleFrames;

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

        currentLevel = 1;
        isPlaying = true;
        levelStartTime = Time.time; // Set the timer
        SetupLevel();
    }

    private void SetupLevel()
    {
        ClearEntities();
        CreatePlayer();

        if (currentLevel == 1)
        {
            SpawnEnemies(1, 5); // 1 row, 5 enemies
            Debug.Log("Level 1 started.");
        }
        else if (currentLevel == 2)
        {
            SpawnEnemies(2, 6); // 2 rows, 6 enemies
            Debug.Log("Level 2 started.");
        }
        else if (currentLevel == 3)
        {
            SpawnBoss(); // Boss level
            Debug.Log("Boss Level started.");
        }
    }

    private void AnimateEnemies()
    {
        // Do nothing if we don't have frames assigned
        if (enemyIdleFrames == null || enemyIdleFrames.Length == 0) return;

        animationTimer += Time.deltaTime;

        // When the timer exceeds our speed, move to the next frame
        if (animationTimer >= animationSpeed)
        {
            animationTimer = 0f;
            currentFrame++;

            // Loop back to the first frame if we reach the end
            if (currentFrame >= enemyIdleFrames.Length)
            {
                currentFrame = 0;
            }

            // Apply the new frame to all standard enemies
            foreach (var enemy in enemies)
            {
                // Make sure we only animate the regular enemies, not the boss
                if (enemy.name == "Enemy")
                {
                    enemy.GetComponent<Image>().sprite = enemyIdleFrames[currentFrame];
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

        // Keep player in bounds
        float halfWidth = playArea.rect.width / 2f - 25f;
        pos.x = Mathf.Clamp(pos.x, -halfWidth, halfWidth);

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
        // Wait half a second before checking to prevent frame 1 UI glitches
        if (Time.time < levelStartTime + 0.5f) return;

        // Win condition: All enemies destroyed
        if (enemies.Count == 0)
        {
            if (currentLevel < 3)
            {
                currentLevel++;
                levelStartTime = Time.time; // Reset timer for the next level
                SetupLevel();
            }
            else
            {
                EndGame("YOU WIN! SYSTEM SECURED.");
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

        // Reactivate terminal UI
        terminalController.outputText.gameObject.SetActive(true);
        terminalController.inputField.gameObject.SetActive(true);

        terminalController.outputText.text += $"\n<color=yellow>{message}</color>\n";

        // Refocus input
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
            img.preserveAspect = true;
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

        // We use Color.white here so the sprite's natural colors show properly
        GameObject boss = CreateUIElement("Boss", new Vector2(150, 100), Color.white);

        // Apply the boss sprite if you assigned one in the Inspector
        if (bossSprite != null)
        {
            boss.GetComponent<Image>().sprite = bossSprite;
        }
        else
        {
            // Fallback to purple color if no sprite is assigned
            boss.GetComponent<Image>().color = new Color(0.5f, 0f, 0.5f);
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