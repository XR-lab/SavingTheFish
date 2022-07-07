using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenMachine : MonoBehaviour
{
     public List<Tween> activeTweens = new List<Tween>();

    private void Update()
    {
        if (activeTweens.Count < 1) return;

        for (int i = 0; i < activeTweens.Count; i++)
        {
            activeTweens[i].UpdateTween(Time.deltaTime);
        }
    }

    public void MoveGameObject(GameObject objectToMove, Vector3 targetPosition, float speed, Func<float, float> EaseMethod)
    {
        PositionTween newTween = new PositionTween(objectToMove, targetPosition, speed, EaseMethod);
        activeTweens.Add(newTween);
    }
}
