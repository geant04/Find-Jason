using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalNPC : NPCType
{
    public NormalNPC() {  }

    public TaskBase StartTask(NPC NPCParent)
    {
        return new IdleTask(NPCParent);
    }

    public void Decorate(NPC NPCParent)
    {
        Color randomColor = new Color();
        randomColor.r = Random.Range(0.0f, 1.0f);
        randomColor.g = Random.Range(0.0f, 1.0f);
        randomColor.b = Random.Range(0.0f, 1.0f);

        NPCParent.capsuleTransform.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
        NPCParent.transform.localScale = new Vector3(1.0f, Random.Range(0.2f, 1.5f), 1.0f);
        NPCParent.moveSpeed = 0.0f;
    }
}
