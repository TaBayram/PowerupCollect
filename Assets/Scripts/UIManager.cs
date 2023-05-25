using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Unit player, ai;

    public TMP_Text playerScoreText, AIScoreText;
    public TMP_Text playerTotalText, AIScoreTotalText;

    void Start()
    {
        player.onScoreChange += onPlayerScoreChange;
        ai.onScoreChange += onAIScoreChange;

        player.onRoundEnd += onRoundEnd;
        ai.onRoundEnd += onRoundEnd;
        
    }

    void Update()
    {
        
    }

    public void onPlayerScoreChange(Unit unit) {
        playerScoreText.text ="Player "+ unit.Score.ToString();
    }

    public void onAIScoreChange(Unit unit) {
        AIScoreText.text ="AI " + unit.Score.ToString();
    }
    public void onRoundEnd(bool hasWon) {
        playerTotalText.text = "W: " + player.winCount + " L: " + player.lossCount;
        AIScoreTotalText.text = "W: " + ai.winCount + " L: " + ai.lossCount;
    }
}
