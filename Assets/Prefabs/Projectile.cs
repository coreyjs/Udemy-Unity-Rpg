using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

	public float projectileSpeed;

	public float damageCaused { get; set; }

	void OnTriggerEnter(Collider collider)
	{
		Component damagableComponent = collider.gameObject.GetComponent(typeof(IDamageable));
		print("damageabelComponent = " + damagableComponent);
		if(damagableComponent)
		{
			(damagableComponent as IDamageable).TakeDamage(damageCaused);
		}
	}

}
