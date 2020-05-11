using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy : Enemy
{
    public GameObject bulletPrefab;
    public GameObject bombPrefab;

    private Transform bulletsHolder;
    private float fireRate;
    private float bulletsFrequency;
    private bool isShooting;
    private bool bulletAttack;
    private bool doingBulletAttack;
    private int nBullets;
    private bool doingBombAttack;
    private int nBombs;

    private int direction;

    private bool activated;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        bulletsHolder = new GameObject("BossBullets").transform;

        hp = 100;
        damage = 2;

        fireRate = 0.1f;
        bulletsFrequency = 3;
        nBullets = 0;

        bulletAttack = false;
        doingBulletAttack = false;

        doingBombAttack = false;

        activated = false;

        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            RangeAttack();
            Move();
        }
    }

    private void RangeAttack()
    {
        if (!bulletAttack)
        {
            if (!doingBulletAttack)
            {
                doingBulletAttack = true;
            }
            else
            {
                if (nBullets >= 8)
                {
                    bulletAttack = true;
                    StartCoroutine(ChargingBulletAttack());
                }
                else
                {
                    if (!isShooting)
                    {
                        nBullets++;
                        isShooting = true;
                        StartCoroutine(GenerateBullet());
                    }
                }
            }
        }
    }

    private void Move()
    {
        Vector3 position = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 distance = position - transform.position;
        switch (direction)
        {
            case 0: //up
            case 1: //down
                if (distance.x < 0)
                {
                    rb.velocity = new Vector3(-3, 0);
                }
                else
                {
                    if (distance.x > 0)
                    {
                        rb.velocity = new Vector3(3, 0);
                    }
                    else rb.velocity = new Vector3(0, 0);
                }
                break;
            case 2: //left
            case 3: //right
                if (distance.y < 0)
                {
                    rb.velocity = new Vector3(0, -3);
                }
                else
                {
                    if (distance.y > 0)
                    {
                        rb.velocity = new Vector3(0, 3);
                    }
                    else rb.velocity = new Vector3(0, 0);
                }
                break;
            default:
                break;
        }
    }

    private IEnumerator ChargingBulletAttack()
    {
        doingBombAttack = false;

        yield return new WaitForSeconds(bulletsFrequency);
        doingBombAttack = false;
        nBullets = 0;
        bulletAttack = false;
        doingBulletAttack = false;
    }

    private IEnumerator GenerateBullet()
    {

        Vector3 center = GetComponent<SpriteRenderer>().bounds.center;
        Vector3 extent = GetComponent<SpriteRenderer>().bounds.extents;
        Vector3 position = new Vector3(0, 0, 0);
        Vector3 velocity = new Vector3(0, 0, 0);
        Vector3 offset = new Vector3(0, 0, 0);

        switch (direction)
        {
            case 0: //up
                offset.x = -1;
                position = new Vector3(center.x - extent.x + nBullets, center.y + extent.y + 0.5f, 0);
                velocity = new Vector3(0, 7, 0);
                break;
            case 1: //down
                offset.x = -1;
                position = new Vector3(center.x - extent.x + nBullets, center.y - extent.y - 0.5f, 0);
                velocity = new Vector3(0, -7, 0);
                break;
            case 2: //left
                offset.y = -1;
                position = new Vector3(center.x - extent.x - 0.5f, center.y - extent.y + nBullets, 0);
                velocity = new Vector3(-7, 0, 0);
                break;
            case 3: //right
                offset.y = -1;
                position = new Vector3(center.x + extent.x + 0.5f, center.y - extent.y + nBullets, 0);
                velocity = new Vector3(7, 0, 0);
                break;
            default:
                break;
        }

        GameObject bullet = Instantiate(bulletPrefab, offset + position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = velocity;
        bullet.GetComponent<BulletController>().SetDamage(damage);
        bullet.tag = "EnemyBullet";
        bullet.transform.SetParent(bulletsHolder);

        yield return new WaitForSeconds(fireRate);

        isShooting = false;
    }

    public void SetDirection(int d)
    {
        direction = d;
    }

    public new void GetHurt(float dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            room.EnemyDown();
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().AddPoints(5000);
            Instantiate(enemyExplosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            audioSource.PlayOneShot(explosionClips[Random.Range(0, explosionClips.Length)]);
            Invoke("ReloadGame", 3f);
        }
    }

    public void Activate()
    {
        activated = true;
    }

    void ReloadGame(){
        SceneManager.LoadScene("Rogue Like");
    }

}
