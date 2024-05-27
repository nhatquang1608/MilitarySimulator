using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static event Action OnExplode;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button homeButton;
    [SerializeField] private GameObject explosive;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject grenadeEffect;
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject[] listCameras;

    private void Awake()
    {
        replayButton.onClick.AddListener(Replay);
        homeButton.onClick.AddListener(BackToHome);
        HideAllCamera();
        listCameras[3].SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            HideAllCamera();
            listCameras[0].SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            HideAllCamera();
            listCameras[1].SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            HideAllCamera();
            listCameras[2].SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha4))
        {
            HideAllCamera();
            listCameras[3].SetActive(true);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            HideAllCamera();
            listCameras[4].SetActive(true);
        }

        if(Input.GetMouseButtonDown(0))
        {
            DetectObject();
        }
    }

    private void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BackToHome()
    {
        SceneManager.LoadScene("TopScene");
    }

    private void DetectObject()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "RightPlayer")
            {
                HideAllCamera();
                listCameras[0].SetActive(true);
            }
            else if(hit.collider.tag == "CenterPlayer")
            {
                HideAllCamera();
                listCameras[1].SetActive(true);
            }
            else if(hit.collider.tag == "LeftPlayer")
            {
                HideAllCamera();
                listCameras[2].SetActive(true);
            }
            else if(hit.collider.tag == "Bunker")
            {
                HideAllCamera();
                listCameras[4].SetActive(true);
            }
        }
    }

    private void HideAllCamera()
    {
        foreach(GameObject camera in listCameras)
        {
            camera.SetActive(false);
        }
    }

    public void OnPlacedExplosive()
    {
        explosive.SetActive(true);
        StartCoroutine(CountDown());
    }

    public void OnGrenadeExplode(Transform grenade)
    {
        GameObject explosion = Instantiate(grenadeEffect, grenade.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundManager.Instance.grenadeSound);
        Destroy(grenade.gameObject);
        Destroy(explosion, 3f);
    }

    private IEnumerator CountDown()
    {
        yield return new WaitForSeconds(15);

        GameObject explosion = Instantiate(explosionEffect, explosive.transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundManager.Instance.explosiveSound);
        explosive.SetActive(false);
        Destroy(explosion, 3f);

        OnExplode?.Invoke();
    }
}
