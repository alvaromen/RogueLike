﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    private Animator anim;

    public Transform groundPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public GameObject bulletPrefab;

    private int speed;
    private int bulletSpeed;
    private bool isShooting;
    private float fireRate;
    private float lastAngle;

    private Transform bulletsHolder; //variable to store references to the transform of our Board to keep the hierarchy clean
    public AudioClip[] shootAudioClips;
    public AudioClip[] dmgAudioClips;
    public AudioClip lowHealthClip;
    private AudioSource audioSource;

    private void Start()
    {
        hp = 10;
        damage = 1;

        audioSource = gameObject.AddComponent<AudioSource>();

        //anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speed = 5;
        bulletSpeed = 10;

        isShooting = false;
        fireRate = 0.5f;

        bulletsHolder = new GameObject("PlayerBullets").transform;
    }

    private void Update()
    {
        float moveInputX = Input.GetAxisRaw("Horizontal");
        float moveInputY = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(moveInputX * speed, moveInputY * speed);

        if (!isShooting)
        {
            if (moveInputY > 0)
            {
                if (moveInputX == 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    lastAngle = 0;
                } else if(moveInputX > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 315);
                    lastAngle = 315;
                } else
                {
                    transform.eulerAngles = new Vector3(0, 0, 45);
                    lastAngle = 45;
                }
            } else
            {
                if(moveInputY < 0)
                {
                    if (moveInputX == 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 180);
                        lastAngle = 180;
                    }
                    else if (moveInputX > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 225);
                        lastAngle = 225;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 135);
                        lastAngle = 135;
                    }
                } else
                {
                    if(moveInputX == 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, lastAngle);
                    } else if (moveInputX > 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, 270);
                        lastAngle = 270;
                    }
                    else
                    {
                        transform.eulerAngles = new Vector3(0, 0, 90);
                        lastAngle = 90;
                    }
                }
            }
        }

        if (Input.GetKey("up"))
        {
            if (!isShooting)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                lastAngle = 0;
                isShooting = true;
                StartCoroutine(Shoot("up"));
            }
        }
        if (Input.GetKey("down"))
        {
            if (!isShooting)
            {
                transform.eulerAngles = new Vector3(0, 0, 180);
                lastAngle = 180;
                isShooting = true;
                StartCoroutine(Shoot("down"));
            }
        }
        if (Input.GetKey("left"))
        {
            if (!isShooting)
            {
                transform.eulerAngles = new Vector3(0, 0, 90);
                lastAngle = 90;
                isShooting = true;
                StartCoroutine(Shoot("left"));
            }
        }
        if (Input.GetKey("right"))
        {
            if (!isShooting)
            {
                transform.eulerAngles = new Vector3(0, 0, -90);
                lastAngle = -90;
                isShooting = true;
                StartCoroutine(Shoot("right"));
            }
        }
    }

    public new void GetHurt(int dmg)
    {
        hp -= dmg;
        audioSource.PlayOneShot(dmgAudioClips[Random.Range(0, dmgAudioClips.Length)]);
        
        // if((hp / maxHp) < 0.3){
        //     audioSource.PlayOneShot(lowHealthClip);
        // }

        if(hp <= 0)
        {
            //die
        }
    }

    /**
     * Creates a bullet and apply it a force in order to move it in the direction given by key
     */
    private IEnumerator Shoot(string key)
    {

        Quaternion q = Quaternion.identity;
        Vector3 pos = transform.position;
        Vector3 vel = new Vector3(0, 0, 0);

        switch (key)
        {
            //The quaternion for the prefab is taking into account that the bullet originally is looking to the right
            case "up":
                q = Quaternion.Euler(0, 0, 90);
                pos.y += 1f; // displace the bullet a little bit to appear above of the player
                vel.y = bulletSpeed;
                break;
            case "down":
                q = Quaternion.Euler(0, 0, 270);
                pos.y += -1f; // displace the bullet a little bit to appear below of the player
                vel.y = -bulletSpeed;
                break;
            case "left":
                q = Quaternion.Euler(0, 0, 180);
                pos.x += -1f; // displace the bullet a little bit to appear at the left of the player
                vel.x = -bulletSpeed;
                break;
            case "right":
                pos.x += 1f; // displace the bullet a little bit to appear at the right of the player
                vel.x = bulletSpeed;
                break;
            default:
                break;
        }

        GameObject bullet = Instantiate(bulletPrefab, pos, q);
        bullet.GetComponent<Rigidbody2D>().velocity = vel;
        bullet.GetComponent<BulletController>().SetDamage(damage);
        bullet.tag = "PlayerBullet";
        bullet.transform.SetParent(bulletsHolder);
        int randIndex = Random.Range(0, shootAudioClips.Length);
        
        audioSource.PlayOneShot(shootAudioClips[randIndex]);


        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }

    public void setPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Door"))
        {
            other.GetComponent<DoorController>().Entering();

            float xOffset = -0.5f;
            float yOffset = -0f;
            float x = ((transform.position.x - transform.position.x % 16) + 8) + xOffset;
            float y = ((transform.position.y - transform.position.y % 16) + 8) + yOffset;
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(x, y, -10);
        }
    }
}
