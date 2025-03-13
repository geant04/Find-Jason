using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("ESSENTIAL LINKABLES")]
    public GameObject viewModel; // assign this in inspector, needed to change model
    public GameObject spark;
    public GameObject tracer;

    [SerializeField] public SoundData soundData;

    [HideInInspector] public int maxAmmo, ammo;
    [HideInInspector] public float reloadTime, cooldownTime;
    [HideInInspector] public bool isAuto = false;
    [HideInInspector] public bool isActive = true;
    [HideInInspector] public GameObject viewAnim;
    [HideInInspector] public GameObject point;
    [HideInInspector] public Vector3 localPos;
    [HideInInspector] public Quaternion localRot;

    protected float reloadTimeLeft;
    protected float cooldownTimeLeft;

    public virtual void Initialize() { }
    public virtual bool Attack(Vector3 origin, Vector3 dir) 
    {
        return true;
    }

    public virtual void Animate(GameObject viewAnim, MonoBehaviour mono) {}
    public virtual bool Reload() 
    {
        return false;
    }

    public virtual void GunUpdate() { }

    public void RefillAmmo(int amt)
    {
        ammo = Mathf.Min(maxAmmo, amt + ammo);
        Debug.Log("Refilled: " + ammo);
    }
    public void SetOrigin(Vector3 origin)
    {
        localPos = origin;
    }

    public GameObject GetPoint()
    {
        if (point == null)
        {
            Transform pointTransform = viewModel.transform.Find("Point");
            point = (pointTransform) ? pointTransform.gameObject : null;
        }
        if (point != null)
        {
            Debug.Log("Found: " + point.transform.position);
        }
        return point;
    }
}

public abstract class HitScanWeapon : Gun
{
    [HideInInspector] public int bulletDmg;
    protected LineRenderer laserLine;
    protected RaycastHit hit;

    private float range = 100;

    public HitScanWeapon()
    {
        Debug.Log("HitScanWeapon Constructed");
    }

    public Enemy RayCastFire(Vector3 origin, Vector3 forward)
    {
        if (Physics.Raycast(origin, forward, out hit, range))
        {
            return hit.collider.GetComponent<Enemy>();
        }

        return null;
    }
}

public abstract class ProjectileWeapon : Gun
{
    public GameObject projectile;
}