using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : Enemy
{
    private NavMeshAgent agent;

    private int attackRate;

    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
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

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().GetHurt(damage);

        yield return new WaitForSeconds(attackRate);
    }
}
