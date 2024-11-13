using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TargetFinding
{
    public static Vector3 GetRandomTarget(NPC ParentNPC)
    {
        for (int i = 0; i < 20; i++)
        {
            float randOut = Random.Range(5.0f, 40.0f);
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
                    return hit.position;
                }
            }
        }

        return ParentNPC.GetPosition();
    }

    // TO DO: Refactor this code for later
    public static Vector3 GetTargetNearJason(NPC ParentNPC)
    {
        int trials = 20;

        // calculate a direction towards Jason
        for (int i = 0; i < trials && GameManager.Jason != null; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0.0f;

            Vector3 toJason = GameManager.Jason.transform.position - ParentNPC.GetPosition();
            float randOut = Random.Range(0.0f, 2.0f) + toJason.magnitude * Random.Range(0.0f, 1.0f);

            randomDirection = Vector3.Normalize(randomDirection + toJason) * randOut;

            Vector3 target = ParentNPC.GetPosition() + randomDirection;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(target, out hit, randOut, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(ParentNPC.GetPosition(), hit.position, NavMesh.AllAreas, path)
                    && path.status == NavMeshPathStatus.PathComplete)
                {
                    return hit.position;
                }
            }
        }

        return ParentNPC.GetPosition();
    }
}
