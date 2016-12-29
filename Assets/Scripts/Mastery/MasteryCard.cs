using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasteryCard : MonoBehaviour {

  //  private bool _existMastery;

    public GameObject _masteryCard;
    public GameObject _emptyCard;

    public Image _cardArtImage;
    public Text _cardNameText;
    public Text _cardDescriptionText;
    public Text _cardLevelText;
    public Text _cardRankText;

    private MasteryType _cardType;
    private MasteryRank _cardRank;
    private MasteryLevel _cardLevel;

	public void TurnCard(bool turn)
    {
        _emptyCard.SetActive(!turn);
        _masteryCard.SetActive(turn);
    }
}
