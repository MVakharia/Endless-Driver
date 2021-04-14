using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection4
{
    None, Up, Right, Down, Left
}

public class SwipeDetector : MonoBehaviour
{
    private static SwipeDetector singleton;
    [SerializeField]
    Vector2 lastTouchPoint;
    [SerializeField]
    bool hasTouched;
    [SerializeField]
    Vector2 lastReleasePoint;
    [SerializeField]
    bool hasReleased;
    [SerializeField]
    float pointDifferenceX;
    [SerializeField]
    float pointDifferenceY;
    [SerializeField]
    SwipeDirection4 swipeDirection;
    [SerializeField]
    float swipeThreshold;

    public static SwipeDetector Singleton
    {
        get
        {
            if(singleton == null)
            {
                singleton = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<SwipeDetector>();
            }
            return singleton;
        }
    }

    public Vector2 LastTouchPoint { get => lastTouchPoint; private set => lastTouchPoint = value; }
    public bool HasTouched { get => hasTouched; private set => hasTouched = value; }
    public Vector2 LastReleasePoint { get => lastReleasePoint; private set => lastReleasePoint = value; }
    public bool HasReleased { get => hasReleased; private set => hasReleased = value; }
    public float PointDifferenceX { get => pointDifferenceX; private set => pointDifferenceX = value; }
    public float PointDifferenceY { get => pointDifferenceY; private set => pointDifferenceY = value; }
    public SwipeDirection4 SwipeDirection { get => swipeDirection; private set => swipeDirection = value; }
    public float SwipeThreshold { get => swipeThreshold; private set => swipeThreshold = value; }
    
    bool IsTouchingScreen() { return Input.touchCount > 0; }
    void TouchStarted() { HasTouched = true; }
    void TouchEnded() { HasReleased = true; }
    void SetTouchPoint() { LastTouchPoint = Input.GetTouch(0).position; }
    void SetReleasePoint() { LastReleasePoint = Input.GetTouch(0).position; }
    bool HasTouchedAndReleased() { return HasTouched && HasReleased; }
    void ResetTouchAndRelease() { HasTouched = false; HasReleased = false; }
    void ResetTouchPoints() { LastTouchPoint = new Vector2(); LastReleasePoint = new Vector2(); }
    void CalculateTouchPointDifference()
    {
        PointDifferenceX = Mathf.Abs(LastTouchPoint.x - LastReleasePoint.x);
        PointDifferenceY = Mathf.Abs(LastTouchPoint.y - LastReleasePoint.y);
    }

    SwipeDirection4 LastSwipeDirection()
    {
        if (HorizontalSwipeWasAboveThreshold())
        {
            if (LastTouchPoint.x < LastReleasePoint.x)
            {
                return SwipeDirection4.Right;
            }
            else if (LastTouchPoint.x > LastReleasePoint.x)
            {
                return SwipeDirection4.Left;
            }
        }
        else if (VerticalSwipeWasABoveThreshold())
        {
            if (LastTouchPoint.y < LastReleasePoint.y)
            {
                return SwipeDirection4.Up;
            }
            else if (LastTouchPoint.y > LastReleasePoint.y)
            {
                return SwipeDirection4.Down;
            }
        }
        return SwipeDirection4.None;
    }
    bool HorizontalSwipeWasAboveThreshold() { return PointDifferenceX > SwipeThreshold; }
    bool VerticalSwipeWasABoveThreshold() { return pointDifferenceY > SwipeThreshold; }

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
            SwipeDirection = LastSwipeDirection();
        }
    }
}