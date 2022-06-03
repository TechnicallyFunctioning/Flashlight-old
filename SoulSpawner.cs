using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject soul;
    [SerializeField] private float startDelay;
    [SerializeField] private float frequencyMin;
    [SerializeField] private float frequencyMax;

    private void Start()
    {
        InvokeRepeating("SpawnSoul", startDelay, Random.Range(frequencyMin, frequencyMax));
    }

    private void Update()
    {
        if (gameManager.gameOver) { CancelInvoke(); }
    }

    private void SpawnSoul()
    {
        GameObject spawnedSoul = Instantiate<GameObject>(soul, transform.position, Quaternion.identity);
    }
}
