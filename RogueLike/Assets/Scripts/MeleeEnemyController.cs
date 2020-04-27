using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : MonoBehaviour
{

    private NavMeshAgent agent;

    private int dmg;
    private int attackRate;
    private int hp;

    // Start is called before the first frame update
    void Start()
    {
        dmg = 1;
        attackRate = 1;
        hp = 4;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        //attack animation

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetHurt(dmg);

        yield return new WaitForSeconds(attackRate);
    }
}
