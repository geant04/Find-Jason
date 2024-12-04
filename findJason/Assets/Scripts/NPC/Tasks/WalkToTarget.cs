using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToTarget : TaskBase, NPCTask
{
    protected Vector3 target;

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

public class JasonWalkToTarget : WalkToTarget
{
    private float walkSpeed;
    public JasonWalkToTarget(Vector3 target, NPC parentNPC) : base(target, parentNPC)
    {
        this.target = target;
        walkSpeed = parentNPC.NavMeshAgent.speed;
    }

    public override NPCTask Next()
    {
        return new JasonIdleTask(ParentNPC);
    }

    public override void Update()
    {
        if (ParentNPC.isFound) ParentNPC.SetSpeed(walkSpeed * 1.25f);
        ParentNPC.NavMeshAgent.SetDestination(target);
    }
}