using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPlayerSimulation : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform bunker;
    [SerializeField] private Transform targetPoint1;
    [SerializeField] private Transform targetPoint2;
    [SerializeField] private Transform targetPoint3;

    private void Start()
    {
        playerController.UsedRifle();
        StartCoroutine(StandToCrouch());
    }

    private void OnEnable()
    {
        GameManager.OnExplode += OnExplode;
    }

    private IEnumerator StandToCrouch()
    {
        playerController.StandToCrouch();

        yield return new WaitForSeconds(0.933f);

        StartCoroutine(WalkToPointCrouch());
    }

    private IEnumerator WalkToPointCrouch()
    {
        playerController.WalkToPointCrouch(targetPoint1.position);

        while(playerController.waiting)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(WalkToPoint2Crouch());
    }

    private IEnumerator WalkToPoint2Crouch()
    {
        playerController.WalkToPointCrouch(targetPoint2.position);

        while(playerController.waiting)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        playerController.RotateToTarget(bunker);
        StartCoroutine(ThrowGrenade());
    }

    private IEnumerator ThrowGrenade()
    {
        playerController.ThrowGrenade();

        yield return new WaitForSeconds(25f);

        StartCoroutine(CrouchToProne());
    }

    private IEnumerator CrouchToProne()
    {
        playerController.CrouchToProne();

        yield return null;
    }

    private void OnExplode()
    {
        StartCoroutine(StopProneFire());
    }

    private IEnumerator StopProneFire()
    {
        playerController.StopProneFire();

        yield return new WaitForSeconds(1);

        StartCoroutine(ProneToCrouch());
    }

    private IEnumerator ProneToCrouch()
    {
        playerController.ProneToCrouch();

        yield return new WaitForSeconds(1.967f);

        StartCoroutine(CrouchToStand());
    }

    private IEnumerator CrouchToStand()
    {
        playerController.CrouchToStand();

        yield return new WaitForSeconds(1.233f);

        StartCoroutine(RunToBunker());
    }

    private IEnumerator RunToBunker()
    {
        playerController.RunToPointStand(targetPoint3.position);

        while(playerController.waiting)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        playerController.RotateToTarget(bunker);
        playerController.StandFire(5);
    }

    private void OnDisable()
    {
        GameManager.OnExplode -= OnExplode;
    }
}
