using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipEnemy : Enemy
{

    public GameObject bulletPrefab;

    private Transform bulletsHolder; //variable to store references to the transform of our Board to keep the hierarchy clean

    private bool isShooting;

    private float fireRate = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        hp = 3;
        isShooting = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (!isShooting)
        {
            StartCoroutine(Shoot(player.transform.position));
        }
    }

    private IEnumerator Shoot(Vector3 destiny)
    {
        Quaternion q = Quaternion.identity;
        Vector3 pos = transform.position;

        Vector3 direction = transform.position - destiny;
        float angle = Mathf.Atan(direction.x / direction.y);
        pos.x += 0.2f + Mathf.Cos(angle);
        pos.y += 0.2f + Mathf.Sin(angle);
        q[2] = angle;
        
        Vector3 vel = new Vector3(10 * Mathf.Cos(angle), 10 * Mathf.Sin(angle), 0);

        GameObject bullet = Instantiate(bulletPrefab, pos, q);
        bullet.GetComponent<Rigidbody2D>().velocity = vel;
        bullet.transform.SetParent(bulletsHolder);
        bullet.tag = "EnemyBullet";

        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }
}
