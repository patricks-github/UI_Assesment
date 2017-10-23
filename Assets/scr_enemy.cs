using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_enemy : MonoBehaviour {
    
    public float health = 100.0f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void TakeHit(float damage)
    {
        health -= damage;
        Debug.Log("Damage: " + damage);

        if (health <= 0)
        {
            death();
        }
    }

    public void death()
    {
        GameObject.Destroy(gameObject);
    }
}
