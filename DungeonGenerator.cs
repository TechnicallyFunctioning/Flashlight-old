using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.AI;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject dungeonExit;
    public GameObject[] enemies;
    public int enemySpawnChance;

    public GameObject combinedWalls;

    public List<Vector3> floorPositions;
    public Vector3 currentPosition;

    public BoxCollider boxCollider;

    public int maxTiles = 25;
    public int currentTiles;

    public Vector3 front;
    public bool frontEmpty;
    public Vector3 back;
    public bool backEmpty;
    public Vector3 left;
    public bool leftEmpty;
    public Vector3 right;
    public bool rightEmpty;

    public bool boxedIn;

    public int sideRoll = 0;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        currentPosition = transform.position;

        AssignFloorPositions();
        SpawnFloor();
        Instantiate(dungeonExit, floorPositions[floorPositions.Count - 1], transform.rotation);
        SpawnWalls();
        boxCollider.enabled = false;
        SpawnEnemies();
    }


    private void AssignFloorPositions()
    {
        while (!boxedIn && currentTiles < maxTiles)
        {
            //Get positions on all sides based on direction, tile scale, and collider size
            front = currentPosition + Vector3.forward * (boxCollider.size.z * transform.localScale.z);
            back = currentPosition + Vector3.back * (boxCollider.size.z * transform.localScale.z);
            left = currentPosition + Vector3.left * (boxCollider.size.x * transform.localScale.x);
            right = currentPosition + Vector3.right * (boxCollider.size.x * transform.localScale.x);

            //Check which positions are already taken on our list
            if (!floorPositions.Contains(front)) { frontEmpty = true; }
            if (!floorPositions.Contains(back)) { backEmpty = true; }
            if (!floorPositions.Contains(left)) { leftEmpty = true; }
            if (!floorPositions.Contains(right)) { rightEmpty = true; }
            if (!frontEmpty && !backEmpty && !leftEmpty && !rightEmpty) { boxedIn = true; }

            if(back.z < transform.position.z + boxCollider.size.z * transform.localScale.z) { backEmpty = false; }

            //Roll for next position to add to our list
            sideRoll = Random.Range(0, 5);

            //If the position we rolled for is empty, add one to our count, add it to our list, and set it as our current position
            if (sideRoll < 2 && frontEmpty)
            {
                currentTiles++;
                floorPositions.Add(front);
                currentPosition = front;
            }
            else if (sideRoll == 2 && backEmpty)
            {
                currentTiles++;
                floorPositions.Add(back);
                currentPosition = back;
            }
            else if (sideRoll == 3 && leftEmpty)
            {
                currentTiles++;
                floorPositions.Add(left);
                currentPosition = left;
            }
            else if (sideRoll <5 && rightEmpty)
            {
                currentTiles++;
                floorPositions.Add(right);
                currentPosition = right;
            }

        }

    }
    private void SpawnFloor()
    {
        for (int i = 0; i < floorPositions.Count; i++)
        {
            GameObject floorTile = Instantiate(floorTiles[Random.Range(0, floorTiles.Length)], floorPositions[i], transform.rotation);
            floorTile.transform.SetParent(transform);
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (!meshRenderer.enabled) { meshRenderer.enabled = true; }

        gameObject.AddComponent<MeshCollider>();

        NavMeshBuilder.BuildNavMesh();
    }
    private void SpawnWalls()
    {
        floorPositions.Add(transform.position);
        combinedWalls = new GameObject("Combined Walls");
        combinedWalls.AddComponent<MeshFilter>();
        combinedWalls.AddComponent<MeshRenderer>();

        for (int i = 0; i < floorPositions.Count; i++)
        {
            frontEmpty = false; backEmpty = false; leftEmpty = false; rightEmpty = false;

            //Get positions on all sides based on direction, tile scale, and collider size
            front = floorPositions[i] + Vector3.forward * (boxCollider.size.z * transform.localScale.z);
            back = floorPositions[i] + Vector3.back * (boxCollider.size.z * transform.localScale.z);
            left = floorPositions[i] + Vector3.left * (boxCollider.size.x * transform.localScale.x);
            right = floorPositions[i] + Vector3.right * (boxCollider.size.x * transform.localScale.x);

            //Check which positions are already taken on our list
            if (!floorPositions.Contains(front)) { frontEmpty = true; }
            if (!floorPositions.Contains(back)) { backEmpty = true; }
            if (!floorPositions.Contains(left)) { leftEmpty = true; }
            if (!floorPositions.Contains(right)) { rightEmpty = true; }
            if (!frontEmpty && !backEmpty && !leftEmpty && !rightEmpty) { boxedIn = true; }

            if (back.z < transform.position.z) { backEmpty = false; }

            //if (back.z < transform.position.z + boxCollider.size.z * transform.localScale.z) { backEmpty = false; }

            if (frontEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.forward * ((boxCollider.size.z * transform.localScale.z) / 2), transform.rotation * Quaternion.Euler(Vector3.up * 90));
                wallTile.transform.SetParent(combinedWalls.transform);
            }
            if (backEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.back * ((boxCollider.size.z * transform.localScale.z) / 2), transform.rotation * Quaternion.Euler(Vector3.up * 90));
                wallTile.transform.SetParent(combinedWalls.transform);

            }
            if (leftEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.left * ((boxCollider.size.x * transform.localScale.x) / 2), transform.rotation);
                wallTile.transform.SetParent(combinedWalls.transform);

            }
            if (rightEmpty)
            {
                GameObject wallTile = Instantiate(wallTiles[Random.Range(0, wallTiles.Length)], floorPositions[i] + Vector3.right * ((boxCollider.size.x * transform.localScale.x) / 2), transform.rotation);
                wallTile.transform.SetParent(combinedWalls.transform);
            }
        }

        combinedWalls.AddComponent<MeshCollider>();
        combinedWalls.layer = LayerMask.NameToLayer("Obstacle");
    }
    private void SpawnEnemies()
    {
        for (int i = 0; i < floorPositions.Count; i++)
        {
            if (Random.Range(0, 10) < enemySpawnChance) { Instantiate(enemies[Random.Range(0, enemies.Length)], floorPositions[i], transform.rotation); }
        }
    }



    /* 
       private void SpawnEnemies() { }*/
}