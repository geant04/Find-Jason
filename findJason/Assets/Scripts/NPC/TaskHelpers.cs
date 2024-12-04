using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public delegate Vector3 TargetSampler(NPC ParentNPC);
public class TargetFinding
{
    public static Vector3 GetTarget(NPC ParentNPC, Sampler sampler)
    {
        for (int i = 0; i < 20; i++)
        {
            // sample gives you a point to go towards from the player position
            Vector3 sample = sampler.GetSample(ParentNPC);
            Vector3 target = ParentNPC.GetPosition() + sample;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(target, out hit, sample.magnitude, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(ParentNPC.GetPosition(), hit.position, NavMesh.AllAreas, path)
                    && path.status == NavMeshPathStatus.PathComplete)
                {
                    //if (Mathf.Abs(hit.position.y - ParentNPC.GetPosition().y) > 0.5f) continue; // verticality check fail
                    return hit.position;
                }
            }
        }

        return ParentNPC.GetPosition();
    }

    public static Vector3 GetTargetNearJason(NPC ParentNPC)
    {
        return GetTarget(ParentNPC, new JasonSampler());
    }
    public static Vector3 GetRandomTarget(NPC ParentNPC)
    {
        return GetTarget(ParentNPC, new Sampler());
    }
    public static Vector3 GetTargetNearLeader(NPC ParentNPC, NPC Leader)
    {
        return GetTarget(ParentNPC, new LeaderSampler(Leader));
    }
}