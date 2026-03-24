using UnityEngine;

public class CatController : MonoBehaviour
{
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    public float topBoundary = 5f;      // 上边界
    public float bottomBoundary = -5f;  // 下边界
    public AudioSource jumpSound;
    public GameObject gameOverPanel;
    private bool isGameOver = false; // Add this line


    private void Start()
    {
        if (jumpSound == null)
        {
            jumpSound = GetComponent<AudioSource>();
            // 如果仍然没有 AudioSource，则自动添加一个
            if (jumpSound == null)
            {
                jumpSound = gameObject.AddComponent<AudioSource>();
                Debug.LogWarning("未指定 AudioSource，已自动添加。请将音频剪辑拖拽到该组件上。");
            }
            rb = GetComponent<Rigidbody2D>();
            float camHeight = Camera.main.orthographicSize;
            topBoundary = camHeight - 0.5f;
            bottomBoundary = -camHeight + 0.5f;
        }
    }

    private void Update()
    {
        if (isGameOver) return; // Add this line
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (transform.position.y > topBoundary || transform.position.y < bottomBoundary)
        {
            GameOver();
        }
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * jumpForce;
        if (jumpSound != null && jumpSound.clip != null)
        {
            jumpSound.Play();
        }
        else
        {
            Debug.LogWarning("跳跃音效未设置或 AudioClip 为空！");
        }
        }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isGameOver) return; // Add this line

        ScoreManager.score++;
        Debug.Log("Score: " + ScoreManager.score);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cow"))
        {
            // Game over
            GameOver();
        }
    }

    private void GameOver()
    {
        // Show the game over panel

        isGameOver = true; // Add this line
        rb.velocity = Vector2.zero;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}