using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jason : NPCType
{
    public Jason() { }

    public TaskBase StartTask(NPC NPCParent)
    {
        return new JasonIdleTask(NPCParent);
    }

    public void Decorate(NPC NPCParent)
    {
        Transform transform = NPCParent.capsuleTransform;
        GameObject badge = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        badge.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        badge.transform.position = transform.position + transform.forward * 0.50f;
        badge.transform.parent = transform;

        Color randomColor = new Color();
        randomColor.r = Random.Range(0.0f, 1.0f);
        randomColor.g = Random.Range(0.0f, 1.0f);
        randomColor.b = Random.Range(0.0f, 1.0f);
        randomColor = ColorUtil.HSVTransform(randomColor, 1.0f, 3.0f, 1.0f);

        NPCParent.capsuleTransform.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor);
        NPCParent.color = randomColor;
        NPCParent.SetSpeed(3.5f);

        // hat stuff
        Transform hat = NPCParent.transform.Find("Hat");
        Transform obj = hat.Find("obj");
        obj.GetComponent<MeshRenderer>().material.SetColor("_Color", randomColor); // add "wear"
    }
}
