using System.Collections.Generic;

using TMPro;

using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Monster> monsterPrefabs;

    [SerializeField] private Transform monsterSlotA;
    [SerializeField] private Transform monsterSlotB;
    
    [SerializeField] private MonsterUI monsterAUI;
    [SerializeField] private MonsterUI monsterBUI;

    [SerializeField] private TextMeshProUGUI commentaryText;

    private GameInput input;
    
    private Monster monsterA;
    private Monster monsterB;

    private bool isMonsterATurn = true;

    private void Awake()
    {
        input = new GameInput();
        input.Player.Next.performed += PerformNextAction;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Start()
    {
        StartNewBattle();
    }
    private void StartNewBattle()
    {
        int challengerAIndex = Random.Range(0, monsterPrefabs.Count);
        int challengerBIndex = Random.Range(0, monsterPrefabs.Count);

        // Try again if picking the same two monsters.
        if (monsterPrefabs.Count >= 2)
        {
            while (challengerAIndex == challengerBIndex)
            {
                challengerBIndex = Random.Range(0, monsterPrefabs.Count);
            }
        }
        
        // Use the selected number to pick the corresponding monster.
        Monster challengerA = monsterPrefabs[challengerAIndex];
        Monster challengerB = monsterPrefabs[challengerBIndex];

        monsterA = RegisterNewMonster(challengerA, monsterSlotA, monsterAUI);
        monsterB = RegisterNewMonster(challengerB, monsterSlotB, monsterBUI);
        
        commentaryText.SetText($"{monsterA.GetTitle()} trifft auf {monsterB.GetTitle()}!");
    }

    private void OnDestroy()
    {
        input.Player.Next.performed -= PerformNextAction;
    }

    private void PerformNextAction(InputAction.CallbackContext context)
    {
        if (monsterA.HasFainted() || monsterB.HasFainted())
        {
            StartNewBattle();
            return;
        }

        Monster attacker;
        Monster defender; //attackee
        MonsterUI defenderUI;
        
        if (isMonsterATurn) // Monster A greift an.
        {
            attacker = monsterA;
            defender = monsterB;
            defenderUI = monsterBUI;
        }
        else // Monster B greift an.
        {
            attacker = monsterB;
            defender = monsterA;
            defenderUI = monsterAUI;
        }
        
        string attackDescription = attacker.Attack(defender);
        UpdateHealth(defender, defenderUI);
        
        commentaryText.SetText(attackDescription);

        isMonsterATurn = !isMonsterATurn;
    }

    private Monster RegisterNewMonster(Monster monsterPrefab, Transform monsterSlot, MonsterUI monsterUI)
    {
        // Remove old monsters in slots
        ClearSlot((monsterSlot));

        // Spawn monster from prefab.
        Monster newSpawned = Instantiate(monsterPrefab, monsterSlot);
        
        // Link the UI to the monster.
        UpdateTitle(newSpawned, monsterUI);
        UpdateHealth(newSpawned, monsterUI);
        
        return newSpawned;
    }

    private void ClearSlot(Transform slot)
    {
        for (int i = slot.childCount - 1; i >= 0; i--)
        {
            Transform child = slot.GetChild(i);
            Destroy(child.gameObject);
        }
    }

    private void UpdateTitle(Monster monster, MonsterUI monsterUI)
    {
        monsterUI.UpdateTitle(monster.GetTitle());
    }

    private void UpdateHealth(Monster monster, MonsterUI monsterUI)
    {
        monsterUI.UpdateHealth(monster.GetCurrentHealth(), monster.GetMaxHealth());
    }
}
