using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Paddle2 : MonoBehaviour
{
    public bool isPlayer1;
    public float speed = 10f;

    void Update()
    {
        float move = 0;

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("MenuScene");
        }

        if (isPlayer1)
        {
            if (Keyboard.current.wKey.isPressed) move = 1;
            if (Keyboard.current.sKey.isPressed) move = -1;
        }
        else
        {
            if (Keyboard.current.oKey.isPressed) move = 1;
            if (Keyboard.current.lKey.isPressed) move = -1;
        }

        transform.Translate(Vector2.up * move * speed * Time.deltaTime);

        // Keep the paddle on screen
        float yPos = Mathf.Clamp(transform.position.y, -4f, 4f);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}