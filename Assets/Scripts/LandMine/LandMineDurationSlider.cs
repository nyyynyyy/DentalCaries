using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Canvas))]
public class LandMineDurationSlider : MonoBehaviour
{

	private Slider _slider;

	void Awake()
	{
		_slider = transform.GetChild(0).GetComponent<Slider>();
		Debug.Log(_slider);
	}

	public void Init(int maxDuration)
	{
		_slider.maxValue = maxDuration;
		_slider.minValue = 0;

		_slider.value = maxDuration;
	}

	public void Set(int value) { 
		if (ViewManager.instance.viewMode == ViewType.MINE && value != (int) _slider.value)
		{
			StartCoroutine(SmoothSetValue(value, 1f));
		}
		else
		{
			_slider.value = value;
		}
	}

	private IEnumerator SmoothSetValue(int value, float time) 
	{
		float normalizeValue = value / _slider.maxValue;

		int loopSize = (int) (60f * time);
		WaitForSeconds loopDelay = new WaitForSeconds(time / loopSize);

		float addValue = (normalizeValue - _slider.normalizedValue) / (float) loopSize;
		normalizeValue = _slider.normalizedValue;

		for (int i = 0; i < loopSize; i++)
		{
			normalizeValue += addValue;
			_slider.normalizedValue = normalizeValue;
			yield return loopDelay;
		}
	}
}
