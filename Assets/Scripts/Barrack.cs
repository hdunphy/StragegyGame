using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrack : MonoBehaviour
{

    public Button player1ToggleButton, player2ToggleButton;
    public GameObject Player1Menu, Player2Menu;

    GameMaster gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm.playerTurn == PlayerTurn.BLUE)
        {
            player1ToggleButton.interactable = true;
            player2ToggleButton.interactable = false;
        }
        else
        {
            player1ToggleButton.interactable = false;
            player2ToggleButton.interactable = true;
        }
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void CloseMenus()
    {
        Player1Menu.SetActive(false);
        Player2Menu.SetActive(false);
    }

    public void BuyItem(BarrackItem item)
    {
        if(gm.playerTurn == PlayerTurn.BLUE && item.Cost <= gm.Player1Gold)
        {
            gm.Player1Gold -= item.Cost;
            Player1Menu.SetActive(false);
        } else if (gm.playerTurn == PlayerTurn.RED && item.Cost <= gm.Player2Gold)
        {
            gm.Player2Gold -= item.Cost;
            Player2Menu.SetActive(false);
        }
        else
        {
            return;
        }

        gm.UpdateGoldText();
        gm.purchasedItem = item;

        gm.SetSelectedUnit(null);

        GetCreatableTiles();

    }

    void GetCreatableTiles()
    {
        foreach(Tile tile in FindObjectsOfType<Tile>())
        {
            if (tile.IsClear())
            {
                tile.SetCreatable();
            }
        }
    }
}
