﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bacteria"))
        {
            Destroy(bullet);

            if (other.gameObject.GetComponent<BaseController>().armor > 0)
            {
                other.gameObject.GetComponent<BaseController>().armor -= 10f;
            }
            else if (other.gameObject.GetComponent<BaseController>().armor <= 0)
            {
                other.gameObject.GetComponent<BaseController>().health -= 5f;
            }
            
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
