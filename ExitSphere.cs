using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSphere : MonoBehaviour
{
    [SerializeField] private float maxDistance = 3;
    private Vector3 startPos;
    private bool isHeld = false;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, Vector3.zero) > maxDistance) { ResetPosition(); }
    }

    private void ResetPosition()
    {
        transform.position = startPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Head") && isHeld) { Application.Quit(); Debug.Log("Quit"); }
    }

    public void PickedUp() { isHeld = true; }
    public void Dropped() { isHeld = false; }
}
