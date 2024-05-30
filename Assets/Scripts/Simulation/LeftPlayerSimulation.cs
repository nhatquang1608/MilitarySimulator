using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPlayerSimulation : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Transform bunker;
    [SerializeField] private Transform targetPoint1;
    [SerializeField] private Transform targetPoint2;
    [SerializeField] private Transform targetPoint3;

    private void Start()
    {
        playerController.UsedExplosive();
        StartCoroutine(StandToCrouch());
    }

    private void OnEnable()
    {
        Grenade.OnGrenadeExplode += OnPlaceExplosive;
        GameManager.OnExplode += OnExplode;
    }

    private IEnumerator StandToCrouch()
    {
        playerController.StandToCrouch();

        yield return new WaitForSeconds(2.933f);

        StartCoroutine(WalkToPointCrouch());
    }

    private IEnumerator WalkToPointCrouch()
    {
        playerController.WalkToPointCrouch(targetPoint1.position);

        while(playerController.waiting)
        {
            yield return null;
        }

        playerController.RotateToTarget(bunker);
    }

    private void OnPlaceExplosive(Transform grenade)
    {
        StartCoroutine(CrouchToStand0());
    }

    private IEnumerator CrouchToStand0()
    {
        playerController.CrouchToStand();

        yield return new WaitForSeconds(1.233f);

        StartCoroutine(RunToPoint2());
    }

    private IEnumerator RunToPoint2()
    {
        playerController.RunToPointStand(targetPoint2.position);

        while(playerController.waiting)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        playerController.RotateToTarget(bunker);
        StartCoroutine(StandToCrouch0());
    }

    private IEnumerator StandToCrouch0()
    {
        playerController.StandToCrouch();

        yield return new WaitForSeconds(3.933f);

        StartCoroutine(PlaceExplosive());
    }

    private IEnumerator PlaceExplosive()
    {
        playerController.PlaceExplosive();

        yield return new WaitForSeconds(1);

        StartCoroutine(CrouchToStand());
    }

    private IEnumerator CrouchToStand()
    {
        playerController.CrouchToStand();

        yield return new WaitForSeconds(1.233f);

        StartCoroutine(RunToPoint3());
    }

    private IEnumerator RunToPoint3()
    {
        playerController.RunToPointStand(targetPoint3.position);

        while(playerController.waiting)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        playerController.RotateToTarget(bunker);
        StartCoroutine(StandToCrouch1());
    }

    private IEnumerator StandToCrouch1()
    {
        playerController.StandToCrouch();

        yield return new WaitForSeconds(0.933f);

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

        StartCoroutine(CrouchToStand1());
    }

    private IEnumerator CrouchToStand1()
    {
        playerController.CrouchToStand();

        yield return new WaitForSeconds(1.233f);

        StartCoroutine(RunToBunker());
    }

    private IEnumerator RunToBunker()
    {
        playerController.RunToPointStand(targetPoint2.position);

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
        Grenade.OnGrenadeExplode -= OnPlaceExplosive;
        GameManager.OnExplode -= OnExplode;
    }
}
