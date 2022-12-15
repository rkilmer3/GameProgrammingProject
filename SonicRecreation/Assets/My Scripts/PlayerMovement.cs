using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    Rigidbody2D currentRB;
    public float acceleration;
    public bool isGrounded = true;
    public float jumpHeight = 5;
    public float currentSpeed;
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
        if (isGrounded == true && movementDisable == false) 
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0.0f);
            rb.AddForce(movement * acceleration);
            if (moveHorizontal > 0 || moveHorizontal < 0)
            {
                anime.SetBool("Running", true);
            }
            else
            {
                anime.SetBool("Running", false);
            }
        }

        if (!isGrounded && movementDisable == false)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0.0f);
            rb.AddForce(movement * 0.2f *acceleration);
        }

        if (Input.GetKeyDown(KeyCode.Space) && movementDisable == false)
        {
            if (isGrounded == true)
            {
                rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
            }
            anime.SetBool("Jump", true);
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
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rotation.rotation = Quaternion.Euler(0, 180, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
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
}
