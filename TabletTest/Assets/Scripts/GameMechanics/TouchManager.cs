using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EzySlice;

public class TouchManager : MonoBehaviour
{
    [SerializeField] float followSpeed = .002f;
    [SerializeField] float swipeRange = 200f;

    [Space, SerializeField] float shakeSpeed = 5f;
    [SerializeField] float shakeAmount = .1f;

    GameObject currentInteractable;
    Vector3 startSwipePos, currentSwipePos;
    float hookStartPosX;

    [Space, SerializeField] Material crossMaterial;
    [SerializeField] LayerMask layerMask;
    Ray ray;
    RaycastHit hit;

    private void Update()
    {
        if (Input.touchCount == 0)
            return;

        CheckTouchBegan();
        MoveHook();
        CheckTouchEnded();
    }

    void CheckTouchBegan()
    {
        if (Input.GetTouch(0).phase != TouchPhase.Began)
            return;

        startSwipePos = Input.GetTouch(0).position;

        ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("FishHook"))
            {
                currentInteractable = hit.collider.gameObject;
                hookStartPosX = currentInteractable.transform.position.x;
            }
        }
    }

    void MoveHook()
    {
        Touch touch = Input.GetTouch(0);
        Vector3 currentPos = currentInteractable.transform.position;

        if (currentInteractable.GetComponent<FishHook>())
        {
            float shakeHookPosX = hookStartPosX + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount;
            currentInteractable.transform.position = new Vector3(shakeHookPosX, currentPos.y + touch.deltaPosition.y * followSpeed, currentPos.z);
        }
    }

    void CheckTouchEnded()
    {
        if (Input.GetTouch(0).phase != TouchPhase.Ended)
            return;

        if (CheckIfSwipeThresholdReached() && currentInteractable.GetComponent<FishHook>())
        {
            currentInteractable.GetComponent<FishHook>().HookTriggered(false);

            SaveTheFishData data = FindObjectOfType<SaveTheFishData>();
            if (data != null) data.FishSaved += 1;
        }

        Slice();

        currentInteractable = null;
    }

    bool CheckIfSwipeThresholdReached()
    {
        currentSwipePos = Input.GetTouch(0).position;
        float distancePosY = currentSwipePos.y - startSwipePos.y;

        return distancePosY < -swipeRange || distancePosY > swipeRange;
    }

    public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(hit.point, new Vector3(5, 0.1f, 5), CalculateRotation(startSwipePos, currentSwipePos), layerMask);

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            FishHookCounter.fishHookCounter.FishHookGotStopped();

            SlicedHull hull = SliceObject(hits[i].gameObject, crossMaterial);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, crossMaterial);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, crossMaterial);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Destroy(hits[i].gameObject);
            }
        }
        
        SaveTheFishData data = FindObjectOfType<SaveTheFishData>();
        if (data != null) data.FishSaved += 1;
    }

    public void AddHullComponents(GameObject go)
    {
        go.layer = 9;
        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = go.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(10, go.transform.position, 40);
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // slice the provided object using the transforms of this object
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(hit.point, hit.transform.up, crossSectionMaterial);
    }

    public Quaternion CalculateRotation(Vector3 target, Vector3 origin)
    { 
        Vector3 dir = target - origin;
        Quaternion rotation = Quaternion.Euler(dir);

        return rotation;
    }
}
