using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    //private Animator anim;

    public Transform groundPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float moveInputX = Input.GetAxisRaw("Horizontal");
        float moveInputY = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(moveInputX, moveInputY);
        /*
        if (moveInputX != 0 || moveInputY != 0)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        */
        if (moveInputX < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else if (moveInputX > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    
}
