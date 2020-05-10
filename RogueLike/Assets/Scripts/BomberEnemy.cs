using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberEnemy : Enemy
{
    public GameObject bombPrefab;

    private int direction; //0 up, 1 down, 2 left, 3 right
    private float a;

    private bool placingBomb;
    private float fireRate;
    private float timePlacing;

    private bool move;

    private Transform bombHolder; //variable to store references to the transform of our Board to keep the hierarchy clean

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        damage = 3.0f;
        hp = 5.0f;
        direction = (int)Random.Range(0, 3.99f);
        a = 0;
        placingBomb = false;
        fireRate = 5f;
        timePlacing = 3f;
        move = true;

        bombHolder = new GameObject("BomberEnemy").transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (!placingBomb)
            StartCoroutine(Attack());
    }

    private void Move()
    {
        Vector3 velocity = new Vector3(0, 0, 0);
        if (move)
        {

            int newDirection;
            bool repetir;

            if (Random.value < 0.001)
                do
                {
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
                if (direction == 2) direction = (int)Random.Range(0, 1.99f);
                else direction = 3;
            else if (transform.position.x % 16 > 13) //if it is too close to the right margin, dont go right
                if (direction == 3) direction = (int)Random.Range(0, 1.99f);
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
                    direction = (int)Random.Range(0, 3.99f);
                    break;
            }
        }

        rb.velocity = velocity;

        transform.eulerAngles =  new Vector3(0, 0, a * 360);
        a++;
    }

    private IEnumerator Attack()
    {
        placingBomb = true;
        move = false;
        yield return new WaitForSeconds(timePlacing);
        StartCoroutine(PlaceBomb());
    }

    private IEnumerator PlaceBomb()
    {
        Vector3 pos = transform.position;
        pos.z = 0.5f;
        GameObject bomb = Instantiate(bombPrefab, pos, Quaternion.identity);
        bomb.GetComponent<BombController>().SetDamage(damage);
        bomb.tag = "EnemyBomb";
        bomb.transform.SetParent(bombHolder);

        move = true;

        yield return new WaitForSeconds(fireRate);

        placingBomb = false;
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
