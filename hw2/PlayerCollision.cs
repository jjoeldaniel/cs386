using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public AudioClip deathSound;

    public void Start()
    {
        GameManager.Instance.onPlay.AddListener(() => gameObject.SetActive(true));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Obstacle"))
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);

        }
    }
}