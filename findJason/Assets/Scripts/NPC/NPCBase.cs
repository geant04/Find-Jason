using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NPCTaskSystem taskSystem;
    public NPCType npcType;
    public Mesh hat;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public bool IsJason = false;
    [HideInInspector] public bool isFound = false;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public Transform capsuleTransform;
    public bool disable = false;
    public Color color;

    public void Initialize()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        capsuleTransform = transform.Find("Capsule");
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public virtual TaskBase StartTask()
    {
        if (npcType != null) return npcType.StartTask(this);
        return new IdleTask(this); // default
    }

    public void AssignType(NPCType type)
    {
        npcType = type;
        taskSystem = new NPCTaskSystem(this);
    }
    public void SetSpeed(float speed)
    {
        NavMeshAgent.speed = speed;
    }

    public void Decorate()
    {
        if (npcType != null) npcType.Decorate(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (disable) return;
        if (npcType != null && taskSystem != null) taskSystem.Update();
    }
}

public class NPCTaskSystem
{
    Queue<NPCTask> taskQueue;
    NPCTask currentTask;
    NPC ParentNPC;
    public NPCTaskSystem(NPC ParentNPC)
    {
        taskQueue = new Queue<NPCTask>();
        taskQueue.Enqueue(ParentNPC.StartTask());
        currentTask = taskQueue.Peek();
    }

    public void AbortTask()
    {
        if (taskQueue.Count < 2) taskQueue.Enqueue(currentTask.Next());
        currentTask = taskQueue.Dequeue();
    }

    public void Update() // task queue
    {
        if (currentTask == null)
        {
            Debug.Log(taskQueue);
            currentTask = taskQueue.Dequeue();
        }
        if (currentTask.IsDone())
        {
            taskQueue.Enqueue(currentTask.Next());
            currentTask = taskQueue.Dequeue();
        }

        currentTask.Update();
    }
}

public abstract class TaskBase : NPCTask
{
    protected NPC ParentNPC;

    public TaskBase(NPC parentNPC)
    {
        ParentNPC = parentNPC;
    }

    public abstract bool IsDone();
    public abstract void Update();
    public abstract NPCTask Next();
}

public interface NPCTask
{
    bool IsDone();
    void Update();
    NPCTask Next();
}
public interface NPCType
{
    public abstract void Decorate(NPC ParentNPC);
    public abstract TaskBase StartTask(NPC ParentNPC);
}