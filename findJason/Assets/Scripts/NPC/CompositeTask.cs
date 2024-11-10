using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mission : TaskBase, NPCTask
{
    private Queue<NPCTask> tasks = new Queue<NPCTask>();
    private NPCTask currentTask;

    public Mission(NPC parentNPC) : base(parentNPC)
    {}

    public void AddTask(NPCTask task)
    {
        tasks.Enqueue(task);
    }

    public override NPCTask Next()
    {
        return new IdleTask(ParentNPC);
    }

    public override bool IsDone()
    {
        return currentTask == null && tasks.Count == 0;
    }

    public override void Update()
    {
        if (currentTask == null && tasks.Count > 0)
        {
            currentTask = tasks.Dequeue();
        }

        if (currentTask != null)
        {
            currentTask.Update();

            if (currentTask.IsDone())
            {
                currentTask = null;
            }
        }
    }
}

public class Missions
{
    public static Mission GoNearJason(NPC ParentNPC)
    {
        Mission GoNearJason = new Mission(ParentNPC);
        int trials = 20;

        // calculate a direction towards Jason
        for (int i = 0; i < trials && GameManager.Jason != null; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere;
            randomDirection.y = 0.0f;

            Vector3 toJason = GameManager.Jason.transform.position - ParentNPC.GetPosition();
            float randOut = Random.Range(-2.0f, 2.0f) + toJason.magnitude;

            randomDirection = Vector3.Normalize(randomDirection + toJason) * randOut;

            Vector3 target = ParentNPC.GetPosition() + randomDirection;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(target, out hit, randOut, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                if (NavMesh.CalculatePath(ParentNPC.GetPosition(), hit.position, NavMesh.AllAreas, path)
                    && path.status == NavMeshPathStatus.PathComplete)
                {
                    GoNearJason.AddTask(new WalkToTarget(hit.position, ParentNPC));
                    return GoNearJason;
                }
            }
        }

        return GoNearJason;
    }
}
