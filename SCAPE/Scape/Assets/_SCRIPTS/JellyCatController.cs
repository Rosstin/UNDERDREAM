using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyCatController : Boxable
{
    [Header("Outlets")]
    public Camera ArCamera;

    [Header("Outlets - Items")]
    public CucumberController Cucumber;
    public BroomController Broom;

    public LaunchpadBController Launchpad;

    [Header("Configurables")]
    public float StartSpeed;
    public float IdleRotationSpeed;
    public float GoalRotationSpeed;
    public float AverageDirectionChangePeriod;
    public float VariationInPeriod;
    public float CucumberClosenessThreshholdPixels;
    public float GoalClosenessThreshholdPixels;
    public float BackupAmountMeters;
    public float RotationsOnDeath;
    public float DeathTimePeriod;
    public float RespawnTimePeriod;
    public float CatnipDurationPeriod;

    public float PitDeathClosenessThreshholdPixels;    
    private Vector3 FORWARD = new Vector3(0, 1, 0);
    private Vector3 curDirection;
    private float curSpeed;
    private float leftOrRight = 1f;
    private float curDirectionChangePeriod;
    private float curCucumberTime = 0f;
    // time of cat in current state/action
    private float curTime = 0f;
    // time ratio of current action over current state/action period
    private float curTimeOverPeriodRatio;

    private float initialLength;
    private Vector3 initialLocalScale;
    private Vector3 initialLocalPosition;

    //animation vars
    public Animator JellyCatAnimator;
    private bool isGoalSeeking = true;

    public enum JellyCatState {
        IDLE,
        IDLE_MOVE,
        GOAL_MOVE,
        GOAL_IDLE,
        DYING,
        REBORN,
        CATNIP
    }

    // default not moving
    public JellyCatState currentJellyCatState = JellyCatState.GOAL_IDLE;

    public void doneExploding() {
        currentJellyCatState = JellyCatState.GOAL_MOVE;
    }

    void Start()
    {
        NewDirectionChangePeriod();
        initialLocalScale = this.transform.localScale;
        initialLocalPosition = this.transform.localPosition;
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
        this.transform.localPosition = initialLocalPosition;
    }

    private void SetAnimState(string stateName)
    {
        //JellyCatAnimator.SetInteger("SleepCondition", 0);
        //set each condition to 0, set correct condition to 1
        // SleepCondition, AvoidCondition, AttractCondition

    }

    // calc angle direction between forward and goal
    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);
		
		if (dir > 0f) {
			return 1f;
		} else if (dir < 0f) {
			return -1f;
		} else {
			return 0f;
		}
	}

    void Update()
    {
        curTime += Time.deltaTime;

        Vector2 screenPointOfCat = ArCamera.WorldToScreenPoint(this.transform.position);

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
                    this.transform.Rotate(0.0f, Time.deltaTime * 360f * RotationsOnDeath / DeathTimePeriod , 0f, Space.Self); // spin
                    this.transform.localScale = new Vector3(  1 - curTimeOverPeriodRatio, 1, 1 - curTimeOverPeriodRatio); // shrink
                }
                JellyCatAnimator.SetInteger("SleepCondition", 0);
                break;
            case JellyCatState.REBORN:
                if (curTime > DeathTimePeriod) {
                    if ( isGoalSeeking ) currentJellyCatState = JellyCatState.GOAL_MOVE;
                    else currentJellyCatState = JellyCatState.IDLE_MOVE;
                    curTime = 0f;
                }
                else {
                    curTime += Time.deltaTime;
                    curTimeOverPeriodRatio = curTime / RespawnTimePeriod; 
                    this.transform.Rotate(0, - Time.deltaTime * 360f * RotationsOnDeath / DeathTimePeriod , 0, Space.Self); // opposite spin
                    this.transform.localScale = new Vector3( curTimeOverPeriodRatio, 1, curTimeOverPeriodRatio); // grow
                }
                JellyCatAnimator.SetInteger("SleepCondition", 0);
                break;
            case JellyCatState.GOAL_MOVE:
                // get goal position
                if (Launchpad.gameObject.activeSelf)
                {
                    isGoalSeeking = true;
                    
                    Vector3 heading = Launchpad.transform.position - this.transform.position;
		            leftOrRight = AngleDir(this.transform.forward, heading, this.transform.up);
                    Vector2 screenPointOfLaunchpad = ArCamera.WorldToScreenPoint(Launchpad.transform.position);

                    // TURN ANGLE
                    this.transform.Rotate(0.0f, leftOrRight * GoalRotationSpeed * Time.deltaTime, 0.0f, Space.Self);
                    // FORWARD MOVE
                    this.transform.position += this.transform.forward * StartSpeed * Time.deltaTime;
                    
                    if ( Mathf.Abs(Vector2.Distance(screenPointOfCat, screenPointOfLaunchpad)) < GoalClosenessThreshholdPixels)
                    {
                        currentJellyCatState = JellyCatState.GOAL_IDLE;
                    }
                }
                JellyCatAnimator.SetInteger("SleepCondition", 0);

                goto default;
            case JellyCatState.GOAL_IDLE:
                JellyCatAnimator.SetInteger("SleepCondition", 1);
                break;
            case JellyCatState.IDLE_MOVE:
                isGoalSeeking = false;
                if (curTime > curDirectionChangePeriod)
                {
                    leftOrRight *= -1;
                    curTime = 0f;
                    NewDirectionChangePeriod();
                }
                JellyCatAnimator.SetInteger("SleepCondition", 0);
                // BACK AND FORTH TURN
                this.transform.Rotate(0.0f, leftOrRight * IdleRotationSpeed * Time.deltaTime, 0.0f, Space.Self);

                // FORWARD MOVE
                this.transform.position += this.transform.forward * StartSpeed * Time.deltaTime;
                goto default;
            case JellyCatState.CATNIP:
                isGoalSeeking = false;
                if (curTime > curDirectionChangePeriod)
                {
                    leftOrRight *= -1;
                    curTime = 0f;
                    NewDirectionChangePeriod();
                }
                JellyCatAnimator.SetInteger("SleepCondition", 0);
                // BACK AND FORTH TURN
                this.transform.Rotate(0.0f, leftOrRight * IdleRotationSpeed * Time.deltaTime, 0.0f, Space.Self);

                // FORWARD MOVE
                this.transform.position += this.transform.forward * StartSpeed * Time.deltaTime;
                goto default;
            default:
                // JELLY WIGGLE
                Vector3 vec = new Vector3( ( Mathf.Sin(Time.time) / 2 ) + 1.5f , 1, ( Mathf.Sin(12*Time.time) / 2 ) + 1.5f );
        
                transform.localScale = vec;

                // FALLOFF == DEATH
                // bottom left is 0,0, bottom right is 0,1, top left is 1,0, top right is 1,1
                
                if (screenPointOfCat.x < PitDeathClosenessThreshholdPixels)
                {
                    currentJellyCatState = JellyCatState.DYING;
                }
                else if (screenPointOfCat.x > ArCamera.pixelWidth - PitDeathClosenessThreshholdPixels)
                {
                    currentJellyCatState = JellyCatState.DYING;
                }

                if (screenPointOfCat.y < PitDeathClosenessThreshholdPixels)
                {
                    currentJellyCatState = JellyCatState.DYING;
                }
                else if (screenPointOfCat.y > ArCamera.pixelHeight - PitDeathClosenessThreshholdPixels)
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

                // CATNIP TRIGGER
                if (Catnip.gameObject.activeSelf)
                {
                    Vector2 screenPointOfCucumber = ArCamera.WorldToScreenPoint(Catnip.transform.position);
                    if (Mathf.Abs(Vector2.Distance(screenPointOfCat, screenPointOfCucumber)) < CatnipClosenessThreshholdPixels)
                    {
                        currentJellyCatState = JellyCatState.CATNIP;
                    }
                }
            break;
        }
    }
}
