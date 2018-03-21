using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealthPoints = 100f;
    [SerializeField] private float attackRadius = 4f;

    private float currentHealthPoints = 100f;

    AICharacterControl aiCharacterControl = null;

    private GameObject player = null;

    public float healthAsPercentage
    {
        get { return currentHealthPoints /  maxHealthPoints;  }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        aiCharacterControl = GetComponent<AICharacterControl>();

    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= attackRadius)
        {
            aiCharacterControl.SetTarget(player.transform);
        }
        else
        {
            aiCharacterControl.SetTarget(transform);
        }
    }

}
