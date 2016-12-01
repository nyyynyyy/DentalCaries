using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("Game State")]
    [SerializeField] private int _round = 1;
    [SerializeField] private int _money = 0;

    [Header("Player State")]
    [SerializeField] private float _health = 1000;
    [SerializeField] private float _power = 1f;

    private bool _isGame = false;

    public int money
    {
        get
        {
            return _money;
        }
    }
    public float hp
    {
        get
        {
            return _health;
        }
    }
    public int round
    {
        get
        {
            return _round;
        }
    }
    public float power
    {
        get
        {
            return _power;
        }
    }

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : GameManger");
        }
        instance = this;
    }

    void Start () {
        Debug.Log("GAME MANAGER IS READY");

        TextManager.instance.ResetUI();
    }

    public void TakeMoney(int pay)
    {
        _money += pay;
        TextManager.instance.ViewGold();
    }

    public void Damage(float damage)
    {
        _health -= damage;
        TextManager.instance.ViewHp();
        if (_health <= 0) GameOver();
    }

    public bool IsGame()
    {
        return _isGame;
    }

    public void GameOver()
    {
        _isGame = false;
    }

    public void RoundClear()
    {
        _round++;
    }

    public void PowerUp()
    {
        int pay = 100;
        if (_money >= pay)
        {
            _power++;
            _money -= pay;
        }
        TextManager.instance.ViewAttack();
        TextManager.instance.ViewGold();
    }
}
