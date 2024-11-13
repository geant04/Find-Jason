using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleTask : TaskBase
{
    private float idleTimer;
    private float waitTime = -1.0f;

    private float GetWaitTime()
    {
        return waitTime == -1.0f ? Random.Range(0.0f, 4.0f) : waitTime;
    }

    public IdleTask(NPC parentNPC) : base(parentNPC)
    {
        ParentNPC = parentNPC;
        idleTimer = GetWaitTime();
    }

    public IdleTask(NPC parentNPC, float waitTime) : base(parentNPC)
    {
        ParentNPC = parentNPC;
        idleTimer = waitTime;
        this.waitTime = waitTime;
    }

    public override bool IsDone()
    {
        return idleTimer == 0.0f;
    }

    public override NPCTask Next()
    {
        float random = Random.Range(0.0f, 1.0f);

        if (!ParentNPC.IsJason && random > 0.96f)
        {
            return Missions.GoNearJason(ParentNPC);
        }

        if (random > 0.4f)
        {
            // A random walk...
            Vector3 target = TargetFinding.GetRandomTarget(ParentNPC);
            if (Vector3.Distance(target, ParentNPC.GetPosition()) > 0.1f) return new WalkToTarget(target, ParentNPC);
        }

        idleTimer = GetWaitTime();
        return this;
    }

    public override void Update()
    {
        idleTimer = Mathf.Max(0.0f, idleTimer - Time.deltaTime);
    }
}
