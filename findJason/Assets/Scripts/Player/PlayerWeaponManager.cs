using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponManager : MonoBehaviour
{
    public Player player;
    public GameObject viewModel;
    public List<GameObject> inventory;
    [HideInInspector] public GameObject weapon;

    private int index = 0;
    private PlayerWeaponController playerWeaponController;

    void Awake()
    {
        playerWeaponController = player.playerWeaponController;
        weapon = inventory[index];

        foreach(var weapon in inventory)
        {
            weapon.GetComponent<Gun>().Initialize();
        }
    }

    public void ResetView()
    {
        Transform parent = viewModel.transform.parent;
        Gun gun = weapon.GetComponent<Gun>();

        Destroy(viewModel); // this is bad. This is bad. Don't do this. - Anthony
        viewModel = Instantiate(gun.viewModel, parent);

        if (player != null)
        {
            gun.Animate(playerWeaponController.viewAnim, this);

            Transform viewModelPoint = viewModel.transform.Find("Point");

            if (viewModelPoint)
            {
                playerWeaponController.weaponPointLocalPos =
                    viewModelPoint.transform.localPosition + gun.localPos;
            } else
            {
                playerWeaponController.weaponPointLocalPos = Vector3.zero;
            }

            Debug.Log(playerWeaponController.weaponPointLocalPos);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int idxChange = -1;

        for (int i = 1; i < inventory.Count + 1; i++)
        {
            if (Input.GetKeyDown( "" + i ))
            {
                idxChange = i - 1;
            }
        } 

        if (idxChange != -1)
        {
            /*int temp = index;
            index = (index + idxChange) % inventory.Count;
            if (index < 0) index += inventory.Count;*/
            
            weapon = inventory[idxChange];
            ResetView();
        }
    }
}
