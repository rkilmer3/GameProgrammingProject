using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    Rigidbody2D currentRB;
    private float horizontal;
    public float acceleration = 10;
    public bool isGrounded = true;
    public float jumpHeight = 5;
    public bool movementDisable = false;
    public int ringCount;
    public TMP_Text ringText;
    Animator anime;
    CapsuleCollider2D capsule;
    Transform rotation;
    public SpillRingManager spillRing;
    int enemyScore;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ringCount = 0;
        RingText();
        anime = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider2D>();
        rotation = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        if (movementDisable == false) 
        {
            rb.velocity = new Vector2(horizontal * acceleration, rb.velocity.y);
            if (horizontal > 0 || horizontal < 0)
            {
                anime.SetBool("Running", true);
            }
            else
            {
                anime.SetBool("Running", false);
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
        if (anime.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            anime.SetBool("Jump", false);
        }
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rings"))
        {
            other.gameObject.SetActive(false);
            ringCount += 1;
            RingText();
        }
        if (other.CompareTag("Enemy"))
        {
            if (anime.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
            {
                other.gameObject.SetActive(false);
                rb.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);
                enemyScore += 100;
            }
            else
            {
                if (ringCount > 0)
                {
                    spillRing.ringSpill = true;
                    ringCount -= ringCount;
                    RingText();
                }
                else
                {
                    Death();
                }
            }
        }
        if (other.CompareTag("Goal Ring"))
        {
            StageClear();
            other.gameObject.SetActive(false);
        }
    }

    public void RingText()
    {
        ringText.text = "Rings:" + ringCount.ToString();
    }

    void Rotation()
    {
        if (horizontal < 0)
        {
            rotation.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (horizontal > 0)
        {
            rotation.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    
    void Death()
    {
        movementDisable = true;
        capsule.isTrigger = true;
        anime.SetBool("Dead", true);
    }

    void StageClear()
    {
        movementDisable = true;
        anime.SetBool("StageClear", true);
        int ringScore = ringCount * 100;
        int totalScore = enemyScore + ringScore;
        ringText.text = "Score:" + totalScore.ToString();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb.velocity = new Vector2(horizontal * acceleration, jumpHeight);
            anime.SetBool("Jump", true);
        }
    }
}
