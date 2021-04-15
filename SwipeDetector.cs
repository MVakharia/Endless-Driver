using UnityEngine;

public enum SwipeDirection4
{
    None, Up, Right, Down, Left
}

public class SwipeDetector : MonoBehaviour
{
    private static SwipeDetector singleton;

    public static SwipeDetector Singleton
    {
        get
        {
            if (singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<SwipeDetector>();
            }
            return singleton;
        }
    }

    #region Fields
    [SerializeField] Vector2 lastTouchPoint;
    [SerializeField] bool hasTouched;
    [SerializeField] Vector2 lastReleasePoint;
    [SerializeField] bool hasReleased;
    [SerializeField] float pointDifferenceX;
    [SerializeField] float pointDifferenceY;
    [SerializeField] SwipeDirection4 swipeDirection;
    [SerializeField] float swipeThreshold;
    #endregion

    #region Properties
    bool IsTouchingScreen() => Input.touchCount > 0;
    bool HasTouchedAndReleased() => hasTouched && hasReleased;
    bool HorizontalSwipeWasAboveThreshold() => pointDifferenceX > swipeThreshold;
    bool VerticalSwipeWasABoveThreshold() => pointDifferenceY > swipeThreshold;
    SwipeDirection4 LastSwipeDirection
    {
        get
        {
            if (HorizontalSwipeWasAboveThreshold())
            {
                if (lastTouchPoint.x < lastReleasePoint.x)
                {
                    return SwipeDirection4.Right;
                }
                else if (lastTouchPoint.x > lastReleasePoint.x)
                {
                    return SwipeDirection4.Left;
                }
            }
            else if (VerticalSwipeWasABoveThreshold())
            {
                if (lastTouchPoint.y < lastReleasePoint.y)
                {
                    return SwipeDirection4.Up;
                }
                else if (lastTouchPoint.y > lastReleasePoint.y)
                {
                    return SwipeDirection4.Down;
                }
            }
            return SwipeDirection4.None;
        }
    }
    #endregion

    #region Methods
    void TouchStarted() => hasTouched = true;
    void TouchEnded() => hasReleased = true;
    void SetTouchPoint() => lastTouchPoint = Input.GetTouch(0).position;
    void SetReleasePoint() => lastReleasePoint = Input.GetTouch(0).position;    
    void ResetTouchAndRelease() { hasTouched = false; hasReleased = false; }
    void ResetTouchPoints() { lastTouchPoint = new Vector2(); lastReleasePoint = new Vector2(); }
    void CalculateTouchPointDifference()
    {
        pointDifferenceX = Mathf.Abs(lastTouchPoint.x - lastReleasePoint.x);
        pointDifferenceY = Mathf.Abs(lastTouchPoint.y - lastReleasePoint.y);
    }
    #endregion

    private void Update()
    {
        if (IsTouchingScreen())
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                TouchStarted();
                SetTouchPoint();
            }

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                SetReleasePoint();
                TouchEnded();
            }
        }

        if (HasTouchedAndReleased())
        {
            ResetTouchAndRelease();
            CalculateTouchPointDifference();
            swipeDirection = LastSwipeDirection;
        }
    }
}