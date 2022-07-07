using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishnetIndicator : MonoBehaviour
{
    [SerializeField] Image leftIndicator;
    [SerializeField] Image rightIndicator;
    [SerializeField] float angleThreshold = .5f;
    [SerializeField] float forwardThreshold = .5f;
    Vector3 targetDirection;
    bool gameEnded;

    void Start()
    {
        DisableIndicators();
    }

    void DisableIndicators()
    {
        leftIndicator.enabled = rightIndicator.enabled = false;
    }

    void Update()
    {
        if (!FindObjectOfType<FishNet>() || gameEnded)
        {
            DisableIndicators();
            return;
        }

        CalculateDirection();
    }

    void CalculateDirection()
    {
        targetDirection = FindObjectOfType<FishNet>().transform.position - transform.position;
        float sideAngle = Vector3.Dot(transform.right, targetDirection); 
        float forwardAngle = Vector3.Dot(transform.forward, targetDirection);

        if (sideAngle < -angleThreshold)
        {
            leftIndicator.enabled = true;
            rightIndicator.enabled = false;
        }
        else if (sideAngle > angleThreshold)
        {
            rightIndicator.enabled = true;
            leftIndicator.enabled = false;
        }
        else if(forwardAngle > .5f)
        {
            DisableIndicators();
        }
    }

    public void GameEnded()
    {
        gameEnded = true;
    }
}
