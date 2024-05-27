using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public static event Action<Transform> OnGrenadeExplode;
    [SerializeField] private GameManager gameManager;
    private bool exploded;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!exploded)
        {
            exploded = true;
            OnGrenadeExplode?.Invoke(transform);
            gameManager.OnGrenadeExplode(transform);
        }
    }
}
