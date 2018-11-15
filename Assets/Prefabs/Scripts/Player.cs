using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{

    [SerializeField] private float maxHealthPoints = 100f;

    [SerializeField] int enemyLayer = 9;
    [SerializeField] float damagePerHit = 10f;
    [SerializeField] float minTimeBetweenHits = 0.5f;
    [SerializeField] float maxAttackRange =2f;
    [SerializeField] Weapon weaponInUse;

    GameObject currentTarget;
    private float currentHealthPoints;

    CameraRaycaster cameraRaycaster;
    float lastHitTime = 0f;

    void Start() 
    {
        RegisterForMouseClick();
        currentHealthPoints = maxHealthPoints;
        PutWeaponInHand();
    }

    private void PutWeaponInHand()
    {
        var weaponPrefab = weaponInUse.GetWeaponPrefab();
        var weapon = Instantiate(weaponPrefab);
        // TODO Move to correct place and child to hand
    }

    private void RegisterForMouseClick()
    {
        cameraRaycaster = FindObjectOfType<CameraRaycaster>();
        cameraRaycaster.notifyMouseClickObservers += OnMouseClicked;
    }

    void OnMouseClicked(RaycastHit raycastHit, int layerHit)
    {
        if(layerHit == enemyLayer)
        {
            var enemy = raycastHit.collider.gameObject;
            currentTarget = enemy;
            //check enemy is in range

            if ((enemy.transform.position - transform.position).magnitude > maxAttackRange)
            {
                return;
            }

            var enemyComponent = enemy.GetComponent<Enemy>();
            if (Time.time - lastHitTime > minTimeBetweenHits)
            {
                enemyComponent.TakeDamage(damagePerHit);
                lastHitTime = Time.time;
            }
            
        }
    }

    public float healthAsPercentage
    {
        get { return currentHealthPoints /  maxHealthPoints;  }
    }

    public void TakeDamage(float damage)
    {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <=0) {
          //  Destroy(gameObject);
        }
    }
}
