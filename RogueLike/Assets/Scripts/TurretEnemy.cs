using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : Enemy
{

    public GameObject bulletPrefab;

    private Transform bulletsHolder; //variable to store references to the transform of our Board to keep the hierarchy clean

    private bool isShooting;

    private float fireRate = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        damage = 0.5f;
        isShooting = false;

        bulletsHolder = new GameObject("TurretBullets").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShooting)
        {
            isShooting = true;
            StartCoroutine(Shoot());
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(fireRate);
        if (Random.value < 0.5)
        {
            Vector3 destiny = GameObject.FindGameObjectWithTag("Player").transform.position;

            Vector3 pos = transform.position;
            Vector3 direction = destiny - pos;
            float angle = Mathf.Atan(direction.y / direction.x);

            if (direction.x < 0)
            {
                angle += Mathf.PI;
            }

            pos.x += Mathf.Cos(angle) * 1.2f;
            pos.y += Mathf.Sin(angle) * 1.2f;
            Quaternion q = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI);

            Vector3 vel = new Vector3(5 * Mathf.Cos(angle), 5 * Mathf.Sin(angle), 0);

            GameObject bullet = Instantiate(bulletPrefab, pos, q);
            bullet.GetComponent<Rigidbody2D>().velocity = vel;
            bullet.GetComponent<BulletController>().SetDamage(damage);
            bullet.tag = "EnemyBullet";
            bullet.transform.SetParent(bulletsHolder);

            isShooting = false;
        }
    }
}
