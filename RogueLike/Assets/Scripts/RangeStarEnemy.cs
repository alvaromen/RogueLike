using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeStarEnemy : Enemy
{
    private NavMeshAgent agent;

    public GameObject bulletPrefab;

    private int direction; //0 up, 1 down, 2 left, 3 right

    private bool isShooting;
    private float fireRate;

    private Transform bulletsHolder; //variable to store references to the transform of our Board to keep the hierarchy clean

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        damage = 1.0f;
        hp = 3.0f;
        direction = (int)Random.Range(0, 3.99f);
        isShooting = false;
        fireRate = 3f;

        bulletsHolder = new GameObject("RangeStarEnemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShooting) {
            isShooting = true;
            StartCoroutine(Shoot());
        }
        if (Random.value < 0.25)
        {
            Vector3 velocity = new Vector3(0, 0, 0);

            switch (direction)
            {
                case 0: //up
                    velocity.y += 3;
                    break;
                case 1: //down
                    velocity.y -= 3;
                    break;
                case 2: //left
                    velocity.x -= 3;
                    break;
                case 3: //right
                    velocity.x += 3;
                    break;
                default:
                    break;
            }
            rb.velocity = velocity;

            if(Random.value < 0.1)
            {
                direction = (int) Random.Range(0, 3.99f);
            }
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(fireRate);

        GameObject[] bullets = new GameObject[8];

        //bullet 0: up
        Quaternion q = Quaternion.identity;
        //q[2] = 90;
        Vector3 pos = transform.position;
        pos.y += 1f;
        Vector3 velocity = new Vector3(0, 5, 0);
        bullets[0] = Instantiate(bulletPrefab, pos, q);
        bullets[0].GetComponent<Rigidbody2D>().velocity = velocity;

        //bullet1: up-left
        q = Quaternion.identity;
        //q[2] = 135;
        pos = transform.position;
        pos.x -= 1f;
        pos.y += 1f;
        velocity = new Vector3(-5, 5, 0);
        bullets[1] = Instantiate(bulletPrefab, pos, q);
        bullets[1].GetComponent<Rigidbody2D>().velocity = velocity;

        //bullet2: left
        q = Quaternion.identity;
        //q[1] = 180;
        pos = transform.position;
        pos.x -= 1f;
        velocity = new Vector3(-5, 0, 0);
        bullets[2] = Instantiate(bulletPrefab, pos, q);
        bullets[2].GetComponent<Rigidbody2D>().velocity = velocity;

        //bullet3: down-left
        q = Quaternion.identity;
        //q[2] = 225;
        pos = transform.position;
        pos.x -= 1f;
        pos.y -= 1f;
        velocity = new Vector3(-5, -5, 0);
        bullets[3] = Instantiate(bulletPrefab, pos, q);
        bullets[3].GetComponent<Rigidbody2D>().velocity = velocity;

        //bullet4: down
        q = Quaternion.identity;
        //q[2] = 270;
        pos = transform.position;
        pos.y -= 1f;
        velocity = new Vector3(0, -5, 0);
        bullets[4] = Instantiate(bulletPrefab, pos, q);
        bullets[4].GetComponent<Rigidbody2D>().velocity = velocity;

        //bullet5: down-right
        q = Quaternion.identity;
        //q[2] = 315;
        pos = transform.position;
        pos.x += 1f;
        pos.y -= 1f;
        velocity = new Vector3(5, -5, 0);
        bullets[5] = Instantiate(bulletPrefab, pos, q);
        bullets[5].GetComponent<Rigidbody2D>().velocity = velocity;

        //bullet6: right
        q = Quaternion.identity;
        pos = transform.position;
        pos.x += 1f;
        velocity = new Vector3(5, 0, 0);
        bullets[6] = Instantiate(bulletPrefab, pos, q);
        bullets[6].GetComponent<Rigidbody2D>().velocity = velocity;

        //bullet7: up-right
        q = Quaternion.identity;
        pos = transform.position;
        pos.x += 1f;
        pos.y += 1f;
        velocity = new Vector3(5, 5, 0);
        bullets[7] = Instantiate(bulletPrefab, pos, q);
        bullets[7].GetComponent<Rigidbody2D>().velocity = velocity;

        foreach (GameObject bullet in bullets)
        {
            bullet.GetComponent<BulletController>().SetDamage(damage);
            bullet.tag = "EnemyBullet";
            bullet.transform.SetParent(bulletsHolder);
        }

        isShooting = false;
    }
}
