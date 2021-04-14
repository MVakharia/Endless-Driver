using UnityEngine;

public class PlayerSteering : MonoBehaviour
{
    #region Fields
    private static PlayerSteering singleton;
    [SerializeField]
    GameObject car;
    [SerializeField]
    float screenBoundLeft;
    [SerializeField]
    float screenBoundRight;
    [SerializeField]
    float currentTurnSpeed;
    [SerializeField]
    float maxLeftTurnSpeed;
    [SerializeField]
    float maxRightTurnSpeed;
    [SerializeField]
    float turnAcceleration;
    [SerializeField]
    float deadZone;
    [SerializeField]
    float rotationAngleMultiplier;
    #endregion

    #region Properties
    GameObject[] LanePoints => RoadManager.Singleton.LanePoints;
    bool IsTouchingScreen => Input.touchCount > 0;
    bool IsTouchingLeftSideOfScreen => IsTouchingScreen && (Input.GetTouch(0).position.x < screenBoundLeft);
    bool IsTouchingRightSideOfScreen => IsTouchingScreen && (Input.GetTouch(0).position.x > screenBoundRight);
    bool IsTouchingLeftLaneBoundary => transform.position.x < LanePoints[0].transform.position.x;
    bool IsTouchingRightLaneBoundary => transform.position.x > LanePoints[RoadManager.Singleton.NumberOfLanes - 1].transform.position.x;
    bool WheelRotationIsWithinDeadzone => Mathf.Abs(currentTurnSpeed) < deadZone;    
    bool WheelTurnedTooFarAntiClockwise => currentTurnSpeed <= maxLeftTurnSpeed;
    bool WheelTurnedTooFarClockwise => currentTurnSpeed >= maxRightTurnSpeed;
    Vector3 OffLeftBoundary =>
        new Vector3(LanePoints[0].transform.position.x, transform.position.y, transform.position.z);    
    Vector3 OffRightBoundary => 
        new Vector3(LanePoints[RoadManager.Singleton.NumberOfLanes - 1].transform.position.x, transform.position.y, transform.position.z);
    public static PlayerSteering Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Player Car").GetComponent<PlayerSteering>();
            }
            return singleton;
        }
    }
    bool TouchingLeftRail => transform.position == OffLeftBoundary;
    bool TouchingRightRail => transform.position == OffRightBoundary;
    bool WheelIsRotatedAntiClockwise => currentTurnSpeed < 0;
    bool WheelIsRotatedClockwise => currentTurnSpeed > 0;
    bool ScrapingLeftRail => TouchingLeftRail && IsTouchingLeftSideOfScreen;
    bool ScrapingRightRail => TouchingRightRail && IsTouchingRightSideOfScreen;
    bool ScrapingAnyRail => TouchingLeftRail || TouchingRightRail;
    bool AttemptingToSteerLeft => !TouchingLeftRail && IsTouchingLeftSideOfScreen;
    bool AttemptingToSteerRight => !TouchingRightRail && IsTouchingRightSideOfScreen;
    bool AntiClockwiseWheelReleased => !IsTouchingScreen && WheelIsRotatedAntiClockwise;
    bool ClockwiseWheelReleased => !IsTouchingScreen && WheelIsRotatedClockwise;
    #endregion

    #region Methods
    void StraightenWheel() { if (WheelRotationIsWithinDeadzone) { currentTurnSpeed = 0; } }
    void RotateCar() { transform.rotation = Quaternion.Euler(0, currentTurnSpeed * rotationAngleMultiplier, 0); }
    void TurnWheelAntiClockwise()
    {
        if (WheelTurnedTooFarAntiClockwise)
        {
            currentTurnSpeed = maxLeftTurnSpeed;
        }
        else
        {
            currentTurnSpeed -= turnAcceleration * Time.deltaTime;
        }
    }
    void TurnWheelClockwise()
    {
        if (WheelTurnedTooFarClockwise)
        {
            currentTurnSpeed = maxRightTurnSpeed;
        }
        else
        {
            currentTurnSpeed += turnAcceleration * Time.deltaTime;
        }
    }
    void MoveCarHorizontally ()
    {
        transform.Translate(new Vector3(currentTurnSpeed, 0, 0) * Time.deltaTime);
    }

    void PreventCarFromCrossingBoundaries ()
    {
        if (IsTouchingLeftLaneBoundary)
        {
            transform.position = OffLeftBoundary;
        }
        if (IsTouchingRightLaneBoundary)
        {
            transform.position = OffRightBoundary;
        }
    }
    #endregion

    void FixedUpdate()
    {
        MoveCarHorizontally();

        RotateCar();

        PreventCarFromCrossingBoundaries();
    }

    void Update()
    {
        if(ScrapingAnyRail || !IsTouchingScreen)
        {
            StraightenWheel();
        }

        if (AttemptingToSteerRight || ScrapingLeftRail || AntiClockwiseWheelReleased)
        {
            TurnWheelClockwise();
        }

        if (AttemptingToSteerLeft || ScrapingRightRail || ClockwiseWheelReleased)
        {
            TurnWheelAntiClockwise();
        }
    }
}