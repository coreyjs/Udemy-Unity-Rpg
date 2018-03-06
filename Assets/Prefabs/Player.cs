using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float maxHealthPoints = 100f;
    private float currentHealthPoints = 100f;

    public float healthAsPercentage
    {
        get { return currentHealthPoints /  maxHealthPoints;  }
    }

}
