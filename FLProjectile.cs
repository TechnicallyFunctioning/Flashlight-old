using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLProjectile : MonoBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private Light projectileLight;
    [SerializeField] private ParticleSystem mainParticle;
    [SerializeField] private ParticleSystem lightningParticle;
    [SerializeField] private ParticleSystem glowParticle;
    [SerializeField] private ParticleSystem lightningTrailParticle;
    [SerializeField] private ParticleSystem glowTrailParticle;
    private ParticleSystem.MainModule mainMain;
    private ParticleSystem.MainModule lightningMain;
    private ParticleSystem.MainModule glowMain;
    private ParticleSystem.MainModule lightningTrailMain;
    private ParticleSystem.MainModule glowTrailMain;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mainMain = mainParticle.main;
        lightningMain = lightningParticle.main;
        glowMain = glowParticle.main;
        lightningTrailMain = lightningTrailParticle.main;
        glowTrailMain = glowTrailParticle.main;
        Destroy(gameObject, 10);
    }

    private void Update()
    {
        projectileLight.color = gameManager.healthColor;
        mainMain.startColor = gameManager.healthColor;
        lightningMain.startColor = gameManager.healthColor;
        glowMain.startColor = gameManager.healthColor;
        lightningTrailMain.startColor = gameManager.healthColor;
        glowTrailMain.startColor = gameManager.healthColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        GameObject spawnedExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(spawnedExplosion, 2);

        if(other.gameObject.layer == 9)
        {
            other.gameObject.GetComponent<Soul>().Die();
        }
    }
}
