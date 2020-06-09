﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.AI;


public class GBaseController : NewBaseController
{
    public bool travelling;
    private NavMeshAgent navMeshAgent;

    protected override void Start()
    {
        base.Start();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

    }

    protected override void Update()
    {
        base.Update();
        if (navMeshAgent == null)
        {
            Debug.LogError("nav mesh component is not attached");
        }
        else
        {
            DestinationF();
        }

        if (travelling && navMeshAgent.isStopped == true)
        {
            travelling = false;
        }
        if (travelling == false)
        {
            
        }
    }

    private void DestinationF()
    {
        float closestRange = Mathf.Infinity;
        GameObject closestFood = null;

        //health Green
        if (health < 50)
        {
            GameObject[] hFood = GameObject.FindGameObjectsWithTag("Health");

            for (var i = 0; i < hFood.Length; i++)
            {
                float dist = Vector3.Distance(player.transform.position, hFood[i].transform.position);
                if (dist < closestRange)
                {
                    closestRange = dist;
                    closestFood = hFood[i];
                }
                Vector3 targetVector = closestFood.transform.position;
                navMeshAgent.SetDestination(targetVector);
                travelling = true;
            }
        }
        //MetaPlus Red
        if (health >= 70 && metabolism < 8 && energy > 60)
        {
            GameObject[] mFood = GameObject.FindGameObjectsWithTag("MetaPlus");

            for (var i = 0; i < mFood.Length; i++)
            {
                float dist = Vector3.Distance(player.transform.position, mFood[i].transform.position);
                if (dist < closestRange)
                {
                    closestRange = dist;
                    closestFood = mFood[i];
                }
                Vector3 targetVector = closestFood.transform.position;
                navMeshAgent.SetDestination(targetVector);
                travelling = true;
            }
        }

        //MetaMin Blue
        if (health >= 70 && metabolism >= 12 && energy > 60)
        {
            GameObject[] m2Food = GameObject.FindGameObjectsWithTag("MetaMin");

            for (var i = 0; i < m2Food.Length; i++)
            {
                float dist = Vector3.Distance(player.transform.position, m2Food[i].transform.position);
                if (dist < closestRange)
                {
                    closestRange = dist;
                    closestFood = m2Food[i];
                }
                Vector3 targetVector = closestFood.transform.position;
                navMeshAgent.SetDestination(targetVector);
                travelling = true;
            }
        }

        //Energy Yellow
        if (health >= 70 && metabolism < 12 && energy < 60)
        {
            GameObject[] eFood = GameObject.FindGameObjectsWithTag("Energy");

            for (var i = 0; i < eFood.Length; i++)
            {
                float dist = Vector3.Distance(player.transform.position, eFood[i].transform.position);
                if (dist < closestRange)
                {
                    closestRange = dist;
                    closestFood = eFood[i];
                }
                Vector3 targetVector = closestFood.transform.position;
                navMeshAgent.SetDestination(targetVector);
                travelling = true;
            }
        }

        //ammo Pink
        if (ammo < 20)
        {
            GameObject[] amFood = GameObject.FindGameObjectsWithTag("Ammo");

            for (var i = 0; i < amFood.Length; i++)
            {
                float dist = Vector3.Distance(player.transform.position, amFood[i].transform.position);
                if (dist < closestRange)
                {
                    closestRange = dist;
                    closestFood = amFood[i];
                }
                Vector3 targetVector = closestFood.transform.position;
                navMeshAgent.SetDestination(targetVector);
                travelling = true;
            }
        }

        //armor Light blue
        if (armor <= 30)
        {
            GameObject[] aFood = GameObject.FindGameObjectsWithTag("Armor");

            for (var i = 0; i < aFood.Length; i++)
            {
                float dist = Vector3.Distance(player.transform.position, aFood[i].transform.position);
                if (dist < closestRange)
                {
                    closestRange = dist;
                    closestFood = aFood[i];
                }
                Vector3 targetVector = closestFood.transform.position;
                navMeshAgent.SetDestination(targetVector);
                travelling = true;
            }
        }
    }
    
}