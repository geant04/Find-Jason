using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner
{
    private Transform parent;
    private GameManager manager;

    public NPCSpawner(Transform parent, GameManager manager) 
    {
        this.parent = parent;
        this.manager = manager;
    }

    public NPCType AssignRandomNPCType()
    {
        float f = Random.Range(0.0f, 1.0f);

        if (f > 1.0f) return new StandingNPC();
        return new NormalNPC();
    }

    public void SpawnFriendsAndJason(int n)
    {
        int rand = Random.Range(0, n);

        for (int i = 0; i < n; i++)
        {
            bool isJason = (i == rand);
            NPCType type = AssignRandomNPCType();

            Vector3 randPos = Random.insideUnitSphere * 17.0f;
            randPos.y = 20.0f;

            SpawnNPC(isJason ? new Jason() : type, isJason, randPos);
        }
    }

    public void SpawnIdleCrowds(int n)
    {
        for (int i = 0; i < n; i++)
        {
            Vector3 randPos = Random.insideUnitSphere * 17.0f;
            randPos.y = 20.0f;

            SpawnIdleCrowd(randPos);
        }
    }
    public void SpawnIdleCrowd(Vector3 origin)
    {
        int crowdSize = Random.Range(10, 20);

        for (int i = 0; i < crowdSize; i++)
        {
            Vector3 randPos = Random.insideUnitSphere * (crowdSize * 0.20f);
            randPos.y = 20.0f;

            SpawnNPC(new StandingNPC(), false, origin + randPos);
        }
    }

    public void SpawnNPC(NPCType npcType, bool isJason, Vector3 position)
    {
        GameObject npc = Object.Instantiate(manager.NPC, parent);

        npc.transform.position = position;

        npc.GetComponent<NPC>().Initialize();

        if (isJason)
        {
            GameManager.AssignJason(npc);
            npc.GetComponent<NPC>().IsJason = true;
            npc.name = "Jason";
        }

        npc.GetComponent<NPC>().AssignType(npcType);
        npc.GetComponent<NPC>().Decorate();
    }
}
