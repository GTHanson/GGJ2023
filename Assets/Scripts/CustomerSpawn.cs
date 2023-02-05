using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    public bool OnCooldown;

    private CustomerManager customerManager;

    private void Awake()
    {
        customerManager = FindAnyObjectByType<CustomerManager>();
    }

    public NPC SpawnCustomer(GameObject customerPrefab)
    {
        Vector3 spawnRot = new Vector3(0, 0, 0);

        if (Random.Range(0, 2) == 1)
            spawnRot.y = 180;

        GameObject npcObj = Instantiate(customerPrefab, transform.position, Quaternion.Euler(spawnRot));
        npcObj.transform.SetParent(transform.parent);
        OnCooldown = true;
        StartCoroutine(Cooldown());

        return npcObj.GetComponent<NPC>();
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(customerManager.CooldownPerSpawn);
        OnCooldown = false;
    }
}
