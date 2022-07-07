using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTween : Tween
{
    private Vector3 _startPosition;
    private Vector3 _targetPosition;
    private Vector3 _direction;

    public PositionTween(GameObject objectToMove, Vector3 targetPosition, float speed, Func<float, float> Method) : base(objectToMove, speed, Method)
    {
        _targetPosition = targetPosition;
        _startPosition = _gameObject.transform.position;
        _direction = _targetPosition - _startPosition;
    }

    protected override void PerformTween(float easeStep)
    {
        _gameObject.transform.position = _startPosition + (_direction * easeStep);
    }

    protected override void TweenEnd()
    {
        _gameObject.transform.position = _targetPosition;

        TweenMachine tweenMachine = GameObject.FindObjectOfType<TweenMachine>();
        foreach (PositionTween tween in tweenMachine.activeTweens)
        {
            if(tween == this)
            {
                tweenMachine.activeTweens.Remove(tween);
                return;
            }
        }
    }
}
