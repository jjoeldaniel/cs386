using UnityEngine;

public class Ball : MonoBehaviour
{
    public float startingSpeed = 5f;
    public float speedIncrease = 1.1f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Launch();
    }

    public void Launch()
    {
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.linearVelocity = new Vector2(x * startingSpeed, y * startingSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // multiple velocity by speedIncrease factor
            rb.linearVelocity *= speedIncrease;
            Debug.Log("New velocity: " + rb.linearVelocity);
        }
    }

    public void ResetBall()
    {
        transform.position = Vector2.zero;
        rb.linearVelocity = Vector2.zero;
        Invoke("Launch", 1f);
    }
}