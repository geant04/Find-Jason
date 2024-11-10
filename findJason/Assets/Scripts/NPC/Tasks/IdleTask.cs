using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleTask : TaskBase
{
    private float idleTimer;

    private float GetWaitTime()
    {
        return Random.Range(0.0f, 4.0f);
    }

    public IdleTask(NPC parentNPC) : base(parentNPC)
    {
        ParentNPC = parentNPC;
        idleTimer = GetWaitTime();
    }

    public override bool IsDone()
    {
        return idleTimer == 0.0f;
    }

    public override NPCTask Next()
    {
        float random = Random.Range(0.0f, 1.0f);

        if (!ParentNPC.IsJason && random > 0.70f)
        {
            return Missions.GoNearJason(ParentNPC);
        }

        if (random > 0.4f)
        {
            // A random walk...
            for (int i = 0; i < 20; i++) 
            {
                float randOut = Random.Range(20.0f, 40.0f);
                Vector3 randomDirection = Random.insideUnitSphere * randOut;
                randomDirection.y = 0.0f;

                Vector3 target = ParentNPC.GetPosition() + randomDirection;

                NavMeshHit hit;
                if (NavMesh.SamplePosition(target, out hit, randOut, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (NavMesh.CalculatePath(ParentNPC.GetPosition(), hit.position, NavMesh.AllAreas, path) 
                        && path.status == NavMeshPathStatus.PathComplete)
                    {
                        return new WalkToTarget(hit.position, ParentNPC);
                    }
                }
            }
        }

        idleTimer = GetWaitTime();
        return this;
    }

    public override void Update()
    {
        idleTimer = Mathf.Max(0.0f, idleTimer - Time.deltaTime);
    }
}
