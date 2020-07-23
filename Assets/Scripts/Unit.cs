using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    GameMaster gameMaster;
    Animator CameraAnimator;

    [Header("Description")]
    public PlayerTurn playerTurn;
    public GameObject weaponIcon;
    public GameObject damageIcon;
    public GameObject deathEffect;
    public Text KingHealth;
    public bool isKing;

    [Header("Movement Stats")]
    public bool hasMoved;
    public int tileSpeed;
    public float moveSpeed;

    [Header("Attack Stats")]
    public int attackRange;
    public int health, attackDamage, DefenseDamage, armor;

    [Header("Internal Variables")]
    public bool isSelected = false;
    List<Unit> enemiesInRange = new List<Unit>();
    public bool hasAttacked;

    // Start is called before the first frame update
    void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
        CameraAnimator = Camera.main.GetComponent<Animator>();
        UpdateKingHealth();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            gameMaster.ToggleStatsPanel(this);
        }
    }

    private void OnMouseDown()
    {
        ResetWeaponIcons();

        if (isSelected)
            gameMaster.SetSelectedUnit(null);
        else if (playerTurn == gameMaster.playerTurn)
        {
            gameMaster.SetSelectedUnit(this);
            GetEnemies();
            GetWalkableTiles();
        }

        Collider2D col = Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        Unit unit = col.GetComponent<Unit>();
        if (gameMaster.SelectedUnit != null
            && gameMaster.SelectedUnit.enemiesInRange.Contains(unit)
            && !gameMaster.SelectedUnit.hasAttacked)
        {
            gameMaster.SelectedUnit.Attack(unit);
        }
    }

    public void UpdateKingHealth()
    {
        if (isKing)
        {
            KingHealth.text = health.ToString();
        }
    }

    private void Attack(Unit enemy)
    {
        CameraAnimator.SetTrigger("Attack");

        hasAttacked = true;
        int enemyDamage = attackDamage - enemy.armor;
        int myDamage = enemy.DefenseDamage - armor;
        float enemyDistance = Math.Abs(transform.position.x - enemy.transform.position.x) + Math.Abs(transform.position.y - enemy.transform.position.y);

        if (enemyDamage >= 1)
        {
            enemy.health -= enemyDamage;
            GameObject instance = Instantiate(damageIcon, enemy.transform.position, Quaternion.identity);
            instance.GetComponent<DamageIcon>().Setup(enemyDamage);
            enemy.UpdateKingHealth();
        }
        if (myDamage >= 1 && enemy.attackRange >= enemyDistance)
        {
            health -= myDamage;
            GameObject instance = Instantiate(damageIcon, transform.position, Quaternion.identity);
            instance.GetComponent<DamageIcon>().Setup(myDamage);
            UpdateKingHealth();
        }

        if(enemy.health <= 0)
        {
            GetWalkableTiles();
            Destroy(enemy.gameObject);
            Instantiate(deathEffect, enemy.transform.position, Quaternion.identity);
            gameMaster.RemoveStatsPanel(enemy);
        }
        if(health <= 0)
        {
            gameMaster.ResetTiles();
            Instantiate(deathEffect, transform.position, Quaternion.identity);
            gameMaster.RemoveStatsPanel(this);
            Destroy(this.gameObject);
        }

        gameMaster.UpdateStatsPanel();
    }

    private void GetWalkableTiles()
    {
        if (hasMoved)
        {
            return;
        }

        foreach (Tile tile in FindObjectsOfType<Tile>())
        {
            float tileDistance = Math.Abs(transform.position.x - tile.transform.position.x) + Math.Abs(transform.position.y - tile.transform.position.y);
            if (tileDistance <= tileSpeed && tile.IsClear())
            {
                tile.Highlight();
            }
        }
    }

    void GetEnemies()
    {
        enemiesInRange.Clear();

        foreach (Unit unit in FindObjectsOfType<Unit>())
        {
            float tileDistance = Math.Abs(transform.position.x - unit.transform.position.x) + Math.Abs(transform.position.y - unit.transform.position.y);
            if (tileDistance <= attackRange && unit.playerTurn != gameMaster.playerTurn && !hasAttacked)
            {
                enemiesInRange.Add(unit);
                unit.weaponIcon.SetActive(true);
            }
        }
    }

    private void ResetWeaponIcons()
    {
        foreach (Unit unit in FindObjectsOfType<Unit>())
            unit.weaponIcon.SetActive(false);
    }

    public void Move(Vector2 tilePos)
    {
        gameMaster.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    private IEnumerator StartMovement(Vector2 tilePos)
    {
        while (transform.position.x != tilePos.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        while (transform.position.y != tilePos.y)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePos.y), moveSpeed * Time.deltaTime);
            yield return null;
        }

        hasMoved = true;
        ResetWeaponIcons();
        GetEnemies();
        gameMaster.MoveStatsPanel(this);
    }
}
