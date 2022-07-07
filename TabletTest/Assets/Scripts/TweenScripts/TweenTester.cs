using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenTester : MonoBehaviour
{
    public TweenMachine tweenMachine;

    [SerializeField] float loweredPosYOffset;
    Vector3 startPos, loweredPos;
    public float travelTime;

    public EaseTypes easeType;

    private void Awake()
    {
        tweenMachine = FindObjectOfType<TweenMachine>();

        startPos = loweredPos = transform.position;
        loweredPos.y = Random.Range(-loweredPosYOffset, loweredPosYOffset);
    }

    public void StartEasing(bool shouldHookLower)
    {
        Vector3 targetPos = shouldHookLower ? loweredPos : startPos;

        switch (easeType)
        {
            case EaseTypes.Linear:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.Linear);
                break;

            case EaseTypes.EaseInQuad:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseInQuad);
                break;

            case EaseTypes.EaseInCubic:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseInCubic);
                break;

            case EaseTypes.EaseInQuart:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseInQuart);
                break;

            case EaseTypes.EaseInQuint:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseInQuint);
                break;

            case EaseTypes.EaseInBack:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseInBack);
                break;

            case EaseTypes.EaseInCirc:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseInCirc);
                break;

            case EaseTypes.EaseInSine:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseInSine);
                break;

            case EaseTypes.EaseOutQuad:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseOutQuad);
                break;

            case EaseTypes.EaseOutCubic:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseOutCubic);
                break;

            case EaseTypes.EaseOutQuart:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseOutQuart);
                break;

            case EaseTypes.EaseOutQuint:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseOutQuint);
                break;

            case EaseTypes.EaseOutBack:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseOutBack);
                break;

            case EaseTypes.EaseOutCirc:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseOutCirc);
                break;

            case EaseTypes.EaseOutSine:
                tweenMachine.MoveGameObject(gameObject, targetPos, travelTime, Easings.EaseOutSine);
                break;
        }
    }
}