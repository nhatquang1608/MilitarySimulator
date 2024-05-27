using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveWeapon : MonoBehaviour
{
    private float throwForce = 20f;
    public Transform throwArea;
    public GameObject grenadePrefab;
    public Animator animator;
    public PlayerController playerController;
    public bool isThrow;
    public Grenade grenade;

    public void ThrowGrenade()
    {
        if(!isThrow) StartCoroutine(OnThrow());
    }

    IEnumerator OnThrow()
    {
        isThrow = true;
        animator.SetTrigger("rifle crouch throw");
        yield return new WaitForSeconds(1.8f);
        Throw();
        yield return new WaitForSeconds(0.1f);
        isThrow = false;
        playerController.UsedRifle();
    }

    private void Throw()
    {
        GameObject objectThrow = Instantiate(grenadePrefab, throwArea.position, throwArea.rotation);
        Rigidbody rb = objectThrow.GetComponent<Rigidbody>();
        rb.AddForce(throwArea.forward*throwForce, ForceMode.VelocityChange);
        grenade = objectThrow.GetComponent<Grenade>();
    }
}
