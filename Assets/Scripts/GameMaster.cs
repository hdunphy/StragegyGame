using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerTurn { BLUE, RED }

public class GameMaster : MonoBehaviour
{
    public Unit SelectedUnit;
    public PlayerTurn playerTurn = PlayerTurn.BLUE;
    public GameObject selectedUnitSquare;
    public BarrackItem purchasedItem;
    public Image PlayerIndicater;
    public GameObject StatsPanel;
    public Vector2 StatsPanelShift;
    public Unit ViewedUnit;
    public Text HealthText, AttackText, DefenseText, CounterText;
    public Text Player1GoldText;
    public Text Player2GoldText;
    public Sprite player1Indicator;
    public Sprite player2Indicator;
    public int Player1Gold = 100;
    public int Player2Gold = 100;

    private void Start()
    {
        GetGoldIncome(playerTurn);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            EndTurn();

        if (SelectedUnit != null)
        {
            selectedUnitSquare.SetActive(true);
            selectedUnitSquare.transform.position = SelectedUnit.transform.position;
        }
        else
            selectedUnitSquare.SetActive(false);
    }

    public void ToggleStatsPanel(Unit unit)
    {
        if(unit.Equals(ViewedUnit) == false)
        {
            StatsPanel.SetActive(true);
            StatsPanel.transform.position = (Vector2)unit.transform.position + StatsPanelShift;
            ViewedUnit = unit;
            UpdateStatsPanel();
        }
        else
        {
            StatsPanel.SetActive(false);
            ViewedUnit = null;
        }
    }

    public void UpdateStatsPanel()
    {
        if (ViewedUnit != null)
        {
            HealthText.text = ViewedUnit.health.ToString();
            AttackText.text = ViewedUnit.attackDamage.ToString();
            DefenseText.text = ViewedUnit.armor.ToString();
            CounterText.text = ViewedUnit.DefenseDamage.ToString();
        }
    }

    public void MoveStatsPanel(Unit unit)
    {
        if (unit.Equals(ViewedUnit))
        {
            StatsPanel.transform.position = (Vector2)unit.transform.position + StatsPanelShift;
        }
    }

    public void RemoveStatsPanel(Unit unit)
    {
        if (unit.Equals(ViewedUnit))
        {
            StatsPanel.SetActive(false);
            ViewedUnit = null;
        }
    }

    public void UpdateGoldText()
    {
        Player1GoldText.text = Player1Gold.ToString();
        Player2GoldText.text = Player2Gold.ToString();
    }

    void GetGoldIncome(PlayerTurn turn)
    {
        foreach(Village village in FindObjectsOfType<Village>())
        {
            if(village.Player == turn)
            {
                Player1Gold += village.GoldPerTurn;
            }
        }

        UpdateGoldText();
    }

    private void EndTurn()
    {
        playerTurn = playerTurn == PlayerTurn.BLUE ? PlayerTurn.RED : PlayerTurn.BLUE;
        PlayerIndicater.sprite = playerTurn == PlayerTurn.BLUE ? player1Indicator : player2Indicator;
        GetGoldIncome(playerTurn);

        SetSelectedUnit(null);

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            unit.hasMoved = false;
            unit.weaponIcon.SetActive(false);
            unit.hasAttacked = false;
        }

        GetComponent<Barrack>().CloseMenus();
    }

    public void SetSelectedUnit(Unit unit)
    {
        if (SelectedUnit != null)
            SelectedUnit.isSelected = false;
        if (unit != null)
            unit.isSelected = true;
        SelectedUnit = unit;
        ResetTiles();
    }

    public void ResetTiles()
    {
        foreach (Tile tile in FindObjectsOfType<Tile>())
            tile.ResetTile();
    }
}
