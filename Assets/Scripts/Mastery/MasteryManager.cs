using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasteryManager : MonoBehaviour {

    public static MasteryManager instance;

    public MasteryCard[] _card;

    private int[] _cardNumber = new int[3];

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : MasteryManager");
        }
        instance = this;
    }

    public void Start()
    {
        _cardNumber[0] = UserManager.instance.GetUserData(CardKey.Type1);
        _cardNumber[1] = UserManager.instance.GetUserData(CardKey.Type2);
        _cardNumber[2] = UserManager.instance.GetUserData(CardKey.Type3);
    }

    public void Test(int slotNumber)
    {
        _card[slotNumber].TurnCard(true);
    }
}
