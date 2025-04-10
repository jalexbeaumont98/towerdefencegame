using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;

public class EnemyBase : MonoBehaviour
{

    [Header("References")]
    [SerializeField] protected Transform target;
    [SerializeField] protected Vector3 tileTarget;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected GameState gameState;
    [SerializeField] protected Tilemap towerMap;
    [SerializeField] protected Sprite frontSprite;
    [SerializeField] protected Sprite backSprite;
    [SerializeField] protected Sprite leftSprite;
    [SerializeField] protected Sprite rightSprite;
    [SerializeField] protected List<EnemyStatus> statuses;
    [SerializeField] protected List<EnemyStatus> startStatuses;
    [SerializeField] protected GameObject statusBar;
    [SerializeField] protected EnemyStatusBar statusBarScript;
    [SerializeField] protected EnemyHealthBar healthBar;



    [Header("Attributes")]
    [SerializeField] public string enemyName;
    [SerializeField] protected int health;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected int armor;
    [SerializeField] protected int shield;
    [SerializeField] protected int reward;
    [SerializeField] protected int damage;
    [SerializeField] protected bool stealth;
    [SerializeField] protected bool flying;
    [SerializeField] protected LayerMask layerMask;
    [SerializeField] protected string description;
    [SerializeField] protected Vector3 uiOffsetVector = Vector3.zero;




    protected NavMeshPath path;
    protected NavMeshPath oldPath;
    protected float elapsed = 0.0f;

    bool selected;
    float minAnimTime;


    public Dictionary<string, int> statusAmounts = new Dictionary<string, int>();


    protected GameObject selectedHighlightPrefab;

    public delegate void OnEnemySelectedChange(int newHealth);

    public event OnEnemySelectedChange onEnemySelectedChange;

    #region Get Set
    public string EnemyName
    {
        get { return enemyName; }
        set { enemyName = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int Armor
    {
        get { return armor; }
        set { armor = value; }
    }

    public int Shield
    {
        get { return shield; }
        set { shield = value; }
    }

    public int Reward
    {
        get { return reward; }
        set { reward = value; }
    }

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public bool Stealth
    {
        get { return stealth; }
        set { stealth = value; }
    }

    public bool Flying
    {
        get { return flying; }
        set { flying = value; }
    }

    public LayerMask LayerMask
    {
        get { return layerMask; }
        set { layerMask = value; }
    }

    public string Description
    {
        get { return description; }
        set { description = value; }
    }

    public Vector3 UiOffsetVector
    {
        get { return uiOffsetVector; }
        set { uiOffsetVector = value; }
    }

    #endregion

    #region Setup
    protected virtual void Start()
    {

        towerMap = FindObjectOfType<TileClickHandler>().GetTowerMap();

        gameState = GameState.Instance;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = speed;

        target = GameObject.FindWithTag("Base").transform;

        path = new NavMeshPath();
        oldPath = path;

        maxHealth = health;

        CreateStatusBar();

        foreach (EnemyStatus status in startStatuses) AddStatus(status);

        print("ive been spawned!");

    }

    public void SetVariations(Dictionary<string, object> variations, List<string> statuses)
    {
        // Modify speed if specified
        if (variations.ContainsKey("speed") && variations["speed"] is double speedValue)
        {
            float vspeed = (float)speedValue; // Convert from double to float

            speed = vspeed;
        }

        // Modify health if specified
        if (variations.ContainsKey("health"))
        {
            int healthValue = Convert.ToInt32(variations["health"]);
            print("is in the block");
            health = healthValue;
        }

        // Modify color if specified
        if (variations.ContainsKey("color") && variations["color"] is string colorString)
        {
            if (UnityEngine.ColorUtility.TryParseHtmlString(colorString, out Color newColor))
            {

                spriteRenderer.color = newColor;

            }
            else
            {
                Debug.LogError($"Invalid color string: {colorString}");
            }
        }

        if (variations.ContainsKey("stealth") && variations["stealth"] is bool isStealth)
        {
            stealth = isStealth;
        }

        if (variations.ContainsKey("flying") && variations["flying"] is bool isFlying)
        {
            flying = isFlying;
        }

        if (variations.ContainsKey("description") && variations["description"] is string description)
        {
            this.description = description;
        }

        foreach (string status in statuses)
        {
            var newstatus = GameState.Instance.GetStatus(status);

            if (status != null)
            {
                startStatuses.Add(newstatus);
            }
            else
            {
                Debug.LogWarning("Status is null and was not added to startStatuses.");
            }
        }
    }

    #endregion


    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (selected) onEnemySelectedChange?.Invoke(health);

        healthBar?.SetHealthBar(health);

        if (health <= 0)
        {
            DeselectEnemy();
            Die();
        }
    }

    public virtual Sprite GetSprite()
    {
        return frontSprite;
    }

    public virtual NavMeshAgent GetAgent()
    {
        return agent;
    }

    public GameObject SelectEnemy(GameObject prefab)
    {
        selectedHighlightPrefab = Instantiate(prefab, this.gameObject.transform);
        selected = true;

        return selectedHighlightPrefab;

    }

    public void DeselectEnemy()
    {
        selected = false;
    }



    public virtual Dictionary<string, string> GetInfo()
    {

        Dictionary<string, string> info = new Dictionary<string, string>
        {
            {"name", enemyName},
            {"maxHealth", maxHealth.ToString()},
            {"health", health.ToString()},
            {"speed", speed.ToString()},
            {"reward", reward.ToString()},
            {"description", description}
        };

        string isBoolValue = "";

        if (stealth)
        {
            isBoolValue = name + " is stealth";
            info.Add("stealth", isBoolValue);
        }

        if (flying)
        {
            isBoolValue = name + " is a flying enemy.";
            info.Add("flying", isBoolValue);
        }

        return info;
    }

    public virtual void Die(bool goal = false)
    {
        gameState.RemoveEnemyFromList(this);
        Destroy(statusBar);

        if (!goal) gameState.SetMoney(reward);

        Destroy(gameObject);
    }



    protected virtual void Move()
    {

        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            Debug.LogError("NavMeshAgent is not on a NavMesh or is not active.");
            agent = GetComponent<NavMeshAgent>();
        }


    }

    protected virtual void AnimateMovement()
    {

        elapsed += Time.deltaTime;
        if (elapsed > minAnimTime)
        {
            elapsed -= minAnimTime;

        }

        else return;


        Vector2 velocity = agent.velocity;

        // Determine the primary direction based on velocity
        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            // Moving horizontally
            if (velocity.x > 0)
            {
                // Moving right
                spriteRenderer.sprite = rightSprite;
            }
            else
            {
                // Moving left
                spriteRenderer.sprite = leftSprite;
            }
        }
        else
        {
            // Moving vertically
            if (velocity.y > 0)
            {
                // Moving up
                spriteRenderer.sprite = backSprite;
            }
            else
            {
                // Moving down
                spriteRenderer.sprite = frontSprite;
            }
        }
    }


    protected virtual void DebugPath()
    {
        oldPath = path;

        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        }

    }

    protected virtual void Update()
    {
        Move();

        AnimateMovement();

        //DebugPath(); 

        // Calculate the angle in degrees
        //float angle = Mathf.Atan2(agent.velocity.y, agent.velocity.x) * Mathf.Rad2Deg + 90f;

        // Set the rotation of the sprite




    }

    public virtual bool CheckForObstructions()
    {
        return CalculateNewPath();
    }


    protected virtual bool FindPathObstruction()
    {
        Vector2 start;
        Vector2 end;
        Vector2 direction;
        float distance;

        for (int i = 0; i < oldPath.corners.Length - 1; i++)
        {
            start = oldPath.corners[i];
            end = oldPath.corners[i + 1];
            direction = end - start;

            distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast(start, direction, distance, layerMask);

            if (hit.collider != null)
            {
                Debug.Log("Hit a collider: " + hit.collider.gameObject.name);

                Vector3 hitWorldPosition = hit.point;
                Vector3Int tilePosition = towerMap.WorldToCell(hitWorldPosition);
                Vector3 tileWorldPosition = towerMap.CellToWorld(tilePosition);

                tileTarget = tileWorldPosition;
                return true;
            }

            else
            {
                Debug.Log("No collider detected between the points.");

            }

        }


        return false;




    }

    protected virtual bool CalculateNewPath()
    {


        agent.CalculatePath(target.position, path);

        print("New path calculated");
        if (path.status != NavMeshPathStatus.PathComplete)
        {
            return false;
        }
        else
        {

            for (int i = 0; i < path.corners.Length - 1; i++)
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);


            return true;
        }
    }


    public virtual void AddStatus(EnemyStatus status)
    {

        if (statusAmounts.ContainsKey(status.type))
        {
            if (!status.stackable) return;

            statusAmounts[status.type] = statusAmounts[status.type]++;
            statusBarScript?.StackAttribute(status, statusAmounts[status.type]);

        }

        else
        {
            statusAmounts.Add(status.type, 1);
            statusBarScript?.AddStatusIcon(status);
        }

        statuses.Add(status.SetStatus(this));
        if (status.isTimed) StartCoroutine(RemoveStatusCo(status.statusTime, status));
    }

    public virtual void RemoveStatus(EnemyStatus status)
    {
        

        statusAmounts[status.type]--;

        if (statusAmounts[status.type] > 0)
        {

            statusBarScript?.RemoveStatusIcon(status, statusAmounts[status.type]);
        }

        else
        {
            statusAmounts.Remove(status.type);
            statusBarScript?.RemoveStatusIcon(status);
        }

        status.RemoveStatus(this);
        statuses.Remove(status);


    }

    private IEnumerator RemoveStatusCo(float delay, EnemyStatus status)
    {
        yield return new WaitForSeconds(delay);

        RemoveStatus(status);

    }

    public virtual void SetStealth(bool input = true)
    {

        stealth = input;

        Color color = spriteRenderer.color;

        if (input)
        {
            // Set the alpha (transparency) value
            color.a = Mathf.Clamp01(0.8f); // Ensure transparency is between 0 and 1
        }

        else
        {
            color.a = 1;
        }

        // Apply the new color
        spriteRenderer.color = color;
    }

    public virtual void SetFlying(bool input = true)
    {
        flying = input;
    }

    public void ModifySpeed(float modifier)
    {
        speed = speed / modifier;
        agent.speed = speed;

    }

    private void CreateStatusBar()
    {

        GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");

        statusBar = Instantiate(GameState.Instance.enemyStatusBarPrefab, canvas.transform);
        statusBar.SetActive(true);

        statusBar.GetComponent<UIAnchor>().SetAnchor(this.transform, uiOffsetVector);

        healthBar = statusBar.GetComponent<EnemyHealthBar>();
        healthBar.SetHealthBar(health, maxHealth);

        statusBarScript = statusBar.GetComponent<EnemyStatusBar>();

    }
}
