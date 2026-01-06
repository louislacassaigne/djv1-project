using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GetUpgrade : MonoBehaviour
{
    [SerializeField] private PlayerCharacter player;

    [SerializeField] private TextMeshProUGUI Power1Description;
    [SerializeField] private TextMeshProUGUI Power2Description;

    public UpgradeType[] currentRewards = new UpgradeType[2];

    private UpgradeType? firstSpecial;   // chosen at level 5
    private UpgradeType? secondSpecial;  // the other one at level 15


    private List<UpgradeType> baseUpgrades = new List<UpgradeType>
    {
        UpgradeType.BulletSpeed,
        UpgradeType.MoveSpeed,
        UpgradeType.BulletDamage,
        UpgradeType.FireRate,
        UpgradeType.MaxHealth
    };

    private Dictionary<UpgradeType, string> upgradeDescriptions = new Dictionary<UpgradeType, string>
    {
        { UpgradeType.BulletSpeed,  "Increases bullet speed by 80%" },
        { UpgradeType.MoveSpeed,    "Increases all movement speed by 60%" },
        { UpgradeType.BulletDamage, "Bullets deal +10 damage" },
        { UpgradeType.FireRate,     "Shoot 30% faster" },
        { UpgradeType.MaxHealth,    "Increase max health by 40" },

        { UpgradeType.Roll,         "You can roll to dodge attacks with invicibility frames. Press ctrl + direction" },
        { UpgradeType.Sprint,       "Hold Shift to move a lot faster. You can't shoot while sprinting" }
    };



    public void GenerateRewards()
    {
        List<UpgradeType> pool = new List<UpgradeType>(baseUpgrades);
        int level = player.level;

        UpgradeType? forced = null;


        if (level >= 5 && firstSpecial == null)
        {
            firstSpecial = Random.value < 0.5f ? UpgradeType.Roll : UpgradeType.Sprint;
            secondSpecial = firstSpecial == UpgradeType.Roll ? UpgradeType.Sprint : UpgradeType.Roll;
        }

        if (level == 5 && firstSpecial.HasValue && !IsSpecialUnlocked(firstSpecial.Value))
        {
            forced = firstSpecial.Value;
        }

        if (level == 15 && secondSpecial.HasValue && !IsSpecialUnlocked(secondSpecial.Value))
        {
            forced = secondSpecial.Value;
        }

        if (level >= 5 && firstSpecial.HasValue && !IsSpecialUnlocked(firstSpecial.Value))
            pool.Add(firstSpecial.Value);

        if (level >= 15 && secondSpecial.HasValue && !IsSpecialUnlocked(secondSpecial.Value))
            pool.Add(secondSpecial.Value);


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

        Power1Description.text = upgradeDescriptions[currentRewards[0]];
        Power2Description.text = upgradeDescriptions[currentRewards[1]];
    }



    UpgradeType PickRandomFrom(List<UpgradeType> list)
    {
        return list[Random.Range(0, list.Count)];
    }




    public void OnFirstUpgradeSelected()
    {
        ApplyUpgrade(currentRewards[0]);
    }

    public void OnSecondUpgradeSelected()
    {
        ApplyUpgrade(currentRewards[1]);
    }


    // APPLying UPGRADES


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
                player.shootDelay /= 1.3f;
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

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.isInMenu = false;
    }
}
