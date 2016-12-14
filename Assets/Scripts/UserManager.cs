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

    Mastery1,   // int
    Mastery2,   // int
    Mastery3,   // int

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

public class UserManager : MonoBehaviour {

    public static UserManager instance;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : UserManager");
        }
        instance = this;
    }

	void Start ()
    {
        Debug.Log("User Manager is Start");
        FirstGame();
    }

    private void FirstGame()
    {
        if (PlayerPrefs.HasKey("UserKey" + UserKey.PlayTime.ToString())) return;

        Debug.Log("First Running");
        SetUserData(UserKey.PlayTime, 0);
        SetUserData(UserKey.Level, 0);
        SetUserData(UserKey.Exp, 0);
        SetUserData(UserKey.Ticket, 0);
        SetUserData(UserKey.Mastery1, 0);
        SetUserData(UserKey.Mastery2, 0);
        SetUserData(UserKey.Mastery3, 0);
    }

    public void SetUserData(Enum key, int value) // save only int
    {
        if (!(key is GameKey) && !(key is UserKey))
        {
            Debug.LogError("Enum Type Error");
            return;
        }

        string inputKey = null;

        if (key is GameKey)
        {
            inputKey = "GameKey" + key.ToString();
        }
        if(key is UserKey)
        {
            inputKey = "UserKey" + key.ToString();
        }

        PlayerPrefs.SetInt(inputKey, value);
        PlayerPrefs.Save();
    }

    public int GetUserData(Enum key)
    {
        if(!(key is GameKey) && !(key is UserKey))
        {
            Debug.LogError("Enum Type Error");
            return 0;
        }

        string inputKey = null;
        int result;

        if (key is GameKey)
        {
            inputKey = "GameKey" + key.ToString();
        }
        if (key is UserKey)
        {
            inputKey = "UserKey" + key.ToString();
        }

        result = PlayerPrefs.GetInt(inputKey);

        if (key is GameKey)
        {
            PlayerPrefs.DeleteKey(key.ToString());
            PlayerPrefs.Save();
        }

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
