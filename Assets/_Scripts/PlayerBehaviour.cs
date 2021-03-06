﻿using System.Collections;
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
    public bool isJumping;
    public bool isCrouch;
    public Transform spawnpoint;
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
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (!isGrounded)
            return;
        if (joystick.Horizontal > joystickHorizontalSensitivity && !isCrouch)
        {
            m_rigidbody2d.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
            m_spriteRenderer.flipX = false;
            if (!isJumping)
                m_animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);
        }
        else if (joystick.Horizontal < -joystickHorizontalSensitivity && !isCrouch)
        {
            m_rigidbody2d.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
            m_spriteRenderer.flipX = true;
            if (!isJumping)
                m_animator.SetInteger("AnimState", (int)PlayerAnimationType.RUN);
        }
        else if (!isJumping)
        {
            m_animator.SetInteger("AnimState", (int)PlayerAnimationType.IDLE);
        }

        if (joystick.Vertical > joystickVerticalSensitivity && !isJumping)
        {
            isJumping = true;
            m_rigidbody2d.AddForce(Vector2.up * verticalForce);
            m_animator.SetInteger("AnimState", (int)PlayerAnimationType.JUMP);

        }
        else
        {
            isJumping = false;
        }

        //if (joystick.Vertical < joystickVerticalSensitivity)
        //{
        //    isCrouch = true;
        //    m_animator.SetInteger("AnimState", (int)PlayerAnimationType.CROUCH);
        //}
        //else
        //{
        //    isCrouch = false;
        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
            isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
            isGrounded = false ;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // respawn
        if (collision.gameObject.CompareTag("Deathplane"))
            transform.position = spawnpoint.position;
    }
}
