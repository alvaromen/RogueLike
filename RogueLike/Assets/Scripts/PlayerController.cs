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
    public GameObject bulletPrefab;

    private bool isShooting;
    private float fireRate;

    private void Start()
    {
        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        isShooting = false;
        fireRate = 0.5f;
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
        if (Input.GetKey("up"))
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(Shoot("up"));
            }
        }
        if (Input.GetKey("down"))
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(Shoot("down"));
            }
        }
        if (Input.GetKey("left"))
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(Shoot("left"));
            }
        }
        if (Input.GetKey("right"))
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(Shoot("right"));
            }
        }
    }

    /**
     * Creates a bullet and apply it a force in order to move it in the direction given by key
     */
    private IEnumerator Shoot(string key)
    {

        Quaternion q = Quaternion.identity;
        Vector3 pos = transform.position;
        Vector3 force = new Vector3(0, 0, 0);

        switch (key)
        {
            //The quaternion for the prefab is taking into account that the bullet originally is looking to the right
            case "up":
                q[0] = 90;
                pos.y += 0.5f; // displace the bullet a little bit to appear above of the player
                force.y += 10;
                break;
            case "down":
                q[0] = -90;
                pos.y += -0.5f; // displace the bullet a little bit to appear below of the player
                force.y -= 10;
                break;
            case "left":
                q[1] = 180;
                pos.x += -0.5f; // displace the bullet a little bit to appear at the left of the player
                force.x -= 10;
                break;
            case "right":
                pos.x += 0.5f; // displace the bullet a little bit to appear at the right of the player
                force.x += 10;
                break;
            default:
                break;
        }

        GameObject bullet = Instantiate(bulletPrefab, pos, q);
        bullet.GetComponent<Rigidbody2D>().AddForce(force);

        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }
    
}
