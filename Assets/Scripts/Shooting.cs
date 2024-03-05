// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class Shotting : MonoBehaviour
{
    [Header("References")]
    public Transform shootPoint;
    public GameObject projectilePrefab;

    [Header("Stats")]
    [Tooltip("Time in seconds between the firing of each projectile")]
    public float fireRate = 1;
    private float lastFireTime = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastFireTime + fireRate) {
            lastFireTime = Time.time;
            var projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            projectile.transform.parent = gameObject.transform;
        }
    }
}
