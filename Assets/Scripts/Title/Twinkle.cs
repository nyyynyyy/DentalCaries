using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Twinkle : MonoBehaviour {

    public float speed = 0.1f;

    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        StartCoroutine(TextTwinkle());
	}
	
	private IEnumerator TextTwinkle()
    {
        while (true)
        { 
            while (speed>0? text.color.a < 1 : text.color.a > 0)
            {
                Color color = text.color;
                color.a += speed;
                text.color = color;
                yield return new WaitForSeconds(0.05f);
            }
            speed *= -1;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
