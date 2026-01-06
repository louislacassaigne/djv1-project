using UnityEngine;

public class XPBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;

    [SerializeField] private PlayerCharacter _playerCharacter;


    private void Update()
    {
        fillTransform.anchorMax = new Vector2(_playerCharacter.currentXP/(100f *_playerCharacter.level), 1f);
    }
}
