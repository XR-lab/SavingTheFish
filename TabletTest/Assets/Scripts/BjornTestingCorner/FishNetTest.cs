using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNetTest : MonoBehaviour
{
    [SerializeField] Collider fishCollider;
    [SerializeField] Collider touchCollider;
    [SerializeField] GameObject FlockPosition;

    [Space, SerializeField] float speed = 3f;
    [SerializeField] float destroyCooldown = 3f;
    [SerializeField] float fishHookWaitTime = 1f;
    [SerializeField] Vector3 fishOffset;
    bool inCooldown;
    bool goUp;

    private void Start()
    {
        HookTriggered(true);
        FlockPosition = GameObject.FindGameObjectWithTag("Flock");
        
    }

    void Update()
    {
        if (transform.position != FlockPosition.transform.position && goUp == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, FlockPosition.transform.position, Time.deltaTime * speed);
        }
        else //(transform.position == FlockPosition.transform.position)
        {
            goUp = true;
            Debug.Log(goUp);
            NetWait();
            transform.position += transform.up * Time.deltaTime * speed;
        }
    }
    public void HookTriggered(bool shouldLowerHook)
    {
        if (inCooldown || shouldLowerHook)
            return;

        inCooldown = true;

        if (transform.childCount == 3)
        {
            GetComponent<Collider>().enabled = false;
        }

        Destroy(gameObject, destroyCooldown);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Fish"))
            return;

        StartCoroutine(ReturnHook(other.gameObject));
    }

    IEnumerator ReturnHook(GameObject fish)
    {
        SetFishValues(fish);

        if (transform.childCount == 1)
        {
            touchCollider.enabled = false;
        }

        if(transform.childCount == 3)
        {
            fishCollider.enabled = false;
        }

        yield return new WaitForSeconds(fishHookWaitTime);

        FishCounter.fishCounter.FishGotHooked();
        HookTriggered(false);
    }
    IEnumerator NetWait()
    {
        yield return new WaitForSeconds(5);
    }

    void SetFishValues(GameObject fish)
    {
        fish.transform.SetParent(this.gameObject.transform);
        Vector3 positionOffset = new Vector3(transform.position.x + Random.Range(fishOffset.x, -fishOffset.x), transform.position.y + Random.Range(fishOffset.y, -fishOffset.y), transform.position.z + Random.Range(fishOffset.z, -fishOffset.z));
        fish.GetComponent<FlockUnit>().myTransform.position = positionOffset;
        fish.GetComponent<FlockUnit>().myTransform.rotation = transform.rotation;
        fish.GetComponent<FlockUnit>().canMove = false; 
        fish.GetComponent<Collider>().enabled = false;
    }
}
