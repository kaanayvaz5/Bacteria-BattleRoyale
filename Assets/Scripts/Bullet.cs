using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bullet;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bacteria") && other.gameObject.GetComponent<BaseController>().bullet.name != gameObject.name)
        {
            Destroy(bullet);

            if (other.gameObject.GetComponent<BaseController>().armor > 0)
            {
                other.gameObject.GetComponent<BaseController>().armor -= 10f;
            }
            else if (other.gameObject.GetComponent<BaseController>().armor <= 0)
            {
                other.gameObject.GetComponent<BaseController>().health -= 10f;
            }
            
        }
        
    }
}
