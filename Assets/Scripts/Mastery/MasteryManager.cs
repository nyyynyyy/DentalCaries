using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasteryManager : MonoBehaviour {

    public static MasteryManager instance;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : MasteryManager");
        }
        instance = this;
    }

    public void Test(int slotNumber)
    {
        Debug.Log(slotNumber + " click");
    }
}
