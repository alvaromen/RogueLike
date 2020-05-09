using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeStarEnemy : Enemy
{
    public GameObject bulletPrefab;
    
    private int direction; //0 up, 1 down, 2 left, 3 right

    private Transform bulletsHolder; //variable to store references to the transform of our Board to keep the hierarchy clean

    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        hp = 3;
        direction = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (agent.velocity.x == 0 && agent.velocity.y == 0) //when the enemy is stopped, maybe we have to change this condition
        {
            Shoot();
            NavMeshPath path = new NavMeshPath();
            Vector3 destiny = transform.position;
            switch (direction)
            {
                case 0: //up
                    destiny.y += 3;
                    break;
                case 1: //downNavMeshPath path = new NavMeshPath();
                    destiny.y -= 3;
                    break;
                case 2: //left
                    destiny.x -= 3;
                    break;
                case 3: //right
                    destiny.x += 3;
                    break;
                default:
                    break;
            }
            if (agent.CalculatePath(destiny, path))
            {
                agent.SetPath(path);
            }
            else
            {
                while (!agent.CalculatePath(destiny, path))
                {
                    //choose a random destiny
                }
            }
        }
        if(Random.value < 0.25)
        {
            direction = (int)Random.Range(0, 3.99f);

        }
    }

    private void Shoot()
    {
        GameObject[] bullets = new GameObject[8];

        //bullet 0: up
        Quaternion q = Quaternion.identity;
        //q[2] = 90;
        Vector3 pos = transform.position;
        pos.y += 0.5f;
        Vector3 force = new Vector3(0, 10, 0);
        bullets[0] = Instantiate(bulletPrefab, pos, q);
        bullets[0].GetComponent<Rigidbody2D>().AddForce(force);

        //bullet1: up-left
        q = Quaternion.identity;
        //q[2] = 135;
        pos = transform.position;
        pos.x -= 0.5f;
        pos.y += 0.5f;
        force = new Vector3(-10, 10, 0);
        bullets[1] = Instantiate(bulletPrefab, pos, q);
        bullets[1].GetComponent<Rigidbody2D>().AddForce(force);

        //bullet2: left
        q = Quaternion.identity;
        //q[1] = 180;
        pos = transform.position;
        pos.x -= 0.5f;
        force = new Vector3(-10, 0, 0);
        bullets[2] = Instantiate(bulletPrefab, pos, q);
        bullets[2].GetComponent<Rigidbody2D>().AddForce(force);

        //bullet3: down-left
        q = Quaternion.identity;
        //q[2] = 225;
        pos = transform.position;
        pos.x -= 0.5f;
        pos.y -= 0.5f;
        force = new Vector3(-10, -10, 0);
        bullets[3] = Instantiate(bulletPrefab, pos, q);
        bullets[3].GetComponent<Rigidbody2D>().AddForce(force);

        //bullet4: down
        q = Quaternion.identity;
        //q[2] = 270;
        pos = transform.position;
        pos.y -= 0.5f;
        force = new Vector3(0, -10, 0);
        bullets[4] = Instantiate(bulletPrefab, pos, q);
        bullets[4].GetComponent<Rigidbody2D>().AddForce(force);

        //bullet5: down-right
        q = Quaternion.identity;
        //q[2] = 315;
        pos = transform.position;
        pos.x += 0.5f;
        pos.y -= 0.5f;
        force = new Vector3(10, -10, 0);
        bullets[5] = Instantiate(bulletPrefab, pos, q);
        bullets[5].GetComponent<Rigidbody2D>().AddForce(force);

        //bullet6: right
        q = Quaternion.identity;
        pos = transform.position;
        pos.x += 0.5f;
        force = new Vector3(10, 0, 0);
        bullets[6] = Instantiate(bulletPrefab, pos, q);
        bullets[6].GetComponent<Rigidbody2D>().AddForce(force);

        //bullet7: up-right
        q = Quaternion.identity;
        pos = transform.position;
        pos.x += 0.5f;
        pos.y += 0.5f;
        force = new Vector3(10, 10, 0);
        bullets[7] = Instantiate(bulletPrefab, pos, q);
        bullets[7].GetComponent<Rigidbody2D>().AddForce(force);

        foreach (GameObject bullet in bullets)
        {
            bullet.transform.SetParent(bulletsHolder);
            bullet.tag = "EnemyBullet";
        }
    }
}
