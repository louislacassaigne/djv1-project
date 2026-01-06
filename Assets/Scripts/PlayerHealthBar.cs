using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;

    [SerializeField] private PlayerCharacter _playerCharacter;


    private void Update()
    {
        fillTransform.anchorMin = new Vector2(1f - _playerCharacter.hitPoints/_playerCharacter.maxHitPoints, 0f);
    }
}
