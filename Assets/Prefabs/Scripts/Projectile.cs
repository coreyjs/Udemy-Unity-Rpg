using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

	public float projectileSpeed;

	float damageCaused { get; set; }

	public void SetDamage(float damage)
	{
		damageCaused = damage;
	}

	void OnCollisionEnter(Collision collision)
	{
		Component damagableComponent = collision.gameObject.GetComponent(typeof(IDamageable));
		print("damageabelComponent = " + damagableComponent);
		if(damagableComponent)
		{
			(damagableComponent as IDamageable).TakeDamage(damageCaused);
		}

		Destroy(gameObject, 0.1f);
	}

}
