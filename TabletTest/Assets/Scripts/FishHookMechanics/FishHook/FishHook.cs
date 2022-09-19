using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishHook : MonoBehaviour
{
    [SerializeField] TweenTester tween;
    [SerializeField] float hookCooldown = 3f;
    [SerializeField] float destroyCooldown = 3f;
    [SerializeField] float fishHookWaitTime = 1f;
    [SerializeField] Vector3 fishRotation;
    [SerializeField] Vector3 fishOffset;

    bool canCollideWithFish;
    bool inCooldown;

    private void Start()
    {
        HookTriggered(true);

        Invoke(nameof(FishCollisionCooldown), hookCooldown + tween.travelTime);
    }

    void FishCollisionCooldown()
    {
        canCollideWithFish = true;
    }

    public void HookTriggered(bool shouldLowerHook)
    {
        if (inCooldown)
            return;

        tween.StartEasing(shouldLowerHook);

        if (shouldLowerHook)
            return;

        inCooldown = true;
        GetComponent<Collider>().enabled = false;

        if (transform.childCount == 0)
        {
            FishHookCounter.fishHookCounter.FishHookGotStopped();
        }

        Destroy(gameObject, destroyCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fish") || !canCollideWithFish)
            return;

        StartCoroutine(ReturnHook(other.gameObject));
    }

    IEnumerator ReturnHook(GameObject fish)
    {
        fish.transform.SetParent(this.gameObject.transform);

        Vector3 positionOffset = new Vector3(transform.position.x + fishOffset.x, transform.position.y + fishOffset.y, transform.position.z + fishOffset.z);
        fish.GetComponent<FlockUnit>().myTransform.position = positionOffset;
        fish.GetComponent<FlockUnit>().myTransform.rotation = Quaternion.Euler(fishRotation);
        fish.GetComponent<FlockUnit>().canMove = false; 

        GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(fishHookWaitTime);

        FishCounter.fishCounter.FishGotHooked();
        HookTriggered(false);
    }
}
