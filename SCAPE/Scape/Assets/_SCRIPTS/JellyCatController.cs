using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCatController : MonoBehaviour
{
    [Header("Outlets")]
    public Camera ArCamera;

    [Header("Outlets - Items")]
    public CucumberController Cucumber;

    [Header("Configurables")]
    public float StartSpeed;
    public float RotationSpeed;
    public float AverageDirectionChangePeriod;
    public float VariationInPeriod;
    public float CucumberClosenessThreshholdPixels;
    public float BackupAmountMeters;
    public float RotationsOnDeath;
    public float DeathTimePeriod;
    public float RespawnTimePeriod;

    
    private Vector3 FORWARD = new Vector3(0, 1, 0);

    private Vector3 curDirection;
    private float curSpeed;
    private int leftOrRight = 1;
    private float curDirectionChangePeriod;
    private float curCucumberTime = 0f;
    // time of cat in current state/action
    private float curTime = 0f;
    // time ratio of current action over current state/action period
    private float curTimeOverPeriodRatio;

    public enum JellyCatState {
        IDLE,
        IDLE_MOVE,
        GOAL_MOVE,
        GOAL_IDLE,
        DYING,
        REBORN
    }

    public JellyCatState currentJellyCatState = JellyCatState.IDLE_MOVE;

    void Start()
    {
        NewDirectionChangePeriod();
    }

    private void NewDirectionChangePeriod()
    {
        curDirectionChangePeriod = AverageDirectionChangePeriod + Random.Range(-VariationInPeriod, +VariationInPeriod);
    }

    private void BackupAndFlip()
    {
        BackupCat();
        FlipCat();
    }

    // flip around and switch direction
    private void FlipCat()
    {
        this.transform.Rotate(0.0f, 180f, 0.0f, Space.Self);
    }

    // move back a bit so you're not colliding anymore
    private void BackupCat()
    {
        this.transform.position += -this.transform.forward * BackupAmountMeters;
    }

    private void MoveCatToOrigin()
    {
        this.transform.SetPositionAndRotation();
    }

    void Update()
    {
        curTime += Time.deltaTime;

        switch ( currentJellyCatState ) {

            case JellyCatState.DYING:
                if (curTime > DeathTimePeriod) {
                    MoveCatToOrigin();
                    currentJellyCatState = JellyCatState.REBORN;
                    curTime = 0f;
                }
                else {
                    curTime += Time.deltaTime;
                    curTimeOverPeriodRatio = curTime / DeathTimePeriod; 
                    this.transform.Rotate(0, RotationsOnDeath * curTimeOverPeriodRatio, 0); // spin
                    this.transform.localScale = new Vector3(  1 - curTimeOverPeriodRatio, 1, 1 - curTimeOverPeriodRatio); // shrink
                }
                break;
            case JellyCatState.REBORN:
                if (curTime > DeathTimePeriod) {
                    currentJellyCatState = JellyCatState.IDLE_MOVE;
                    curTime = 0f;
                }
                else {
                    curTime += Time.deltaTime;
                    curTimeOverPeriodRatio = curTime / RespawnTimePeriod; 
                    this.transform.localScale = new Vector3( curTimeOverPeriodRatio, 1, curTimeOverPeriodRatio); // grow
                }
                break;
            case JellyCatState.IDLE_MOVE:
                if (curTime > curDirectionChangePeriod)
                {
                    leftOrRight *= -1;
                    curTime = 0f;
                    NewDirectionChangePeriod();
                }
                goto default;
            case JellyCatState.GOAL_MOVE:
                if (curTime > curDirectionChangePeriod)
                {
                    leftOrRight *= -1;
                    curTime = 0f;
                    NewDirectionChangePeriod();
                }
                goto default;
            case JellyCatState.GOAL_IDLE:
                // idle animation here?
                break;
            default:
                
                // BACK AND FORTH TURN
                this.transform.Rotate(0.0f, leftOrRight * RotationSpeed * Time.deltaTime, 0.0f, Space.Self);

                // JELLY WIGGLE
                Vector3 vec = new Vector3( ( Mathf.Sin(Time.time) / 2 ) + 1.5f , 1, ( Mathf.Sin(6*Time.time) / 2 ) + 1.5f );
        
                transform.localScale = vec;

                // FORWARD MOVE
                this.transform.position += this.transform.forward * StartSpeed * Time.deltaTime;

                // FALLOFF == DEATH
                // bottom left is 0,0, bottom right is 0,1, top left is 1,0, top right is 1,1
                Vector2 screenPointOfCat = ArCamera.WorldToScreenPoint(this.transform.position);
                if (screenPointOfCat.x < 0f)
                {
                    currentJellyCatState = JellyCatState.DYING;
                }
                else if (screenPointOfCat.x > ArCamera.pixelWidth)
                {
                    currentJellyCatState = JellyCatState.DYING;
                }

                if (screenPointOfCat.y < 0f)
                {
                    currentJellyCatState = JellyCatState.DYING;
                }
                else if (screenPointOfCat.y > ArCamera.pixelHeight)
                {
                    currentJellyCatState = JellyCatState.DYING;
                }

                // AVOID CUCUMBER
                if (Cucumber.gameObject.activeSelf)
                {
                    Vector2 screenPointOfCucumber = ArCamera.WorldToScreenPoint(Cucumber.transform.position);
                    if (Mathf.Abs(Vector2.Distance(screenPointOfCat, screenPointOfCucumber)) < CucumberClosenessThreshholdPixels)
                    {
                        BackupAndFlip();
                    }
                }
            break;
        }

    }
}
