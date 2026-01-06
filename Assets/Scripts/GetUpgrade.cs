using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GetUpgrade : MonoBehaviour
{
    [SerializeField] private PlayerCharacter player;

    [SerializeField] private TextMeshProUGUI Power1Description;
    [SerializeField] private TextMeshProUGUI Power2Description;

    public UpgradeType[] currentRewards = new UpgradeType[2];

    private UpgradeType? firstSpecial;   // choisie au niveau 5
    private UpgradeType? secondSpecial;  // l’autre, niveau 15


    // ------------------------------
    // POOLS DE BASE
    // ------------------------------

    private List<UpgradeType> baseUpgrades = new List<UpgradeType>
    {
        UpgradeType.BulletSpeed,
        UpgradeType.MoveSpeed,
        UpgradeType.BulletDamage,
        UpgradeType.FireRate,
        UpgradeType.MaxHealth
    };

    // ------------------------------

    public void GenerateRewards()
    {
        List<UpgradeType> pool = new List<UpgradeType>(baseUpgrades);
        int level = player.level;

        UpgradeType? forced = null;

        // -------- NIVEAU 5 --------
        if (level >= 5 && firstSpecial == null)
        {
            // Choix aléatoire UNE FOIS
            firstSpecial = Random.value < 0.5f ? UpgradeType.Roll : UpgradeType.Sprint;
            secondSpecial = firstSpecial == UpgradeType.Roll ? UpgradeType.Sprint : UpgradeType.Roll;
        }

        // -------- FORCER AU NIVEAU 5 --------
        if (level == 5 && firstSpecial.HasValue && !IsSpecialUnlocked(firstSpecial.Value))
        {
            forced = firstSpecial.Value;
        }

        // -------- FORCER AU NIVEAU 15 --------
        if (level == 15 && secondSpecial.HasValue && !IsSpecialUnlocked(secondSpecial.Value))
        {
            forced = secondSpecial.Value;
        }

        // -------- AJOUT AU POOL --------

        if (level >= 5 && firstSpecial.HasValue && !IsSpecialUnlocked(firstSpecial.Value))
            pool.Add(firstSpecial.Value);

        if (level >= 15 && secondSpecial.HasValue && !IsSpecialUnlocked(secondSpecial.Value))
            pool.Add(secondSpecial.Value);

        // -------- TIRAGE FINAL --------

        if (forced.HasValue)
        {
            currentRewards[0] = forced.Value;
            pool.Remove(forced.Value);
            currentRewards[1] = PickRandomFrom(pool);
        }
        else
        {
            currentRewards[0] = PickRandomFrom(pool);
            pool.Remove(currentRewards[0]);
            currentRewards[1] = PickRandomFrom(pool);
        }

        Power1Description.text = currentRewards[0].ToString();
        Power2Description.text = currentRewards[1].ToString();
    }


    // ------------------------------
    // OUTILS
    // ------------------------------

    UpgradeType PickRandomFrom(List<UpgradeType> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    UpgradeType? GetRandomLockedSpecial()
    {
        List<UpgradeType> locked = new List<UpgradeType>();

        if (!player.isRollUnlocked)
            locked.Add(UpgradeType.Roll);

        if (!player.isSprintUnlocked)
            locked.Add(UpgradeType.Sprint);

        if (locked.Count == 0)
            return null;

        return locked[Random.Range(0, locked.Count)];
    }

    UpgradeType? GetRemainingSpecial()
    {
        if (!player.isRollUnlocked)
            return UpgradeType.Roll;

        if (!player.isSprintUnlocked)
            return UpgradeType.Sprint;

        return null;
    }

    // ------------------------------
    // UI / INPUT
    // ------------------------------

    public void OnFirstUpgradeSelected()
    {
        ApplyUpgrade(currentRewards[0]);
    }

    public void OnSecondUpgradeSelected()
    {
        ApplyUpgrade(currentRewards[1]);
    }

    // ------------------------------
    // APPLICATION DES UPGRADES
    // ------------------------------

    void ApplyUpgrade(UpgradeType upgrade)
    {
        Debug.Log("Upgrade applied: " + upgrade);

        switch (upgrade)
        {
            case UpgradeType.BulletSpeed:
                player.bulletSpeed *= 1.8f;
                break;

            case UpgradeType.MoveSpeed:
                player.speed *= 1.6f;
                player.rollSpeed *= 1.6f;
                break;

            case UpgradeType.BulletDamage:
                player.damage += 10;
                break;

            case UpgradeType.FireRate:
                player.shootDelay /= 1.4f;
                break;

            case UpgradeType.MaxHealth:
                player.maxHitPoints += 40;
                player.hitPoints += 40;
                break;

            case UpgradeType.Roll:
                player.isRollUnlocked = true;
                break;

            case UpgradeType.Sprint:
                player.isSprintUnlocked = true;
                break;
        }

        ResumeGame();
    }

    bool IsSpecialUnlocked(UpgradeType type)
    {
        return (type == UpgradeType.Roll && player.isRollUnlocked)
            || (type == UpgradeType.Sprint && player.isSprintUnlocked);
    }


    // ------------------------------

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.isInMenu = false;
    }
}
