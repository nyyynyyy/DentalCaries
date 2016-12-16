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

		public int level { get { return _upgradeUnit; } }

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
			CheckError(!HasNext());
			return _upgradeData[_upgradeUnit++];
		}

		public LandMineUpgrade MovePrevious() 
		{
			CheckError(!HasPrevious());
			return _upgradeData[--_upgradeUnit];
		}

		public LandMineUpgrade GetNext()
		{
			if (HasNext())
			{
				return _upgradeData[_upgradeUnit];
			}
			else
			{
				return null;
			}
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

	private int _maxDurability;
	private int _durability;

	//private LandMineDurabilitySlider _durabilitySlider;

	public AttackInfo attackInfo 
	{
		get { return _attackInfo; }
	}

	public UpgradeInfo upgradeInfo
	{
		get { return _upgradeInfo; }
	}

	public int maxDurability { get { return _maxDurability; } }
	public int durability { get { return _durability; } }

	public void Init(LandMineData data)
	{
		_attackInfo = new AttackInfo(data.damage, data.delay);
		_upgradeInfo = new UpgradeInfo(data.upgradeData);

		_maxDurability = data.durability;
		_durability = _maxDurability;

		//_durabilitySlider = Instantiate(originalSlider);
		//_durabilitySlider.Init(_maxDurability);

		//Transform sliderTransform = _durabilitySlider.transform;
		//sliderTransform.SetParent(transform);
		//sliderTransform.localPosition = Vector3.up;
	}

	public void Repair()
	{
		if (!CanRepair())
		{
			return;
		}

		_durability= _maxDurability;
		//_durabilitySlider.Set(_durability);
		StartCoroutine(AlphaColor(1f, 0.8f));
	}

	public bool CanRepair()
	{
		return _durability < _maxDurability;
	}

	public bool IsDeath()
	{
		return _durability <= 0;
	}

	public void Attack(Enemy enemy) {
		enemy.Damage(attackInfo.damage);

		if (--_durability <= 0)
		{
			Death();
		}

		//_durabilitySlider.Set(_durability);
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
		_durability = 0;

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