using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnemy : Enemy
{

    public GameObject bulletPrefab;

    private Transform bulletsHolder; //variable to store references to the transform of our Board to keep the hierarchy clean

    private bool isShooting;

    private float fireRate = 1.5f;

    private float distance = 3.0f;

    //Atributes for movement
    int ampX = 6;
    int ampY = 4;
    float wX = 0.03f;
    float wY = 0.1f;
    int a = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        damage = 1;
        hp = 3;
        isShooting = false;

        bulletsHolder = new GameObject("EnemyBullets").transform;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!isShooting)
        {

            float dist;

            Vector3 playerPos = player.transform.position;
            Vector3 pos = transform.position;
            Vector3 velocity = new Vector3(0, 0);

            if (playerPos.x < pos.x)
            {
                dist = pos.x - playerPos.x;
                if (dist < distance)
                {
                    velocity.x += 3;
                }
                else
                {
                    if (dist > distance)
                    {
                        velocity.x -= 3;
                    }
                    else velocity.x = wX * ampX * Mathf.Cos(wX * a);
                }
            }
            else
            {
                dist = pos.x - playerPos.x;
                if (dist < distance)
                {
                    velocity.x -= 3;
                }
                else
                {
                    if (dist > distance)
                    {
                        velocity.x += 3;
                    }
                    else velocity.x = wX * ampX * Mathf.Cos(wX * a);
                }
            }
            if (playerPos.y < pos.y)
            {
                dist = pos.y - playerPos.y;
                if (dist < distance)
                {
                    velocity.y += 3;
                }
                else
                {
                    if (dist > distance)
                    {
                        velocity.y -= 3;
                    }
                    else velocity.y = wY * ampY * Mathf.Cos(wY * a);
                }
            } else
            {
                dist = pos.y - playerPos.y;
                if (dist < distance)
                {
                    velocity.y -= 3;
                }
                else
                {
                    if (dist > distance)
                    {
                        velocity.y += 3;
                    }
                    else velocity.y = wY * ampY * Mathf.Cos(wY * a);
                }
            }
            a++;
            rb.velocity = velocity;

            isShooting = true;
            StartCoroutine(Shoot(player.transform.position));
        }
    }

    private IEnumerator Shoot(Vector3 destiny)
    {
        Quaternion q = Quaternion.identity;
        Vector3 pos = transform.position;

        Vector3 direction = transform.position - destiny;
        float angle = Mathf.Atan(direction.x / direction.y);
        pos.x += Mathf.Sin(angle);
        pos.y += Mathf.Cos(angle);
        q[2] = 90 - angle;
        
        Vector3 vel = new Vector3(10 * Mathf.Sin(angle), 10 * Mathf.Cos(angle), 0);

        GameObject bullet = Instantiate(bulletPrefab, pos, q);
        bullet.GetComponent<Rigidbody2D>().velocity = vel;
        bullet.GetComponent<BulletController>().SetDamage(damage);
        bullet.tag = "EnemyBullet";
        bullet.transform.SetParent(bulletsHolder);

        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }
}
