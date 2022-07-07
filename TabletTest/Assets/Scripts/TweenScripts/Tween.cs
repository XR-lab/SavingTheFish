using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Tween
{
    protected GameObject _gameObject;

    private float _speed;
    private float _percent;
    private Func<float, float> EaseMethod;

    public Tween(GameObject objectToTween, float speed, Func<float, float> Method)
    {
        _gameObject = objectToTween;
        _speed = speed;
        _percent = 0;
        EaseMethod = Method;
    }

    public void UpdateTween(float dt)
    {
        _percent += dt / _speed;

        if (_percent < 1)
        {
            float easeStep = EaseMethod(_percent);
            PerformTween(easeStep);
        }
        else
        {
            TweenEnd();
        }
    }

    /*
     * Deze abstracte functies krijgt=en een invulling in child classes
     * Maar wordt wel aangeroepen in de UpdateTween functie.
     * Deze class bepaald dus het wanneer, de child class bepaald wat
     */
    protected abstract void PerformTween(float easeStep);
    protected abstract void TweenEnd();
}
