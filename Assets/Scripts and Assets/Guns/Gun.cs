using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.Netcode;
using System;

public class Gun : NetworkBehaviour
{
    public float damage = 10f;
    public float fireRate = 15f;

    public GameObject projectile;
    public Transform firePoint;
    public float projectileSpeed = 30;

    private Vector3 destination;

    [SerializeField] private Transform InistialTransform;

    public int maxAmmo = 15;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public Camera fpsCam;
    public GameObject impactEffect;

    public Animator animator;

    private float nextTimeToFire = 0f;

    private void Start()
    {
        currentAmmo = maxAmmo;
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
     
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && IsOwner)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && IsOwner)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            if (currentAmmo > -1)
                {
                    Shoot();
                }
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading...");

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(1f);


        currentAmmo = maxAmmo;
        isReloading = false;
    }

    void Shoot()
    {
        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            ShootProjectile();

            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }

    void ShootProjectile()
    {
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            destination = hit.point;
            
        } else
        {
            destination = ray.GetPoint(1000);
        }

        SpawnBulletServerRPC(InistialTransform.position, InistialTransform.rotation);
    }

    [ServerRpc]
    private void SpawnBulletServerRPC (Vector3 position, Quaternion rotation, ServerRpcParams serverRpcParams = default)
    {
        GameObject InstantiatedBullet = Instantiate(projectile, position, rotation);

        InstantiatedBullet.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);

        Destroy(InstantiatedBullet, 2f);
    }
}