using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class FlashLight : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private InputActionReference activate;
    [SerializeField] private GameObject fLProjectile;
    [SerializeField] private float launchForce;
    [SerializeField] private Light flashLight;
    [SerializeField] LayerMask layerMask;

    private Rigidbody projectileRb;

    private void Start()
    {
        //Pass Input Action Data to Shoot()
        activate.action.performed += Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        //Reference and Spawn a projectile, get its Rigidbody, Add Forward Force to it
        if (!gameManager.gameOver)
        {
            GameObject launchedProjectile = Instantiate(fLProjectile, transform.position, Quaternion.identity);
            projectileRb = launchedProjectile.GetComponent<Rigidbody>();
            projectileRb.AddForce(transform.forward * launchForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        //Update Flashlight Color with Health Color
        flashLight.color = gameManager.healthColor;
    }
}
