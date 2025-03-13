using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWeaponController : MonoBehaviour
{
    // TO DO: if you want to abstract this further, just make it base class inherit from a weapon class
    public Gun weapon;
    public Player player;
    public GameObject defaultOriginPoint;
    public GameObject viewAnim;
    public GameObject flash;

    private PlayerWeaponManager playerWeaponManager;
    [HideInInspector] public Vector3 defaultPointLocalPos;
    [HideInInspector] public Vector3 weaponPointLocalPos; // assigned by playerWeaponManager

    public void Awake()
    {
        if (player == null) return;
        playerWeaponManager = player.playerWeaponManager;
        playerWeaponManager.ResetView();

        defaultPointLocalPos = defaultOriginPoint.transform.localPosition;
    }

    public void AssignText()
    {
        if (player.playerUIManager)
        {
            player.playerUIManager.SetAmmo($"{weapon.ammo} / {weapon.maxAmmo}");
        }
    }

    private void Update()
    {
        if (playerWeaponManager == null) return;
        weapon = playerWeaponManager.weapon.GetComponent<Gun>();

        GameObject origin = defaultOriginPoint;
        origin.transform.localPosition = defaultPointLocalPos;

        if (weaponPointLocalPos != Vector3.zero)
            origin.transform.localPosition = weaponPointLocalPos;

        bool isAuto = weapon.isAuto && Input.GetKey(KeyCode.Mouse0);

        if (isAuto || Input.GetKeyDown(KeyCode.Mouse0))
        {
            bool fired = weapon.Attack(origin.transform.position, origin.transform.forward);
            if (fired)
            {
                weapon.Animate(viewAnim, this);
                GameObject fx = Instantiate(flash, origin.transform.position, Quaternion.LookRotation(origin.transform.forward));
                fx.transform.Rotate(transform.forward, Random.Range(0.0f, 360.0f));
                Destroy(fx, 0.1f); // do a pool system honestly

                SoundManager.Instance.CreateSound()
                    .WithSoundData(weapon.soundData)
                    .WithRandomPitch()
                    .WithPosition(origin.transform.position)
                    .Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.Reload();
        }

        // silly
        AssignText();

        Debug.DrawRay(origin.transform.position, origin.transform.forward, Color.cyan);
        weapon.GunUpdate();
    }   
}

