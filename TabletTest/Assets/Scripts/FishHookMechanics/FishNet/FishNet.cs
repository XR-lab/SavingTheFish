using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FishNet : MonoBehaviour
{
    [SerializeField] Collider fishCollider;
    [SerializeField] Collider touchCollider;

    public Transform[] m_FlockPositions;
    Vector3 m_Targetpoint;

    [Space, SerializeField] float speed = 3f;
    [SerializeField] float destroyCooldown = 3f;
    [SerializeField] float fishHookWaitTime = 1f;
    [SerializeField] Vector3 fishOffset;
    bool inCooldown;
    bool goUp;

    private void Start()
    {
        HookTriggered(true);
        m_FlockPositions = Transform.FindObjectsOfType<FlockWayPoint>().Select(flock => flock.transform).ToArray();
        GetRandomWaypoint();
    }

    void Update()
    {
        if (transform.position != m_Targetpoint && goUp == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_Targetpoint, Time.deltaTime * speed);
            GetComponent<Collider>().enabled = false;
        }
        else 
        {
            GetComponent<Collider>().enabled = true;
            goUp = true;
            StartCoroutine(NetWait());
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

    void GetRandomWaypoint()
    {
        int m_RandomIndex = Random.Range(0, m_FlockPositions.Length);
        m_Targetpoint = m_FlockPositions[m_RandomIndex].position;
    }

    IEnumerator ReturnHook(GameObject fish)
    {
        SetFishValues(fish);

        if (transform.childCount >= 0)
        {
            touchCollider.enabled = false;
        }

        if(transform.childCount >= 2)
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
        transform.position += transform.up * Time.deltaTime * speed;
        yield return new WaitForSeconds(3);
        HookTriggered(false);
        yield return null;
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
