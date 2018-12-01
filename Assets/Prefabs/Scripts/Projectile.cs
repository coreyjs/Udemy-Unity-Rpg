using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{

	[SerializeField] float projectileSpeed;

	[SerializeField] GameObject shooter;  // Can inspect when paused

	public void SetShooter(GameObject shooter){
		this.shooter = shooter;
	}

	float damageCaused { get; set; }

	public void SetDamage(float damage)
	{
		damageCaused = damage;
	}

	public float GetDefaultLaunchSpeed() {
		return projectileSpeed;
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
