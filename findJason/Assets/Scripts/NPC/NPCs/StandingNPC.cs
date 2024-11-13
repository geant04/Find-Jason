using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingNPC : NPCType
{
    public StandingNPC() { }

    public TaskBase StartTask(NPC NPCParent)
    {
        return Missions.DoNothing(NPCParent);
    }

    public void Decorate(NPC NPCParent)
    {
        Color randomColor = new Color();
        randomColor.r = Random.Range(0.0f, 1.0f);
        randomColor.g = Random.Range(0.0f, 1.0f);
        randomColor.b = Random.Range(0.0f, 1.0f);

        NPCParent.capsuleTransform.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        NPCParent.transform.localScale = new Vector3(1.0f, Random.Range(0.8f, 1.5f), 1.0f);
        NPCParent.moveSpeed = 0.0f;
    }
}
