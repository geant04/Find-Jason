using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Sampler
{
    public virtual Vector3 GetSample(NPC ParentNPC)
    {
        float randOut = Random.Range(5.0f, 40.0f);
        Vector3 randomDirection = Random.insideUnitSphere * randOut;
        randomDirection.y = 0.0f;

        return randomDirection;
    }
}

public class JasonSampler : Sampler
{
    public override Vector3 GetSample(NPC ParentNPC)
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0.0f;

        Vector3 toJason = GameManager.Jason.transform.position - ParentNPC.GetPosition();
        float randOut = Random.Range(0.0f, 2.0f) + toJason.magnitude * Random.Range(0.0f, 1.0f);

        return Vector3.Normalize(randomDirection + toJason) * randOut;
    }
}

public class LeaderSampler : Sampler
{
    private NPC target;

    public LeaderSampler(NPC target) { this.target = target; }
    public override Vector3 GetSample(NPC ParentNPC)
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0.0f;

        Vector3 toTarget = target.GetPosition() - ParentNPC.GetPosition();
        float randOut = Random.Range(0.0f, 0.2f) + toTarget.magnitude * Random.Range(0.0f, 1.0f);

        Vector3 ongod = Vector3.Normalize((toTarget.magnitude * 0.4f) * randomDirection + toTarget) * randOut;
        // space it out a bit
        ongod -= Random.Range(0.2f, 0.6f) * Vector3.Normalize(toTarget);
        ongod.y = 0.0f;
        return ongod;
    }
}
