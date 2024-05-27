using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Riffle Things")]
    private float shootingRange = 1000f;

    [Header("Riffle Effects")]
    public ParticleSystem muzzleSpark;
    public GameObject concreteEffect;
    public GameObject metalEffect;
    public GameObject woodEffect;
    public GameObject softbodyEffect;
    public GameObject goreEffect;
    public Animator animator;

    [Header("Riffle Sounds")]
    public AudioSource audioSource;
    public AudioClip reloadSound;
    public AudioClip shootSound;

    public void Shoot()
    {
        muzzleSpark.Play();
        audioSource.PlayOneShot(shootSound);

        RaycastHit hitInfo;

        if(Physics.Raycast(muzzleSpark.transform.position, muzzleSpark.transform.forward, out hitInfo, shootingRange))
        {
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            
            if(objectToHit != null)
            {
                if(objectToHit.objectType == ObjectToHit.ObjectType.Concrete)
                {
                    GameObject concreteGo = Instantiate(concreteEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(concreteGo, 1f);
                }
                if(objectToHit.objectType == ObjectToHit.ObjectType.Metal)
                {
                    GameObject metalGo = Instantiate(metalEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(metalGo, 1f);
                }
                if(objectToHit.objectType == ObjectToHit.ObjectType.Wood)
                {
                    GameObject woodGo = Instantiate(woodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(woodGo, 1f);
                }
                if(objectToHit.objectType == ObjectToHit.ObjectType.Softbody)
                {
                    GameObject softbodyGo = Instantiate(softbodyEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(softbodyGo, 1f);
                }
            }
        }
    }

    public void Reload(int id)
    {
        if(id == 0) animator.SetTrigger("rifle stand reload");
        else if(id == 1) animator.SetTrigger("rifle crouch reload");
        else if(id == 2)animator.SetTrigger("rifle prone reload");
        audioSource.PlayOneShot(reloadSound);
    }
}
