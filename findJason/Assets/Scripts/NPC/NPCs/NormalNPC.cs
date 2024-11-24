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
        Color randomColor = new Color();
        randomColor.r = Random.Range(0.0f, 1.0f);
        randomColor.g = Random.Range(0.0f, 1.0f);
        randomColor.b = Random.Range(0.0f, 1.0f);
        randomColor = ColorUtil.HSVTransform(randomColor, 1.0f, 0.5f, 1.0f);

        NPCParent.capsuleTransform.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
        NPCParent.transform.localScale = new Vector3(1.0f, Random.Range(0.2f, 1.5f), 1.0f);
        NPCParent.NavMeshAgent.speed = Random.Range(0.4f, 3.5f);

        // hat stuff
        Transform hat = NPCParent.transform.Find("Hat");
        Transform obj = hat.Find("obj");
        obj.GetComponent<MeshFilter>().mesh = GetHat();
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
    }
}