using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    [Header("UI")]
    public Text hpTxt;

    [Header("Game State")]
    [SerializeField] private int _round = 1;

    [Header("Player State")]
    public float _health = 1000;
    public float _power = 1f;

    private bool _isGame = false;

    void Awake()
    {
        if (instance)
        {
            Debug.LogError("Multi Instance Running.. : GameManger");
        }
        instance = this;
    }

    void Start () {
        Debug.Log("GAME MANGER IS READY");

        Init();
    }

    private void Init()
    {
        UIReset();   
    }

    private void UIReset()
    {
        hpTxt.text = _health.ToString();
    }

    public void Damage(float damage)
    {
        _health -= damage;
        UIReset();
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

    public int NowRound()
    {
        return _round;
    }

    public void RoundClear()
    {
        _round++;
    }
}
