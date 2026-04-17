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
            if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
            {
                PlayerMovement pm = GetComponent<PlayerMovement>();
                if (pm != null)
                {
                    pm.Bounce();
                }
                return; // Survived by stomping
            }

            AudioSource.PlayClipAtPoint(deathSound, transform.position);
            GameManager.Instance.GameOver();
            gameObject.SetActive(false);

        }
    }
}