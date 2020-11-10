using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpossomBehaviour : MonoBehaviour
{
    public float runForce;
    public Rigidbody2D rigidbody2D;
    public bool isGrounded;
    public bool hasGroundAhead;
    public Transform groundAheadPoint;
    public Transform lookFrontPoint;
    public LayerMask collisionLayer;
    public LayerMask wallcollisionLayer;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _LookInFront();
        _LookAhead();
        _Move();  
    }

    private void _Move()
    {
        if (hasGroundAhead)
        {
            rigidbody2D.AddForce(Vector2.left * runForce * Time.deltaTime* transform.localScale.x);
            
        }
        else
        {
            _FlipXLocalScale();
        }
        rigidbody2D.velocity *= 0.9f;
    }
    private void _LookInFront()
    {
        if (Physics2D.Linecast(transform.position, lookFrontPoint.position, wallcollisionLayer))
        {
            _FlipXLocalScale();
        }
        Debug.DrawLine(transform.position, lookFrontPoint.position, Color.red);
    }
    private void _LookAhead()
    {
        hasGroundAhead = Physics2D.Linecast(transform.position, groundAheadPoint.position,collisionLayer);

        Debug.DrawLine(transform.position, groundAheadPoint.position, Color.green);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
            isGrounded = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platforms"))
            isGrounded = false;
    }

    private void _FlipXLocalScale()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
