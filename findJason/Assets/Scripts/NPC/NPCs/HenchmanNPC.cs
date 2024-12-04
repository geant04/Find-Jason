using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HenchmanNPC : NPCType
{
    public NPC leader;
    public Color leaderColor;
    public HenchmanNPC() { }
    public HenchmanNPC(NPC leader, Color leaderColor) 
    { 
        this.leader = leader;
        this.leaderColor = leaderColor;
    }

    public TaskBase StartTask(NPC ParentNPC)
    {
        return Missions.FollowLeader(ParentNPC);
    }

    public void Decorate(NPC NPCParent)
    {
        Color randomColor = leader ? leaderColor : ColorUtil.GetRandomColor();
        randomColor = ColorUtil.HSVTransform(randomColor, 1.0f, Random.Range(0.1f, 0.6f), 1.0f);

        NPCParent.capsuleTransform.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
        NPCParent.transform.localScale = new Vector3(1.0f, Random.Range(0.6f, 1.2f), 1.0f);
        NPCParent.NavMeshAgent.speed = Random.Range(2.5f, 3.2f);

        // hat stuff
        Transform hat = NPCParent.transform.Find("Hat");
        Transform obj = hat.Find("obj");
        NPCParent.hat = leader ? leader.hat : ModelSingleton.Instance.hatMap["Cap"];

        obj.GetComponent<MeshFilter>().mesh = NPCParent.hat;
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
    }
}

public class WalkToLeader : TaskBase, NPCTask
{
    protected Vector3 target;
    float timeToSkip = 3.0f;

    public WalkToLeader(Vector3 target, NPC parentNPC) : base(parentNPC)
    {
        if (target.y > 20.0f) target.y = -10000;
        this.target = target;
    }

    public override bool IsDone()
    {
        if (target.y < -1000 || timeToSkip < 0.0f) return true;
        float threshold = Random.Range(1.5f, 2.5f);
        float dist = Vector3.Distance(ParentNPC.GetPosition(), target);
        return dist < threshold;
    }

    public override NPCTask Next()
    {
        return Missions.FollowLeader(ParentNPC);
    }

    public override void Update()
    {
        ParentNPC.NavMeshAgent.SetDestination(target);
        timeToSkip -= Time.deltaTime;
    }
}
