using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlacedExplosive;
    [SerializeField] private GameManager gameManager;

    [Header("Player Movement")]
    public float playerRollSpeed = 1f;
    public float playerSpeed = 2f;
    public float playerCrouchWalkSpeed = 4f;
    public float playerStandRunSpeed = 8f;
    public float horizontalAxis;
    public float verticalAxis;
    public bool fire;
    public bool waiting;

    [Header("Player Audio")]
    public AudioSource audioSource;
    public AudioClip footStepSound;
    public AudioClip gunEquipSound;
    private float nextFootstep = 0;
    private float footStepDelay = 0.55f;

    [Header("Player Weapon")]
    public Rifle rifle;
    public ExplosiveWeapon grenade;
    public List<GameObject> listWeaponUsed;
    public List<GameObject> listWeaponUnused;

    [Header("Player Animator and Gravity")]
    public CharacterController characterController;
    private float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jumping and Velocity")]
    private float turnCalmTime = 0.1f;
    private float turnCalmVelocity;
    private Vector3 velocity;
    public Transform surfaceCheck;
    public bool onSurface;
    private float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    private void Awake()
    {
        HideAllWeapon();
    }

    private void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if(onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    public void ProneRoll(Vector3 targetPoint)
    {
        StartCoroutine(ProneRollCoroutine(targetPoint));
    }

    private IEnumerator ProneRollCoroutine(Vector3 targetPoint)
    {
        fire = false;
        animator.SetBool("rifle prone fire", false);

        yield return new WaitForSeconds(1);

        animator.SetTrigger("rifle prone roll");

        waiting = true;
        while(Vector3.Distance(transform.position, targetPoint) > 1)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * playerRollSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, targetPoint) <= 1)
            {
                break;
            }
            yield return null;
        }
        waiting = false;
    }

    public void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }

    public void StandToCrouch()
    {
        animator.SetBool("rifle stand idle", false);
        animator.SetBool("rifle crouch idle", true);
    }

    public void CrouchToStand()
    {
        animator.SetBool("rifle crouch idle", false);
        animator.SetBool("rifle stand idle", true);
    }

    public void CrouchToProne()
    {
        fire = false;
        animator.SetBool("rifle crouch fire", false);
        animator.SetBool("rifle crouch idle", false);
        animator.SetBool("rifle prone idle", true);
    }

    public void ProneToCrouch()
    {
        fire = false;
        animator.SetBool("rifle prone fire", false);
        animator.SetBool("rifle prone idle", false);
        animator.SetBool("rifle crouch idle", true);
    }

    public void ProneFire(float time)
    {
        StartCoroutine(ProneFireCoroutine(time));
    }

    public void StopProneFire()
    {
        fire = false;
        animator.SetBool("rifle prone fire", false);
        animator.SetBool("rifle prone idle", true);
    }

    public void CrouchFire(float time)
    {
        StartCoroutine(CrouchFireCoroutine(time));
    }

    public void StandFire(float time)
    {
        StartCoroutine(StandFireCoroutine(time));
    }

    public void ThrowGrenade()
    {
        fire = false;
        UsedGrenade();
        grenade.ThrowGrenade();
    }

    public void PlaceExplosive()
    {
        UsedRifle();
        OnPlacedExplosive?.Invoke();
        gameManager.OnPlacedExplosive();
    }

    public IEnumerator StandFireCoroutine(float time)
    {
        waiting = true;
        fire = true;
        animator.SetBool("rifle stand fire", true);

        int amounts = 30;
        float timeDelta = 0;
        while(time > 0 && fire)
        {
            time -= Time.deltaTime;
            timeDelta += Time.deltaTime;
            if(timeDelta > 0.1f)
            {
                timeDelta = 0;
                rifle.Shoot();
                amounts --;
            }

            if(amounts <= 0)
            {
                rifle.Reload(0);
                yield return new WaitForSeconds(3.3f);
                amounts = 30;
                time -= 3.3f;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        animator.SetBool("rifle stand fire", false);
        waiting = false;
        fire = false;
    }

    public IEnumerator CrouchFireCoroutine(float time)
    {
        waiting = true;
        fire = true;
        animator.SetBool("rifle crouch fire", true);

        int amounts = 30;
        float timeDelta = 0;
        while(time > 0 && fire)
        {
            time -= Time.deltaTime;
            timeDelta += Time.deltaTime;
            if(timeDelta > 0.1f)
            {
                timeDelta = 0;
                rifle.Shoot();
                amounts --;
            }

            if(amounts <= 0)
            {
                rifle.Reload(1);
                yield return new WaitForSeconds(6.867f);
                amounts = 30;
                time -= 6.867f;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        animator.SetBool("rifle crouch fire", false);
        waiting = false;
        fire = false;
    }

    public IEnumerator ProneFireCoroutine(float time)
    {
        waiting = true;
        fire = true;
        animator.SetBool("rifle prone fire", true);

        int amounts = 30;
        float timeDelta = 0;
        while(time > 0 && fire)
        {
            time -= Time.deltaTime;
            timeDelta += Time.deltaTime;
            if(timeDelta > 0.1f)
            {
                timeDelta = 0;
                rifle.Shoot();
                amounts --;
            }

            if(amounts <= 0)
            {
                rifle.Reload(2);
                yield return new WaitForSeconds(6.4f);
                amounts = 30;
                time -= 6.4f;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        animator.SetBool("rifle prone fire", false);
        waiting = false;
        fire = false;
    }

    public void WalkToPointCrouch(Vector3 targetPoint)
    {
        StartCoroutine(WalkToPointCrouchCoroutine(targetPoint));
    }

    public void RunToPointStand(Vector3 targetPoint)
    {
        StartCoroutine(RunToPointStandCoroutine(targetPoint));
    }

    public IEnumerator RunToPointStandCoroutine(Vector3 targetPoint)
    {
        waiting = true;
        animator.SetBool("rifle stand run", true);

        while(Vector3.Distance(transform.position, targetPoint) > 1)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * playerStandRunSpeed * Time.deltaTime);

            // Sound
            footStepDelay = 0.4f;
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0) 
            {
                audioSource.PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
            }

            if(Vector3.Distance(transform.position, targetPoint) <= 1)
            {
                break;
            }
            yield return null;
        }

        animator.SetBool("rifle stand run", false);
        waiting = false;
    }

    public IEnumerator WalkToPointCrouchCoroutine(Vector3 targetPoint)
    {
        waiting = true;
        animator.SetBool("rifle crouch walk fast", true);
        yield return new WaitForSeconds(1f);

        while(Vector3.Distance(transform.position, targetPoint) > 1)
        {
            Vector3 direction = (targetPoint - transform.position).normalized;

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            characterController.Move(moveDirection.normalized * playerCrouchWalkSpeed * Time.deltaTime);

            // Sound
            footStepDelay = 0.8f;
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0) 
            {
                audioSource.PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
            }

            if(Vector3.Distance(transform.position, targetPoint) <= 1)
            {
                break;
            }
            yield return null;
        }

        animator.SetBool("rifle crouch walk fast", false);
        waiting = false;
    }

    private void HideAllWeapon()
    {
        foreach(GameObject weapon in listWeaponUsed)
        {
            weapon.SetActive(false);
        }
        foreach(GameObject weapon in listWeaponUnused)
        {
            weapon.SetActive(false);
        }
    }

    public void UnusedRifle()
    {
        listWeaponUsed[1].SetActive(false);
        listWeaponUnused[1].SetActive(true);
        audioSource.PlayOneShot(gunEquipSound);
    }

    public void UsedRifle()
    {
        listWeaponUsed[1].SetActive(true);
        listWeaponUnused[1].SetActive(false);
        UnusedGrenade();
        UnusedExplosive();
    }

    public void UsedGrenade()
    {
        listWeaponUsed[2].SetActive(true);
        UnusedRifle();
        UnusedExplosive();
    }

    public void UnusedGrenade()
    {
        listWeaponUsed[2].SetActive(false);
        audioSource.PlayOneShot(gunEquipSound);
    }

    public void UsedExplosive()
    {
        listWeaponUsed[3].SetActive(true);
        UnusedRifle();
        UnusedGrenade();
    }

    public void UnusedExplosive()
    {
        listWeaponUsed[3].SetActive(false);
        audioSource.PlayOneShot(gunEquipSound);
    }
}
