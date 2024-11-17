using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ModelSingleton : MonoBehaviour
{
    public static ModelSingleton Instance { get; private set; }

    public Dictionary<string, Mesh> hatMap = new Dictionary<string, Mesh>();
    public NameHat[] nameHatList = { };

    [Serializable]
    public struct NameHat
    {
        public string name;
        public Mesh mesh;
    }

    void populateDict()
    {
        for (int i = 0; i < nameHatList.Length; i++)
        {
            hatMap.Add(nameHatList[i].name, nameHatList[i].mesh);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        populateDict();
        Debug.Log(nameHatList.Length);
    }
}
