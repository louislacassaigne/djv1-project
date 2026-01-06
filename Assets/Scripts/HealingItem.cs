using UnityEngine;

public class HealingItem : MonoBehaviour
{

    private Animator animator;
    [SerializeField] private PlayerCharacter player;

    // Start is called before the first frame update

    protected void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("pickup");
            player.hitPoints = Mathf.Min(player.hitPoints + 20, player.maxHitPoints);
        }
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}
