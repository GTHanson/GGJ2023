using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Profiling;

public class CustomerManager : MonoBehaviour
{
    public float CooldownPerSpawn;
    [SerializeField]
    private List<GameObject> CustomerPrefabs;
    [SerializeField]
    private GameObject world;

    private List<CustomerSpawn> spawns = new List<CustomerSpawn>();
    private List<NPC> npcList = new List<NPC>();

    private void Awake()
    {
        spawns = FindObjectsByType<CustomerSpawn>(FindObjectsSortMode.None).ToList();
    }

    private void Start()
    {
        StartCoroutine(CustomerSpawnLoop());
    }

    private IEnumerator CustomerSpawnLoop()
    {
        while (gameObject.activeSelf)
        {
            if (npcList.Count >= 100)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(10, 30));
                continue;
            }

            foreach (CustomerSpawn spawn in spawns.Where(sp => sp.OnCooldown == false))
            {
                if (UnityEngine.Random.Range(0, 4) == 1)
                {
                    NPC npc = spawn.SpawnCustomer(CustomerPrefabs[UnityEngine.Random.Range(0, CustomerPrefabs.Count)]);
                    if(npc != null)
                    {
                        npcList.Add(npc);
                    }
                }
            }

            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 30));
        }
    }
}
