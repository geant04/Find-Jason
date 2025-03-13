using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : HitScanWeapon
{
    [HideInInspector] public GameObject bundle, flash;

    public override void Initialize()
    {
        Debug.Log("Initialize Pistol");
        bulletDmg = 20;
        maxAmmo = 50;
        ammo = maxAmmo;
        reloadTime = 0.125f;
        localPos = new Vector3(0.2f, 0, 0.5f);
        laserLine = GetComponent<LineRenderer>();
    }

    public override bool Attack(Vector3 origin, Vector3 dir)
    {
        if (reloadTimeLeft > 0 || ammo <= 0)
        {
            return false;
        }

        ammo--;
        reloadTimeLeft = reloadTime;

        dir = Vector3.Normalize(dir + 0.04f * Random.insideUnitSphere);

        Enemy target = RayCastFire(origin, dir);

        // cool tracer
        GameObject trace = Instantiate(tracer);
        float mag = Mathf.Max(Vector3.Distance(hit.point, origin), 40.0f);
        trace.GetComponent<LineRenderer>().SetPosition(0, origin);
        trace.GetComponent<LineRenderer>().SetPosition(1, origin + mag * dir);
        Destroy(trace, 0.2f);

        if (hit.collider != null)
        {
            if (target != null)
            {
                target.TakeDamage(bulletDmg);
            }

            // sus code for hit
            GameObject sp = Object.Instantiate(spark, hit.point, Quaternion.LookRotation(dir));
            Object.Destroy(sp, 0.2f);
        }

        return true;
    }

    public override void Animate(GameObject viewAnim, MonoBehaviour mono)
    {
        mono.StartCoroutine(Recoil(viewAnim, reloadTime * 2.0f));
    }

    public IEnumerator Recoil(GameObject viewAnim, float waitTime)
    {
        float time = 0;

        Vector3 flick = new Vector3(0, 0.5f, 1);
        Vector3.Normalize(flick);
        Quaternion flickRot = Quaternion.LookRotation(flick, Vector3.up);

        while (time < waitTime)
        {
            // move by local forward
            float t = (time / waitTime); // [0 1]

            t = Mathf.Pow(t, 12.0f);
            
            Vector3 delta = new Vector3(0.0f, 0.0f, -1.0f) * Mathf.Lerp(0.15f, 0.0f, t);

            viewAnim.transform.localPosition = localPos + delta;
            viewAnim.transform.localRotation = Quaternion.Slerp(flickRot, localRot, t);

            time += Time.deltaTime;
            yield return null;
        }

        viewAnim.transform.localPosition = localPos; // reset
        viewAnim.transform.localRotation = localRot;

        yield return null;
    }

    public override bool Reload()
    {
        return true;
    }

    public override void GunUpdate()
    {
        //cooldownTime = Mathf.Max(0.0f, cooldownTime - Time.deltaTime);
        reloadTimeLeft = Mathf.Max(0.0f, reloadTimeLeft - Time.deltaTime);
    }
}
