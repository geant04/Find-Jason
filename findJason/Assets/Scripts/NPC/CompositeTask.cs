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
        Vector3 target = TargetFinding.GetTargetNearJason(ParentNPC);
        GoNearJason.AddTask(new WalkToTarget(target, ParentNPC)); // TO DO: make a new task called "walk to leader"
        return GoNearJason;
    }

    public static Mission DoNothing(NPC ParentNPC)
    {
        Mission DoNothing = new Mission(ParentNPC);
        DoNothing.AddTask(new IdleTask(ParentNPC, 9999999999.0f));
        return DoNothing;
    }
}
