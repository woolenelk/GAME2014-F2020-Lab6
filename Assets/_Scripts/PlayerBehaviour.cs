using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;
    public bool isGrounded;

    private Rigidbody2D m_rigidbody2d;
    private SpriteRenderer m_spriteRenderer;
    private Animator m_animator;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (!isGrounded)
            return;
        if (joystick.Horizontal > joystickHorizontalSensitivity)
        {
            m_rigidbody2d.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
            m_spriteRenderer.flipX = false;
            m_animator.SetInteger("AnimState", 1);
        }
        else if (joystick.Horizontal < -joystickHorizontalSensitivity)
        {
            m_rigidbody2d.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
            m_spriteRenderer.flipX = true;
            m_animator.SetInteger("AnimState", 1);
        }
        else if (joystick.Vertical > joystickVerticalSensitivity)
        {
            m_rigidbody2d.AddForce(Vector2.up * verticalForce * Time.deltaTime);
            m_animator.SetInteger("AnimState", 2);

        }
        else
        {
            m_animator.SetInteger("AnimState", 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false ;
    }
}
