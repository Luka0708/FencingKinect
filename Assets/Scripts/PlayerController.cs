using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject arm;
    public Transform armTransform;
    public GameObject bulletPrefab;
    public int ammo = 3;
    public float reloadTime = 3f;
    public float shootCooldown = 1f;
    public float armLength = 2f;
    public int maxAmmo = 3;

    private float nextShootTime = 0f;
    private float reloadStartTime = 0f;
    private bool isReloading = false;
    private bool canShoot = true;

    public float angle;
    private Collider2D playerCollider;
    private SpriteRenderer playerRenderer;
    private Vector3 armDirection;
    
    public Transform parentTransform;
    private void Start()
    {
        armTransform = arm.transform;
        playerCollider = GetComponent<Collider2D>();
        playerRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Rotate arm towards mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        armDirection = (mousePos - armTransform.position).normalized;
        angle = Mathf.Atan2(armDirection.y, armDirection.x) * Mathf.Rad2Deg;
        float a  = armTransform.rotation.eulerAngles.z;
        parentTransform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
  




        
        if (Input.GetMouseButtonDown(0) && ammo > 0 && !isReloading && canShoot)
        {
            StartCoroutine(Shoot());
        }
        
        if (ammo == 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }
        
        // Duck
        if (Input.GetMouseButtonDown(1))
        {
            playerCollider.enabled = false;
            playerRenderer.color = Color.red;
        }

        if (Input.GetMouseButtonUp(1))
        {
            playerCollider.enabled = true;
            playerRenderer.color = Color.green;
        }
    }
    IEnumerator Shoot()
    {
        canShoot = false;
        ammo--;
        Vector3 bulletPos = armTransform.position + armDirection * armLength;

        var bullet = Instantiate(bulletPrefab, bulletPos, Quaternion.Euler(0, 0, -90 + angle));
        //bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * 10f;
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);
        ammo = maxAmmo;
        isReloading = false;
    }
}