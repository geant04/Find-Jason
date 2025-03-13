using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slugger : Enemy
{
    public NPC npc;

    public void Awake()
    {
        npc.GetComponent<NPC>().Initialize();
        npc.AssignType(new NormalNPC());
        npc.Decorate();

        npc.NavMeshAgent.speed = this.speed;
    }

    void Update()
    {
        
    }
}
