using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour
{
    protected int hp;
    protected int damage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetHurt(int dmg)
    {
        hp = dmg;
        if(hp <= 0)
        {
            Destroy(this);
        }
    }
}