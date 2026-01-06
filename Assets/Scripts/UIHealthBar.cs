using UnityEngine;

public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform fillTransform;

    private EnemyCharacter _enemyCharacter;
    private Camera _camera;

    private void Awake()
    {
        _enemyCharacter = GetComponentInParent<EnemyCharacter>();
        _camera = Camera.main;
    }

    private void Update()
    {
        fillTransform.anchorMin = new Vector2(1f - _enemyCharacter.HitPointPercent, 0f);
        transform.LookAt(_camera.transform.position);
    }
}
