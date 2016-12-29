using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum UserKey
{
    Level,      // int
    Exp,        // int
    Ticket,     // int

    PlayTime,   // int
}

public enum GameKey
{
    Mode,       // int 
    Exp,        // int
    Money,      // int
    Time,       // int
    OverType,   // int
}

public enum CardKey
{
    Type1, Type2, Type3,
    Rank1, Rank2, Rank3,
    Level1, Level2, Level3,
}

public class UserManager : MonoBehaviour {

    public static UserManager instance;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : UserManager");
        }
        instance = this;

        FirstGame();
    }

	void Start ()
    {
        Debug.Log("User Manager is Start");
    }

    private void FirstGame()
    {
        if (PlayerPrefs.HasKey("UserKey" + UserKey.PlayTime.ToString())) return;

        Debug.Log("First Running");

        SetUserData(UserKey.PlayTime, 0);

        SetUserData(UserKey.Level, 0);
        SetUserData(UserKey.Exp, 0);
        SetUserData(UserKey.Ticket, 0);

        SetUserData(CardKey.Type1, 0);
        SetUserData(CardKey.Type2, 0);
        SetUserData(CardKey.Type3, 0);
    }

    public void SetUserData(GameKey key, int value) // save only int
    {
        string inputKey = null;

        inputKey = "GameKey" + key.ToString();

        PlayerPrefs.SetInt(inputKey, value);
        PlayerPrefs.Save();
    }

    public void SetUserData(UserKey key, int value) // save only int
    {
        string inputKey = null;

        inputKey = "UserKey" + key.ToString();

        PlayerPrefs.SetInt(inputKey, value);
        PlayerPrefs.Save();
    }

    public void SetUserData(CardKey key, int value) // save only int
    {
        string inputKey = null;

        inputKey = "CardKey" + key.ToString();

        PlayerPrefs.SetInt(inputKey, value);
        PlayerPrefs.Save();
    }

    public int GetUserData(GameKey key)
    {
        string inputKey = null;
        int result;

        inputKey = "GameKey" + key.ToString();

        result = PlayerPrefs.GetInt(inputKey);

        PlayerPrefs.DeleteKey(key.ToString());
        PlayerPrefs.Save();

        return result;
    }

    public int GetUserData(UserKey key)
    {
        string inputKey = null;
        int result;

        inputKey = "UserKey" + key.ToString();

        result = PlayerPrefs.GetInt(inputKey);

        return result;
    }

    public int GetUserData(CardKey key)
    {
        string inputKey = null;
        int result;

        inputKey = "CardKey" + key.ToString();

        result = PlayerPrefs.GetInt(inputKey);

        return result;
    }

    public void ClearUserData(bool check = false)
    {
        if (!check) return;

        Debug.Log("CLEAR DATA");

        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
