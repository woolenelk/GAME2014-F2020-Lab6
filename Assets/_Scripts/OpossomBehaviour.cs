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
    public Vector2 Direction;
    public float z_rotate;
    // Start is called before the first frame update
    void Start()
    {
        z_rotate = 0;
        rigidbody2D = GetComponent<Rigidbody2D>();
        Direction = Vector2.left;
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
            rigidbody2D.AddForce(Direction * runForce * Time.deltaTime* transform.localScale.x);
            
        }
        else
        {
            _FlipXLocalScale();
        }
        rigidbody2D.velocity *= 0.9f;
    }
    private void _LookInFront()
    {
        var wallhit = Physics2D.Linecast(transform.position, lookFrontPoint.position, wallcollisionLayer);
        if (wallhit)
        {
            _FlipXLocalScale();
        }
        Debug.DrawLine(transform.position, lookFrontPoint.position, Color.red);
    }
    private void _LookAhead()
    {
        Vector3 testAngle = (transform.position - lookFrontPoint.position);
        Vector2 testAngle2D = new Vector2(testAngle.x, testAngle.y);

        
        var groundHit = Physics2D.Linecast(transform.position, groundAheadPoint.position, collisionLayer);

        float angle = Vector2.SignedAngle(testAngle2D, groundHit.normal);
        
        float angleDir = 1;
        if (transform.localScale.x > 0)
        {
            if (angle > 80)
            {
                angleDir = -1;
                // going up 
            }
            else if (angle > 0)
            {
                angleDir = 1;
            }
            //else
            //{
            //    angleDir = 1;
            //    // going down
            //}
        }
        else
        {
            if (angle > 0)
            {
                angleDir = -1;
                // going up 
            }
            else if (angle < -80)
            {
                angleDir = 1;
            }
            else
            {
                angleDir = -1;
                // going down
            }
        }
        if (groundHit)
        {
            if (groundHit.collider.CompareTag("Ramps"))
            {
                Debug.Log("normal = " + groundHit.normal);
                Debug.Log("angle = " + angle);
                Debug.Log("angleDir = " + angleDir);

                z_rotate = Mathf.Lerp(z_rotate, -26.0f*angleDir /*transform.localScale.x*angleDir*/, 0.08f);
                Direction = (Vector2.left + (Vector2.up* angleDir/*transform.localScale.x*angleDir*/)).normalized;
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, z_rotate);
                Debug.Log("Ramps");
            }
            if (groundHit.collider.CompareTag("Platforms"))
            {
                z_rotate = Mathf.Lerp(z_rotate, 0.0f, 0.08f);
                Direction = Vector2.left;
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, z_rotate);
                Debug.Log("Platforms");
            }
            hasGroundAhead = true;
        }
        else
        {
            
            hasGroundAhead = false;
        }

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
