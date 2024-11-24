using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleTask : TaskBase
{
    protected float idleTimer;
    protected float waitTime = -1.0f;

    protected float GetWaitTime()
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

public class JasonIdleTask : IdleTask
{
    public JasonIdleTask(NPC parentNPC) : base(parentNPC)
    {
        ParentNPC = parentNPC;
        idleTimer = GetWaitTime();
    }
    public override NPCTask Next()
    {
        ParentNPC.SetSpeed(Random.Range(3.5f, 4.5f));
        float random = Random.Range(0.0f, 1.0f);

        if (random > 0.4f || ParentNPC.isFound)
        {
            Vector3 target = TargetFinding.GetRandomTarget(ParentNPC);
            if (Vector3.Distance(target, ParentNPC.GetPosition()) > 0.1f) return new JasonWalkToTarget(target, ParentNPC);
        }

        idleTimer = GetWaitTime();
        return this;
    }

    public override void Update()
    {
        if (ParentNPC.isFound) idleTimer = 0.0f;
        base.Update();
    }
}
