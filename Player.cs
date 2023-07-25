using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float forceMagnitude = 5f;
    [SerializeField] private float airForceMultiplier = 5f;
    [SerializeField] private int numberOfAirSwitch = 2;

    public enum ChargeStatus
    {
        Normal,
        Charged
    }
    public ChargeStatus chargeStatus = ChargeStatus.Normal;

    private Rigidbody2D rb;//Reference to rigidbody2d
    private int _airSwtichLeft = 1;//current SwitchesLeft
    private PlayerAbilities _abilities;


    //Delegates and Stuff
    public delegate void CollisionWithImmovable(Enemy obst = null);
    public CollisionWithImmovable collisionWithImmovable;

    public delegate void CollisionWithEnemy(Enemy enemy = null);
    public CollisionWithEnemy collisionWithEnemy;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        collisionWithEnemy = NormalEnemyCollision;
        collisionWithImmovable = NormalImmovableCollision;

        _abilities = GetComponent<PlayerAbilities>();
    }

    // Update is called once per frame
    void Update()
    {
        //If Click is detected Pause Player than change lane
        if(Input.GetMouseButtonDown(0))
        {
            SwitchMechanic();
        }

    }
    private void SwitchMechanic()
    {
        if (_airSwtichLeft <= 0) return;

        float _forceDir = 0; //For deciding dirn of force when changing lane
        bool _inContactWithWall = false;

        //Get Collision data with wall and get normal
        Collider2D[] collidingBodies = Physics2D.OverlapCircleAll(transform.position, 2f);

        foreach (Collider2D item in collidingBodies)
        {
            switch (item.name)
            {
                case "L_SideWall":
                    _forceDir = 1; _inContactWithWall = true; break;
                case "R_SideWall":
                    _forceDir = -1; _inContactWithWall = true; break;
                default:
                    break;
            }
        }

        if (!_inContactWithWall)//this is for all the stuff in air
        {
            //Clicked in Air
            _forceDir = (rb.velocity.x < 0) ? -1 : 1;
            _forceDir *= airForceMultiplier;

            chargeStatus = ChargeStatus.Charged;
        }

        rb.velocity = new Vector2();
        //Add force in that direction
        rb.AddForce(_forceDir * forceMagnitude * transform.right, ForceMode2D.Impulse);
        _airSwtichLeft -= 1;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("SideWall"))
        {
            _airSwtichLeft = numberOfAirSwitch + 1;
            chargeStatus = ChargeStatus.Normal;

        }
    }

    public void NormalImmovableCollision(Enemy obst = null)
    {
        //Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Destroy(gameObject);

    }
    public void NormalEnemyCollision(Enemy enemy = null)
    {
        if(chargeStatus == ChargeStatus.Charged)
        {
            //Kill the Enemy
            Destroy(enemy.gameObject);
            _abilities.IncreaseKill(1);
        }
        else
        {
            //Decrease PlayerHealth
            //Debug.Log("GameOver");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    /*
     * Player Health System
     * Specify Maximum Health
     * Add Health --> Healing effect
     * Decrease Health --> decrease 1 Health
     */
}
public class PlayerState //PLayer State Parent Object
{ 
    virtual public PlayerState handleState()
    {
        return this;
    }
}
public class NormalState : PlayerState
{
    public override PlayerState handleState()
    {
        return handleState();
    }
}
public class ChargedState : PlayerState
{
    public override PlayerState handleState()
    {
        return handleState();
    }
}
