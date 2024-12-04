using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mission : TaskBase, NPCTask
{
    private Queue<NPCTask> tasks = new Queue<NPCTask>();
    private NPCTask nextTask;
    private NPCTask currentTask;

    public Mission(NPC parentNPC) : base(parentNPC) 
    {
        nextTask = new IdleTask(parentNPC);
    }
    public Mission(NPC parentNPC, NPCTask nextTask) : base(parentNPC)
    {
        this.nextTask = nextTask;
    }

    public void AddTask(NPCTask task)
    {
        tasks.Enqueue(task);
    }

    public override NPCTask Next()
    {
        return nextTask;
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

    public static Mission FollowLeader(NPC ParentNPC)
    {
        Mission FollowLeader = new Mission(ParentNPC, new WalkToLeader(new Vector3(0, -10000, 0), ParentNPC)); // horrible hack to skip a task
        HenchmanNPC henchman = ParentNPC.npcType as HenchmanNPC;
        NPC Leader = henchman.leader;

        Vector3 target = TargetFinding.GetTargetNearLeader(ParentNPC, Leader);
        FollowLeader.AddTask(new WalkToLeader(target, ParentNPC)); // TO DO: make a new task called "walk to leader"

        return FollowLeader;
    }
}
