using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    public float fireRate = 3;
    public int damage = 20;

    public GameObject laser;

    float timeToFire = 0;
    Transform parentTransform;

    private void Awake()
    {
        parentTransform = GetComponentInParent<Transform>();
    }

    void Update ()
    {
		if(fireRate == 0)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                Debug.Log("Test");
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }
	}

    void Shoot()
    {
        Vector2 bulletSpawn = new Vector2(transform.position.x, transform.position.y);

        GameObject shot = Instantiate(laser, transform.position, transform.rotation);
        shot.GetComponent<Laser>().dmg = damage;
        shot.GetComponent<Laser>().friendly = true;
        GameMaster.instance.playerShootTimeCheck();
    }
}
