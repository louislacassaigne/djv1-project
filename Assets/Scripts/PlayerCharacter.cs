using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour, IDamageable
{
    [SerializeField] public int maxHitPoints = 100;
    public float hitPoints;
    [SerializeField] public float speed = 4f;
    [SerializeField] private float angularSpeed = 360f;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] public float shootDelay = 0.8f;

    [SerializeField] public int damage = 10;

    [SerializeField] public float bulletSpeed = 5f;

    [SerializeField] public float rollSpeed = 8f;


    [SerializeField] private Animator animator;
    [SerializeField] private Animator pivotAnimator;

    [SerializeField] private GameObject PowerSelectionScreen;

    [SerializeField] private GetUpgrade getUpgrade;

    [SerializeField] public bool isRollUnlocked = false;
    public bool isSprintUnlocked = false;

    int defaultLayer;
    int invincibleLayer;

    public int level = 1;
    public int currentXP = 0;

    private float _shootTimer;

    public Vector3 RollDirection;

    private bool _isRolling = false;
    public bool _isJumping = false;

    public bool isInMenu = false;
    private Camera _mainCamera;
    private CharacterController _characterController;

    public Transform cameraTransform;

    public Collider playerCollider;


    protected void Awake()
    {
        _mainCamera = Camera.main;
        hitPoints = maxHitPoints;
        _characterController = GetComponent<CharacterController>();
        defaultLayer = gameObject.layer;
        invincibleLayer = LayerMask.NameToLayer("InvinciblePlayer");
    }

    protected void Update()
    {

        if (!(this.transform.position.y == 0f))
        {
            this.transform.position = new Vector3(this.transform.position.x, 0f, this.transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping && !_isRolling)
        {
            animator.SetTrigger("jump");
        }
        

        // Direction de la caméra (sans inclinaison verticale)
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;

        if (cameraForward.sqrMagnitude < 0.001f)
            return;

        cameraForward.Normalize();

        // Rotation cible
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

        // Rotation progressive du joueur
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            angularSpeed * Time.deltaTime
        );

    

        /* =========================
         * 1️⃣ INPUT ZQSD
         * ========================= */
        float inputX = Input.GetAxisRaw("Horizontal"); // Q / D
        float inputZ = Input.GetAxisRaw("Vertical");   // Z / S

        Vector3 inputDirection = new Vector3(inputX, 0f, inputZ);

        if (!_isRolling && isRollUnlocked && !_isJumping)
        {
            // Gauche
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Q))
            {
                animator.SetTrigger("roll");
                pivotAnimator.SetTrigger("leftroll");
                RollDirection = Vector3.left.normalized;
                _isRolling = true;
                
            }
            // Droite
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.D))
            {
                animator.SetTrigger("roll");
                pivotAnimator.SetTrigger("rightroll");
                RollDirection = Vector3.right.normalized;
                _isRolling = true;
                
            }
            // Avant
            else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.Z))
            {
                animator.SetTrigger("forwardroll");
                pivotAnimator.SetTrigger("leftroll");
                RollDirection = Vector3.forward.normalized;
                _isRolling = true;
            }
        }





        /* =========================
         * 2️⃣ Direction caméra (plan horizontal)
         * ========================= */
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight   = cameraTransform.right;

        camForward.y = 0f;
        camRight.y   = 0f;

        camForward.Normalize();
        camRight.Normalize();

        /* =========================
         * 3️⃣ Direction de déplacement caméra-relative
         * ========================= */
        Vector3 moveDirection =
            camForward * inputZ +
            camRight   * inputX;

        /* =========================
         * 4️⃣ Déplacement
         * ========================= */
        if (moveDirection.sqrMagnitude > 0.001f && !_isRolling)
        {
            if (isSprintUnlocked && Input.GetKey(KeyCode.LeftShift))
            {
                // Sprint
                _characterController.Move(
                    moveDirection.normalized * speed * 4f * Time.deltaTime
                );
            }
            else
            {
                // Marche normale
                _characterController.Move(
                    moveDirection.normalized * speed * Time.deltaTime
                );
            }
        }


        if (_isRolling)
        {
            // Déplacement continu pendant la roulade
            Vector3 moveDir = transform.TransformDirection(RollDirection);
            _characterController.Move(moveDir * rollSpeed * Time.deltaTime);
        }



        _shootTimer -= Time.deltaTime;
        if (Input.GetMouseButton(0) && _shootTimer <= 0f && !Input.GetKey(KeyCode.LeftShift) && !_isJumping && !_isRolling)
        {
            var direction = transform.rotation * Vector3.forward;
            var bullet = Instantiate(bulletPrefab, transform.position + direction * 0.5f, transform.rotation);
            bullet.gameObject.SetActive(true);
            bullet.damage = damage;
            bullet.speed = bulletSpeed;
            _shootTimer = shootDelay;
        }
    }

    public void ApplyDamage(int value)
    {
        hitPoints -= value;
        if (hitPoints <= 0)
        {
            // Game Over
            Destroy(gameObject);
        }
    }



    public void StartRolling()
    {
        gameObject.layer = invincibleLayer;
    }

    public void StopRolling()
    {
        _isRolling = false;
        gameObject.layer = defaultLayer;
    }

    public void levelUp()
    {
        level++;
        getUpgrade.GenerateRewards();
        print("reward 1: " + getUpgrade.currentRewards[0]);
        print("reward 2: " + getUpgrade.currentRewards[1]);
        Time.timeScale = 0f;
        PowerSelectionScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isInMenu = true;

    }



}

