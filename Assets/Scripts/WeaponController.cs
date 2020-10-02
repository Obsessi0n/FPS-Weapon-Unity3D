using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;

    [SerializeField]
    Transform bulletSpawnPoint;
    [SerializeField]
    int magazineCapacity = 30;
    [SerializeField]
    int totalAmmo = 5;

    int bulletsInMagazine;
    [SerializeField]
    float fireRateDelay = 0.1f;
    [SerializeField]
    float reloadTime = 2f;

    bool canShoot = true;

    bool hasAmmo = true;
    [SerializeField]
    bool automaticReload = false;
    [SerializeField]
    bool automaticWeapon = false;

    [SerializeField]
    private Camera mainCamera;


    [SerializeField]
    private Sprite[] concreteDecals;

    [SerializeField]
    private GameObject bulletHolePrefab;

    [SerializeField]
    private GameObject impactParticlePrefab;

    [SerializeField]
    private ParticleSystem muzzleFlash;

    [SerializeField]
    private AudioSource shootSound;

    [SerializeField]
    private UIController uiController;

    //Recoil intensity

    float recoilIntensityCounter = 0;
    float recoilMaxIntensity = 50;

    [SerializeField]
    float recoilXOffset;
    [SerializeField]
    float recoilYOffset;


    [SerializeField]
    bool InvertY = false;

    void Start()
    {        
        bulletsInMagazine = magazineCapacity;
    }

    
    void Update()
    {
        uiController.updateAmmo(bulletsInMagazine, totalAmmo);

        if (Input.GetMouseButton(0) && automaticWeapon)
        {
            if(hasAmmo)
                if (canShoot)
                {
                    Shoot();
                }            
        }
        else if(Input.GetMouseButtonDown(0) && !automaticWeapon)
        {
            if (hasAmmo)
            {
                if (canShoot)
                {
                    Shoot();
                }

            }
        }
        else
        {
            if (recoilIntensityCounter < 0)
                recoilIntensityCounter = 0;
            else
                recoilIntensityCounter--;
        }


        if(Input.GetKeyDown(KeyCode.R))
        {
            if(totalAmmo > 0)
                Reload();
        }
        

    }

    void Shoot()
    {
        StartCoroutine(FireRateDelay());
        Vector3 impactPoint = CastRay();

        muzzleFlash.Play();
        shootSound.Play();

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        if(impactPoint != Vector3.zero)
        {
            bullet.transform.LookAt(impactPoint);
        }

        bulletsInMagazine--;
        if(bulletsInMagazine <= 0)
        {
            hasAmmo = false;
            if (automaticReload)
                if (totalAmmo > 0)
                    Reload();
        }
    }



    void Reload()
    {
        StartCoroutine(ReloadDelay());
    }


    Vector3 CastRay()
    {
   

        RaycastHit hit;
      
        Ray ray = mainCamera.ScreenPointToRay(Recoil());
        if(Physics.Raycast(ray, out hit))
        {
            if(hit.transform != null)
            {
                GameObject bulletHole = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.0001f, Quaternion.identity);
                bulletHole.transform.LookAt(hit.point + hit.normal);
                bulletHole.GetComponent<SpriteRenderer>().sprite = concreteDecals[Random.Range(0, concreteDecals.Length)];
                GameObject impactPrefab = Instantiate(impactParticlePrefab, hit.point + hit.normal * 0.0001f, Quaternion.identity);
                impactPrefab.transform.LookAt(hit.point + hit.normal);
                

                return hit.point;
            }
        }

        Debug.DrawRay(ray.origin, ray.direction * 1000, new Color(1f, 0.922f, 0.015f, 1f));

        return Vector3.zero;

    }

    Vector2 Recoil()
    {
        recoilIntensityCounter++;
        if (recoilIntensityCounter > recoilMaxIntensity)
            recoilIntensityCounter = recoilMaxIntensity;

        float xoffset = Random.Range(0, recoilIntensityCounter);
        float yoffsect = Random.Range(0, recoilIntensityCounter);
        if (Random.Range(0f, 1f) <= 0.5f)
            xoffset = -xoffset;
        if (InvertY)
        {
            if (Random.Range(0f, 1f) <= 0.5f)
                yoffsect = -yoffsect;
        }
   

        float x = (Screen.width / 2) + xoffset*4;
        float y = (Screen.height / 2) + yoffsect*4;

        return new Vector2(x, y);
    }

    IEnumerator FireRateDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(fireRateDelay);
        canShoot = true;
    }

    IEnumerator ReloadDelay()
    {
        canShoot = false;
        yield return new WaitForSeconds(reloadTime);
        if(bulletsInMagazine + totalAmmo <= magazineCapacity)
        {
            bulletsInMagazine += totalAmmo;
            totalAmmo = 0;
        }
        else
        {
            totalAmmo -= magazineCapacity - bulletsInMagazine;
            bulletsInMagazine = magazineCapacity;
        }
        hasAmmo = true;
        canShoot = true;
    }
}
