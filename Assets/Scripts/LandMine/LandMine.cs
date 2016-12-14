using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LandMineCollider))]
public class LandMine : MonoBehaviour
{
	public struct AttackInfo
	{
		[SerializeField]
		private int _damage;
		[SerializeField]
		private float _delay;

		public AttackInfo(int damage, float delay)
		{
			_damage = damage;
			_delay = delay;
		}

		public int damage
		{
			get { return _damage; }
		}

		public float delay
		{
			get { return _delay; }
		}
	}

	public struct UpgradeInfo
	{
		private int _upgradeUnit;

		private LandMineUpgrade[] _upgradeData;

		public UpgradeInfo(LandMineUpgrade[] upgradeData) {
			_upgradeUnit = 0;
			_upgradeData = upgradeData;
		}

		public bool HasNext()
		{
			return (_upgradeUnit < _upgradeData.Length);
		}

		public bool HasPrevious() 
		{
			return (_upgradeUnit > 0);
		}

		public LandMineUpgrade MoveNext()
		{
			CheckError(!HasPrevious());
			return _upgradeData[_upgradeUnit++];
		}

		public LandMineUpgrade MovePrevious() 
		{
			CheckError(!HasNext());
			return _upgradeData[--_upgradeUnit];
		}

		private void CheckError(bool error) {
			if (error)
			{
				throw new UnityException("can't return upgrade data.");
			}
		}

	}

	private string _name;

	private AttackInfo _attackInfo;
	private UpgradeInfo _upgradeInfo;

	private int _maxDuration;
	private int _duration;

	private LandMineDurationSlider _durationSlider;

	public AttackInfo attackInfo 
	{
		get { return _attackInfo; }
	}

	public UpgradeInfo upgradeInfo
	{
		get { return _upgradeInfo; }
	}

	public MineState mineState
	{
		get
		{
			if (CanRepair())
			{
				return upgradeInfo.HasNext() ? MineState.Selected : MineState.FullUpgrade;
			}
			else
			{
				return upgradeInfo.HasNext() ? MineState.SelectedFullDurability : MineState.FullUpgradeFullDurability;
			}
		}
	}

	public void Init(LandMineData data, LandMineDurationSlider originalSlider)
	{
		_attackInfo = new AttackInfo(data.damage, data.delay);
		_upgradeInfo = new UpgradeInfo(data.upgradeData);

		_maxDuration = data.duration;
		_duration = _maxDuration;

		_durationSlider = Instantiate(originalSlider);
		_durationSlider.Init(_maxDuration);

		Transform sliderTransform = _durationSlider.transform;
		sliderTransform.SetParent(transform);
		sliderTransform.localPosition = Vector3.up;
	}

	public void Repair()
	{
		if (!CanRepair())
		{
			return;
		}

		_duration= _maxDuration;
		_durationSlider.Set(_duration);
		StartCoroutine(AlphaColor(1f, 0.8f));
	}

	public bool CanRepair()
	{
		return _duration < _maxDuration;
	}

	public bool IsDeath()
	{
		return _duration <= 0;
	}

	public void Attack(Enemy enemy) {
		enemy.Damage(attackInfo.damage);

		if (--_duration <= 0)
		{
			Death();
		}

		_durationSlider.Set(_duration);
	}

	public LandMineUpgrade Upgrade()
	{
		LandMineUpgrade upgrade = _upgradeInfo.MoveNext();
		_attackInfo = new AttackInfo(upgrade.damage, upgrade.delay);

		return upgrade;
	}

	public LandMineUpgrade Downgrade()
	{
		LandMineUpgrade downgrade = _upgradeInfo.MovePrevious();
		_attackInfo = new AttackInfo(downgrade.damage, downgrade.delay);

		return downgrade;
	}

	private void Death()
	{
		_duration = 0;

		StartCoroutine(AlphaColor(0.2f, 0.8f));
	}

	private IEnumerator AlphaColor(float alpha, float time)
	{
		Material material = GetComponent<MeshRenderer>().material;
		Color color = material.color;

		int loopSize = (int) (30f * time);
		WaitForSeconds loopDelay = new WaitForSeconds(time / loopSize);

		float addValue = (alpha - color.a) / (float) loopSize;
		alpha = color.a;

		for (int i = 0; i < loopSize; i++)
		{
			alpha += addValue;
			material.color = new Color(color.r, color.g, color.b, alpha);
			yield return loopDelay;
		}

	}
}