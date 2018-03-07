using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private float maxHealthPoints = 100f;
    private float currentHealthPoints = 100f;

    public float healthAsPercentage
    {
        get { return currentHealthPoints /  maxHealthPoints;  }
    }

}
