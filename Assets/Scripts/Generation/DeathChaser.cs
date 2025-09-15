using UnityEngine;

public class DeathChaser : MonoBehaviour
{
    public Transform target;
    private GameManager gameManager;

    private void LateUpdate()
    {
        if (target.position.y > transform.position.y)
        {
            Vector3 newPosition = new Vector3(transform.position.x, target.position.y - 25, transform.position.z);
            transform.position = newPosition;
        }
    }

    void Start()
    {
        gameManager = Object.FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("platform"))
        {
            Debug.Log("Death wall hit platform!");
            gameManager.OnDeathWallHitPlatform(other.gameObject);
        }
    }
}

