using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject orb;
    [SerializeField] private GameObject explosion;
    private bool thrown = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (thrown) { Explode(); }
    }

    public void Thrown()
    {
        thrown = true;
    }

    public void Explode()
    {
        orb.SetActive(false);
        explosion.SetActive(true);
        Destroy(gameObject, .5f);
    }
}
