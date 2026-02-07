using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeCollide : MonoBehaviour
{
    private Animator animator;
    private bool isDroped = false;
    private int activeObjects = 0;
    private int isHitHash;
    [SerializeField] private GameObject[] objectsToActivate;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isHitHash = Animator.StringToHash("isHit");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        animator.SetTrigger(isHitHash);

        if (collision.gameObject.CompareTag("Player") && !isDroped)
        {
            Debug.Log("Tree is hitted");

            ActivateObject(activeObjects);
            activeObjects++;
            if (activeObjects >= objectsToActivate.Length)
            {
                isDroped = true;
            }
        }
    }

    private void ActivateObject(int objectIndex)
    {
        objectsToActivate[objectIndex].gameObject.SetActive(true);
    }
}
