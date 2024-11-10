using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    NPCTaskSystem taskSystem;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    public float moveSpeed;
    public bool IsJason = false;

    void Start()
    {
        taskSystem = new NPCTaskSystem(this);
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Decorate()
    {
        if (IsJason)
        {
            GameObject badge = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            badge.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            badge.transform.position = transform.position + transform.forward * 0.50f;
            badge.transform.parent = transform;
        }
    }

    public void Move(Vector3 dir)
    {
        transform.position += Vector3.Normalize(dir) * moveSpeed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        taskSystem.Update();
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
        taskQueue.Enqueue(new IdleTask(ParentNPC));
        currentTask = taskQueue.Peek();
    }

    public void Update() // task queue
    {
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
