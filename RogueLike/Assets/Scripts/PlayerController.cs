﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class PlayerController : Character
{
    public GameObject bulletPrefab;
    public int points;
    private int speed;
    public int speedIncrementFactor;
    private int bulletSpeed;
    private bool isShooting;
    public float fireRate;
    private float lastAngle;

    private Transform bulletsHolder; //variable to store references to the transform of our Board to keep the hierarchy clean
    public AudioClip[] shootAudioClips;
    public AudioClip[] dmgAudioClips;
    public AudioClip lowHealthClip;
    public AudioClip fireRateClip;
    public AudioClip speedClip;
    public AudioClip healClip;
    public AudioClip gunPowerupClip;
    public AudioClip bossKeyClip;
    public bool hasBossKey;
    private AudioSource audioSource;

    private float lastX;
    private float lastY;
    private bool movingX;
    private bool movingY;
    private bool visitada;

    private bool tripleShoot;

    private PlayerStats healthBar;
    private bool inmortal;
    public GameObject winText;

    private void Start()
    {
        winText.SetActive(false);

        healthBar = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        healthBar.maxHealth = maxHp;
        healthBar.health = hp;
        healthBar.Heal(maxHp);

        hp = 10.0f;
        damage = 1.0f;

        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        speed = 5;
        speedIncrementFactor = 1;
        bulletSpeed = 10;

        isShooting = false;
        fireRate = 0.5f;

        bulletsHolder = new GameObject("PlayerBullets").transform;

        lastX = transform.position.x - transform.position.x % 16;
        lastY = transform.position.y - transform.position.y % 16;

        movingX = false;
        movingY = false;
        visitada = true;

        tripleShoot = false;

        inmortal = false;
    }

    private void Update()
    {
        Move();
        Attack();
    }

    private void Attack()
    {

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
        else
        {
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
            else
            {
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
                else
                {
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
            }
        }

        float currentX = transform.position.x - transform.position.x % 16;
        float currentY = transform.position.y - transform.position.y % 16;

        if (currentX != lastX || currentY != lastY)
        {
            float x = (transform.position.x - transform.position.x % 16) + 8;
            float y = (transform.position.y - transform.position.y % 16) + 8;
            GameObject.FindGameObjectWithTag("MainCamera").transform.position = new Vector3(x, y, -10);
            if (currentX != lastX)
            {
                movingX = true;
                lastX = currentX;
            }
            if (currentY != lastY)
            {
                movingY = true;
                lastY = currentY;
            }
            visitada = false;
        }

        if (!visitada)
        {
            if (movingX)
            {
                if (transform.position.x % 16 > 3 && transform.position.x % 16 < 13)
                {
                    VisitRoom();
                    movingX = false;
                }
            }
            if (movingY)
            {
                if (transform.position.y % 16 > 3 && transform.position.y % 16 < 13)
                {
                    VisitRoom();
                    movingY = false;
                }
            }
        }
    }

    private void Move()
    {
        float moveInputX = Input.GetAxisRaw("Horizontal");
        float moveInputY = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(moveInputX * speed * speedIncrementFactor, moveInputY * speed * speedIncrementFactor);

        if (!isShooting)
        {
            if (moveInputY > 0)
            {
                if (moveInputX == 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    lastAngle = 0;
                }
                else if (moveInputX > 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 315);
                    lastAngle = 315;
                }
                else
                {
                    transform.eulerAngles = new Vector3(0, 0, 45);
                    lastAngle = 45;
                }
            }
            else
            {
                if (moveInputY < 0)
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
                }
                else
                {
                    if (moveInputX == 0)
                    {
                        transform.eulerAngles = new Vector3(0, 0, lastAngle);
                    }
                    else if (moveInputX > 0)
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
    }

    private void VisitRoom()
    {
        GameObject.FindGameObjectWithTag("LevelGenerator").GetComponent<LevelGenerator>().VisitRoom(new Vector2(lastX, lastY));
    }

    public void GetHurt(float dmg)
    {
        if (!inmortal)
        {
            StartCoroutine(LoseLife(dmg));
        }
        if (hp <= 0)
        {
            winText.SetActive(true);
            winText.GetComponent<Text>().text = "RIP. Has obtenido " + points + " puntos.";
            Invoke("ReloadGame", 5f);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator LoseLife(float dmg)
    {
        inmortal = true;

        hp -= dmg;
        healthBar.TakeDamage(dmg);
        audioSource.PlayOneShot(dmgAudioClips[Random.Range(0, dmgAudioClips.Length)]);

        if ((hp / maxHp) < 0.3)
        {
            audioSource.PlayOneShot(lowHealthClip);
        }

        yield return new WaitForSeconds(1);

        inmortal = false;
    }

    public void GetBoostObject(string boostName){
        switch(boostName){
            case "Boss Key(Clone)":
                // bool key is now true, and should not appear again. We aew able to go into the boss room.
                audioSource.PlayOneShot(bossKeyClip);
                hasBossKey = true;
                break;
            case "Life Powerup(Clone)":
                audioSource.PlayOneShot(healClip);
                hp = maxHp;
                healthBar.Heal(maxHp);
                break;
            case "Gun Powerup(Clone)":
                // implementar triple disparo
                audioSource.PlayOneShot(gunPowerupClip);

                if (!tripleShoot)
                    tripleShoot = true;
                else damage += 1;
                break;
            case "Speed Powerup(Clone)":
                audioSource.PlayOneShot(speedClip);
                speedIncrementFactor = Mathf.Clamp(speedIncrementFactor *= 2, 0, 4); 
                break;
            case "Fire Ratio Powerup(Clone)":
                audioSource.PlayOneShot(fireRateClip);
                fireRate = (fireRate /= 2f) > 0.1f ? (fireRate /= 2f) : 0.1f;
                break;
        }
        AddPoints(500);
    }

    /**
     * Creates a bullet and apply it a force in order to move it in the direction given by key
     */
    private IEnumerator Shoot(string key)
    {
        List<Quaternion> q = new List<Quaternion>();
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> vel = new List<Vector3>();
        List<GameObject> bullets = new List<GameObject>();

        switch (key)
        {
            //The quaternion for the prefab is taking into account that the bullet originally is looking to the right
            case "up":
                q.Add(Quaternion.Euler(0, 0, 90));
                pos.Add(new Vector3(0, 1, 0));
                vel.Add(new Vector3(0, bulletSpeed, 0));
                if (tripleShoot)
                {
                    //up-left
                    q.Add(Quaternion.Euler(0, 0, 135));
                    pos.Add(new Vector3(-1, 1, 0));
                    vel.Add(new Vector3(-bulletSpeed, bulletSpeed, 0));

                    //up-right
                    q.Add(Quaternion.Euler(0, 0, 45));
                    pos.Add(new Vector3(1, 1, 0));
                    vel.Add(new Vector3(bulletSpeed, bulletSpeed, 0));
                }
                break;
            case "down":
                q.Add(Quaternion.Euler(0, 0, 270));
                pos.Add(new Vector3(0, -1, 0));
                vel.Add(new Vector3(0, -bulletSpeed, 0));
                if (tripleShoot)
                {
                    //down-left
                    q.Add(Quaternion.Euler(0, 0, 235));
                    pos.Add(new Vector3(-1, -1, 0));
                    vel.Add(new Vector3(-bulletSpeed, -bulletSpeed, 0));

                    //down-right
                    q.Add(Quaternion.Euler(0, 0, 315));
                    pos.Add(new Vector3(1, -1, 0));
                    vel.Add(new Vector3(bulletSpeed, -bulletSpeed, 0));
                }
                break;
            case "left":
                q.Add(Quaternion.Euler(0, 0, 180));
                pos.Add(new Vector3(-1, 0, 0));
                vel.Add(new Vector3(-bulletSpeed, 0, 0));
                if (tripleShoot)
                {
                    //up-left
                    q.Add(Quaternion.Euler(0, 0, 135));
                    pos.Add(new Vector3(-1, 1, 0));
                    vel.Add(new Vector3(-bulletSpeed, bulletSpeed, 0));

                    //down-left
                    q.Add(Quaternion.Euler(0, 0, 235));
                    pos.Add(new Vector3(-1, -1, 0));
                    vel.Add(new Vector3(-bulletSpeed, -bulletSpeed, 0));
                }
                break;
            case "right":
                q.Add(Quaternion.identity);
                pos.Add(new Vector3(1, 0, 0));
                vel.Add(new Vector3(bulletSpeed, 0, 0)); if (tripleShoot)
                {
                    //up-right
                    q.Add(Quaternion.Euler(0, 0, 45));
                    pos.Add(new Vector3(1, 1, 0));
                    vel.Add(new Vector3(bulletSpeed, bulletSpeed, 0));

                    //down-right
                    q.Add(Quaternion.Euler(0, 0, 315));
                    pos.Add(new Vector3(1, -1, 0));
                    vel.Add(new Vector3(bulletSpeed, -bulletSpeed, 0));
                }
                break;
            default:
                break;
        }

        int n = 1;
        if (tripleShoot)
            n = 3;

        for(int i = 0; i < n; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + pos[i], q[i]);
            bullet.GetComponent<Rigidbody2D>().velocity = vel[i];
            bullet.GetComponent<BulletController>().SetDamage(damage);
            bullet.tag = "PlayerBullet";
            bullet.transform.SetParent(bulletsHolder);
            bullets.Add(bullet);
        }

        if(tripleShoot)
            audioSource.PlayOneShot(shootAudioClips[0]);
        else
            audioSource.PlayOneShot(shootAudioClips[1]);


        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }

    public void SetPosition(Vector2 pos)
    {
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            GetHurt(1);
        } else
        {
            if (collision.collider.CompareTag("Boss"))
            {
                GetHurt(2);
            }
        }
    }
    
    void ReloadGame(){
        print("sadf");
        SceneManager.LoadScene("Menu");
    }

    public void AddPoints(int p)
    {
        points += p;
    }
}
