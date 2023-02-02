using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : MonoBehaviour
{
    public Transform targetTransform;
    float speed = 10;

    void Update()
    {
        Vector3 displacementFromTarget = targetTransform.position - transform.position;
        Vector3 directionToTarget = displacementFromTarget.normalized;
        Vector3 velocity = directionToTarget * speed;

        float distanceToTarget = displacementFromTarget.magnitude;

        if (distanceToTarget > 1.5f)
        {
            transform.Translate(velocity * Time.deltaTime);
        }
        else if (distanceToTarget < 1.4f)
        {
            transform.Translate(-velocity * Time.deltaTime);
        }
    }
}
