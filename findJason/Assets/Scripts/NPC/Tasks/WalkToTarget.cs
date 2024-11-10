using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToTarget : TaskBase, NPCTask
{
    private Vector3 target;

    public WalkToTarget(Vector3 target, NPC parentNPC) : base(parentNPC)
    {
        this.target = target;
    }

    public override bool IsDone()
    {
        float threshold = 2.0f;
        float dist = Vector3.Distance(ParentNPC.GetPosition(), target);

        return dist < threshold;
    }

    public override NPCTask Next()
    {
        return new IdleTask(ParentNPC);
    }

    public override void Update()
    {
        ParentNPC.NavMeshAgent.SetDestination(target);
    }
}
