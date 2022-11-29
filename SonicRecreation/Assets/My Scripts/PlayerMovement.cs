using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    public float acceleration;
    public bool isGrounded = true;
    public float jumpHeight = 5;
    public float currentSpeed;
    public bool isDead = false;
    int ringCount;
    public TMP_Text ringText;
    public bool rolling;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ringCount = 0;
        RingText();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded) 
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0.0f);
            rb.AddForce(movement * acceleration);
        }

        if (!isGrounded)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            Vector2 movement = new Vector2(moveHorizontal, 0.0f);
            rb.AddForce(movement * 0.2f *acceleration);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            if (isGrounded)
            {
                rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
                rolling = true;
            }
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
        rolling = false;
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rings"))
        {
            other.gameObject.SetActive(false);
            ringCount += 1;
            RingText();
        }
        if (other.CompareTag("Enemy"))
        {
            if (rolling)
            {
                other.gameObject.SetActive(false);
                rb.AddForce(Vector2.up * 0.3f, ForceMode2D.Impulse);
            }
            else
            {
                ringCount -= ringCount;
                RingText();
            }
        }
    }

    public void RingText()
    {
        ringText.text = "Rings:" + ringCount.ToString();
    }
}
