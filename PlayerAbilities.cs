using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    /*
     * GabaMode --> The Player gets super hyper and kills everything on this path
     * Collision with enemy --> Kill them
     * Collision with obstacle --> kill them
     * GabaMode is enabled after certain kills
     * 
     */

    public float killsForGaba = 1;//Kills needed to acivate gaba mode
    public float GabaTime = 2f; //Timer for deactivating gaba mode

    private Player player;
    private bool isCharged = false;
    private bool isGabaModeOn = false;
    [HideInInspector] public int currentKills; //Keep track of kills  
    private bool canGaba = false;//Can the player enter Gaba Mode

    //Debugging
    private Color originalColor;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        player = GetComponent<Player>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }
    private void Update()
    {
        if(player.chargeStatus == Player.ChargeStatus.Normal && isCharged)
        {
            isCharged = false;
        }
        if(player.chargeStatus == Player.ChargeStatus.Charged && !isCharged)
        {
            isCharged = true;
        }
        if(Input.GetMouseButtonDown(1) && !isGabaModeOn && canGaba) //Make this using onscreen button
        {
            //Using Ability Here --> Activate Gaba Mode
            player.collisionWithImmovable = GabaModeCollision;
            player.collisionWithEnemy = GabaModeCollision;

            isGabaModeOn = true;
            StartCoroutine(DisableGabaMode());

            //ChangeColor
            spriteRenderer.color = Color.red;

            canGaba = false;
        }
    }
    private void GabaModeCollision(Enemy enemy)
    {
        enemy.DestroyWithStyle();

    }
    private IEnumerator DisableGabaMode()
    {
        yield return new WaitForSeconds(GabaTime);

        //Reset Delegates
        player.collisionWithImmovable = player.NormalImmovableCollision;
        player.collisionWithEnemy = player.NormalEnemyCollision;

        isGabaModeOn =false;
        spriteRenderer.color = originalColor;


    }
    public void IncreaseKill(int increaseBy)
    {
        currentKills += increaseBy;
        if(currentKills >= killsForGaba)
        {
            canGaba = true;
        }
    }
}
