using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderNPC : NPCType
{
    public LeaderNPC() {  }

    public TaskBase StartTask(NPC NPCParent)
    {
        return new IdleTask(NPCParent);
    }

    public Mesh GetHat()
    {
        int rand = Random.Range(0, ModelSingleton.Instance.hatMap.Count);
        int idx = 0;
        string key = "Fedora";

        foreach (var keyValPair in ModelSingleton.Instance.hatMap)
        {
            if (idx == rand)
            {
                key = keyValPair.Key;
                break;
            }
            idx++;
        }

        return ModelSingleton.Instance.hatMap[key];
    }

    public void Decorate(NPC NPCParent)
    {
        Color randomColor = ColorUtil.GetRandomColor();
        randomColor = ColorUtil.HSVTransform(randomColor, 1.0f, 0.5f, 1.0f);

        NPCParent.color = randomColor;
        NPCParent.capsuleTransform.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
        NPCParent.NavMeshAgent.speed = 3.5f;

        // hat stuff
        Transform hat = NPCParent.transform.Find("Hat");
        Transform obj = hat.Find("obj");
        NPCParent.hat = GetHat();
        obj.GetComponent<MeshFilter>().mesh = NPCParent.hat;
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
    }
}