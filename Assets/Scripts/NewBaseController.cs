﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;
using UnityEngine.AI;

public class NewBaseController : MonoBehaviour
{
    public float shootForce;
    public float shootRange;
    public bool inRange;
    public Transform player;
    public Transform firePoint;
    //public Transform targetE;
    public Transform[] enemiesList;
    public GameObject bullet;
    public float metabolism;
    public float maxHealth;
    public float health;
    public float energy = 10f;
    public float energyDepletion;
    public float speed = 7f;
    public float armor;
    public float ammo;
    public float scaleModifier = 0.5f;
    public float maxSize = 30f;
    public bool idle = true;
    public Rigidbody rb;
    public Transform target;
    public SphereCollider awarenessSphere;
    public Camera camera;
    private IEnumerator currentMoveCoroutine;
    public GameObject gameManager;
    private float initHealth;
    //public List<FoodType> foodTypes = new List<FoodType>();

    // Start is called before the first frame update
    protected virtual void Start()
    {
        gameManager.GetComponent<CameraManager>().agentCameras.Add(camera);
        rb = GetComponent<Rigidbody>();
        camera = GetComponentInChildren<Camera>();

        inRange = false;
        initHealth = health;
        energyDepletion = (1 / metabolism) * 10;

        StartCoroutine(_loseEnergy());
    }

    protected virtual void Update()
    {
        StartCoroutine(_chooseTarget());
        //GameObject.Find("GameManager").GetComponent<GameManager>().constrainZ(transform);
        energyDepletion = metabolism / 10;
        speed = (metabolism / 2) * scaleModifier;
        maxHealth = (1 / metabolism) * 1000;

        if (currentMoveCoroutine != null)
        {
            if (currentMoveCoroutine == _loseHealth())
            {
                if (energy > energyDepletion)
                {
                    StopCoroutine(currentMoveCoroutine);
                    StartCoroutine(_loseEnergy());
                }
            }
        }


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Food>() != null)
        {
            target = other.transform;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        var otherObj = other.collider.gameObject;
        if (otherObj.GetComponent<Food>() != null)
        {
            var food = otherObj.GetComponent<Food>();
            switch (food.foodType)
            {
                case Food.FoodType.AMMO:

                    ammo += food.foodValue;
                    break;
                case Food.FoodType.ARMOUR:
                    armor += food.foodValue;
                    break;
                case Food.FoodType.ENERGY:
                    energy += food.foodValue;
                    break;
                case Food.FoodType.HEALTH:
                    if (health < maxHealth)
                    {
                        health += food.foodValue;
                        if (metabolism < 10)
                        {
                            var ratio = (health / maxHealth);
                            scaleModifier = (1 / metabolism) * 10 * ratio;
                            gameObject.transform.localScale += new Vector3(scaleModifier, scaleModifier, scaleModifier);

                            if (gameObject.transform.localScale.x < maxSize / 4)
                            {
                                //camera.transform.position += new Vector3(0, 0.5f, 0);
                            }
                        }
                    }
                    break;
                case Food.FoodType.METABOLISM:
                    if (food.negative)
                    {
                        if (metabolism > 1)
                        {
                            metabolism -= food.foodValue;
                        }
                        //gameObject.transform.localScale += new Vector3(scaleModifier, scaleModifier,scaleModifier);
                    }
                    else
                    {
                        if (metabolism < 20)
                        {
                            metabolism += food.foodValue;
                        }
                        //gameObject.transform.localScale = new Vector3(transform.localScale.x - scaleModifier, transform.localScale.y - scaleModifier,transform.localScale.z - scaleModifier);
                    }
                    break;
            }

            Destroy(other.collider.gameObject);
            target = null;
        }
    }

    public IEnumerator _loseEnergy()
    {
        while (energy > 0)
        {
            energy -= energyDepletion;
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Energy 0");
        currentMoveCoroutine = _loseHealth();
        StartCoroutine(currentMoveCoroutine);
    }

    public IEnumerator _loseHealth()
    {
        while (health > 0 && energy < energyDepletion)
        {
            health -= 1f;
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Health 0");
        StartCoroutine(_loseEnergy());
    }

    public void Move(Vector3 target)
    {
        GetComponent<NavMeshAgent>().speed = speed;
        GetComponent<NavMeshAgent>().destination = target;
    }

    public IEnumerator _chooseTarget()
    {
        float shortestRange = Mathf.Infinity;
        Transform closestEnemy = null;

        // for getting all enemy distances
        foreach (Transform enemy in enemiesList)
        {
            if(enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(player.transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestRange)
                {
                    shortestRange = distanceToEnemy;
                    closestEnemy = enemy;
                    //Debug.Log(distanceToEnemy + enemy.gameObject.name);
                }
            }
        }

        if (closestEnemy != null && shortestRange <= shootRange)
        {
            inRange = true;
            target = closestEnemy.transform;
            firePoint.LookAt(target);
            Debug.Log("attacking" + closestEnemy.gameObject.name);
            ShootEnemy();
            yield return new WaitForSeconds(.5f);
        }
        else
        {
            inRange = false;
            target = null;
        }
    }

    public void ShootEnemy()
    {
        if (inRange == true && ammo > 0)
        {
            GameObject shot = Instantiate(bullet, firePoint.position, firePoint.rotation);
            shot.GetComponent<Rigidbody>().AddForce(shot.transform.forward * shootForce);
            Destroy(shot, 1.2f);
            ammo -= 1;
            
        }
    }

    /*private void OnMouseDown()
    {
        camera.gameObject.SetActive(true);
    }*/

}




