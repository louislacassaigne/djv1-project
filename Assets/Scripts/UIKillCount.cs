using UnityEngine;

public class UIKillCount : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI killCountText;
    [SerializeField] private EnemySpawner enemySpawner;

    private void Update()
    {
        if (enemySpawner.EnemyKilledCount > 1)
        {
            killCountText.text = $"{enemySpawner.EnemyKilledCount} ennemis tués";
        }
        else
        {
            killCountText.text = $"{enemySpawner.EnemyKilledCount} ennemi tué";
        }
    }
}
