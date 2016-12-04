using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PingerCode
{
    Right,
    Left,
    Space,
    Escape,
}

public class MonoBehaviourC : MonoBehaviour {

    static bool isFirst = false;
    static bool isCheck = false;

    private static List<PingerCode> pingerBuffer;

    public MonoBehaviourC(){
        if (!isFirst)
        {
            isFirst = true;
            Debug.Log("Platform is " + Application.platform);
            pingerBuffer = new List<PingerCode>();
        }
    }

    protected IEnumerator CheckKey()
    {
        if (isCheck) yield break;
        isCheck = true;
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                AddPingerBuffer(PingerCode.Right);
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                AddPingerBuffer(PingerCode.Left);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddPingerBuffer(PingerCode.Space);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AddPingerBuffer(PingerCode.Escape);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                RemovePingerBuffer(PingerCode.Right);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                RemovePingerBuffer(PingerCode.Left);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                RemovePingerBuffer(PingerCode.Space);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                AddPingerBuffer(PingerCode.Escape);
            }

            yield return null;
        }
    }

    protected bool GetPingerDown()
    {
        if(Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        { // Editor
            return Input.GetMouseButtonDown(0);
        }
        if (Application.platform == RuntimePlatform.Android)
        { // Android  
          /*  for (int i = 0; i < 5; i++)
            {
                if (Input.GetTouch(i).phase != TouchPhase.Began) continue;
                return true;
            }
            return false;*/
            return Input.GetMouseButtonDown(0);

        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        { // ios
            /*for (int i = 0; i < 5; i++)
            {
                if (Input.GetTouch(i).phase != TouchPhase.Began) continue;
                return true;
            }
            return false;*/
            return Input.GetMouseButtonDown(0);

        }
        Debug.Log("Check your platform of device");
        return false;
    }

    protected Vector3 PingerPosition(int pinger)
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        { // Editor
            return Input.mousePosition;
        }
        if (Application.platform == RuntimePlatform.Android)
        { // android
            return Input.mousePosition;
            //return Input.GetTouch(pinger).position;
        }
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        { // ios
            return Input.GetTouch(pinger).position;
        }
        Debug.Log("Check your platform of device");
        return Vector3.zero;
    }

    protected bool GetArea(PingerCode area)
    {
        for(int i = 0; i < pingerBuffer.Count; i++)
        {
            if (pingerBuffer[i] == area) return true;
        }
        return false;
    }

    protected void AddPingerBuffer(PingerCode area)
    {
        pingerBuffer.Add(area);
    }

    protected void RemovePingerBuffer(PingerCode area)
    {
        pingerBuffer.Remove(area);
    }
}
