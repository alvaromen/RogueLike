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
        Vector3 velocity = new Vector3(0, 0, 0);

        int newDirection;
        bool repetir;

        if (Random.value < 0.001)
            do {
                newDirection = (int)Random.Range(0, 3.99f);
                if ((direction == newDirection) || (direction == 0 && newDirection == 1) || (direction == 1 && newDirection == 0) || (direction == 2 && newDirection == 3) || (direction == 3 && newDirection == 2))
                    repetir = true;
                else
                {
                    direction = newDirection;
                    repetir = false;
                }
            } while (repetir);

        if (transform.position.x % 16 < 3) //if it is too close to the left margin, dont go left
            if (direction == 2) direction = (int) Random.Range(0, 1.99f);
            else direction = 3;
        else if (transform.position.x % 16 > 13) //if it is too close to the right margin, dont go right
            if (direction == 3) direction = (int) Random.Range(0, 1.99f);
            else direction = 2;

        if (transform.position.y % 16 < 3) //if it is too close to the up margin, dont go up
            if (direction == 1) direction = (int)Random.Range(2, 3.99f);
            else direction = 0;
        else if (transform.position.y % 16 > 13) //if it is too close to the right margin, dont go down
            if (direction == 0) direction = (int)Random.Range(2, 3.99f);
            else direction = 1;

        switch (direction)
        {
            case 0: //up
                transform.eulerAngles = new Vector3(0, 0, 0);
                velocity.y += 3;
                break;

            case 1: //down
                transform.eulerAngles = new Vector3(0, 0, 180);
                velocity.y -= 3;
                break;

            case 2: //left
                transform.eulerAngles = new Vector3(0, 0, 90);
                velocity.x -= 3;
                break;

            case 3: //right
                transform.eulerAngles = new Vector3(0, 0, -90);
                velocity.x += 3;
                break;

            default:
                direction = (int)Random.Range(0, 3.99f);
                break;
        }

        if(Random.value < 0.001)
        {
            velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }

        rb.velocity = velocity;

    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(fireRate);

        List<Quaternion> q = new List<Quaternion>();
        List<Vector3> pos = new List<Vector3>();
        List<Vector3> vel = new List<Vector3>();
        List<GameObject> bullets = new List<GameObject>();

        //bullet 0: up
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(0, 1, 0));
        vel.Add(new Vector3(0, 5, 0));

        //bullet1: up-left
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(-1, 1, 0));
        vel.Add(new Vector3(-5, 5, 0));

        //bullet2: left
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(-1, 1, 0));
        vel.Add(new Vector3(-5, 0, 0));

        //bullet3: down-left
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(-1, -1, 0));
        vel.Add(new Vector3(-5, -5, 0));

        //bullet4: down
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(0, -1, 0));
        vel.Add(new Vector3(0, -5, 0));

        //bullet5: down-right
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(1, -1, 0));
        vel.Add(new Vector3(5, -5, 0));

        //bullet6: right
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(1, 0, 0));
        vel.Add(new Vector3(5, 0, 0));

        //bullet7: up-right
        q.Add(Quaternion.identity);
        pos.Add(new Vector3(1, 1, 0));
        vel.Add(new Vector3(5, 5, 0));

        for (int i = 0; i < 8; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + pos[i], q[i]);
            bullet.GetComponent<Rigidbody2D>().velocity = vel[i];
            bullet.GetComponent<BulletController>().SetDamage(damage);
            bullet.tag = "EnemyBullet";
            bullet.transform.SetParent(bulletsHolder);
            bullets.Add(bullet);
        }

        isShooting = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            switch (direction)
            {
                case 0:
                    direction = (int)Random.Range(2f, 3.99f);
                    break;
                case 1:
                    direction = (int)Random.Range(2f, 3.99f);
                    break;
                case 2:
                    direction = (int)Random.Range(0f, 1.99f);
                    break;
                case 3:
                    direction = (int)Random.Range(0f, 1.99f);
                    break;
                default:
                    direction = (int)Random.Range(0f, 3.99f);
                    break;
            }
        }
    }
}
