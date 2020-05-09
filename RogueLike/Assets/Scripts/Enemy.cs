﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{

    protected Room room;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetRoom(Room r)
    {
        room = r;
    }

    public void GetHurt(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            room.EnemyDown();
            Destroy(gameObject);
        }
    }
}
