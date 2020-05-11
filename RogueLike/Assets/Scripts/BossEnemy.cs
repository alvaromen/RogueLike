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

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        bulletsHolder = new GameObject("BossBullets").transform;

        hp = 10;
        damage = 2;

        fireRate = 0.25f;
        bulletsFrequency = 5;
        nBullets = 0;

        bulletAttack = false;
        doingBulletAttack = false;

        doingBombAttack = false;

        audioSource = GameObject.Find("Audio Source").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        RangeAttack();
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
                if (nBullets >= 7)
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

        switch (direction)
        {
            case 0: //up
                position = new Vector3(center.x - extent.x + nBullets, center.y + extent.y + 0.5f, 0);
                velocity = new Vector3(0, 5, 0);
                break;
            case 1: //down
                position = new Vector3(center.x - extent.x + nBullets, center.y - extent.y - 0.5f, 0);
                velocity = new Vector3(0, -5, 0);
                break;
            case 2: //left
                position = new Vector3(center.x - extent.x - 0.5f, center.y - extent.y + nBullets, 0);
                velocity = new Vector3(-5, 0, 0);
                break;
            case 3: //right
                position = new Vector3(center.x - extent.x + 0.5f, center.y - extent.y + nBullets, 0);
                velocity = new Vector3(5, 0, 0);
                break;
            default:
                break;
        }

        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
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

            if(UnityEngine.Random.Range(0f, 1f) < 0.2f){
                int randIndex = UnityEngine.Random.Range(0, mobDrops.Length);
                Instantiate(mobDrops[randIndex], gameObject.transform.position, Quaternion.identity);
            }
            Instantiate(enemyExplosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
            audioSource.PlayOneShot(explosionClips[Random.Range(0, explosionClips.Length)]);
            Invoke("ReloadGame", 3f);
        }
    }

    void ReloadGame(){
        SceneManager.LoadScene("Rogue Like");
    }

}
