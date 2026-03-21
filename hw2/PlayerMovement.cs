using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GFX;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform feetPos;
    [SerializeField] private float groundDistance = 0.25f;
    [SerializeField] private float jumpTime = 0.3f;
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private Animator animator;

    private bool isGrounded = false;
    private bool isJumping = false;
    private float jumpTimer;
    public AudioSource jumpSound;
    public AudioSource crouchSound;

    private void Start()
    {
        var aSources = GetComponents<AudioSource>();
        if (aSources.Length >= 2)
        {
            jumpSound = aSources[0];
            crouchSound = aSources[1];
        }

        if (animator == null && GFX != null)
            animator = GFX.GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("Run");
        }
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, groundDistance, groundLayer);

        if (animator != null)
        {
            animator.SetBool("IsGrounded", isGrounded);
        }

        #region JUMPING
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            rb.linearVelocity = Vector2.up * jumpForce;
            jumpSound.Play();
        }

        if (isJumping && Input.GetButton("Jump"))
        {
            if (jumpTimer < jumpTime)
            {
                rb.linearVelocity = Vector2.up * jumpForce;
                jumpTimer += Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetButtonDown("Crouch"))
        {
            isJumping = false;
            jumpTimer = 0f;
            rb.linearVelocity = Vector2.down * jumpForce;
            crouchSound.Play();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
            jumpTimer = 0f;
        }
        #endregion

        #region CROUCHING (Manual Scale)
        if (isGrounded && Input.GetButton("Crouch"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, crouchHeight, GFX.localScale.z);
        }

        if (Input.GetButtonUp("Crouch"))
        {
            GFX.localScale = new Vector3(GFX.localScale.x, 1f, GFX.localScale.z);
        }
        #endregion
    }
}