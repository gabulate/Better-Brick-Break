using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private Vector2 startTouchPosition;
    [SerializeField]
    private Vector2 endTouchPosition;
    [SerializeField]
    private Vector2 throwDirection;
    private Camera _cam;

    [SerializeField]
    private InputPhase phase = InputPhase.None;
    [SerializeField]
    public InputMethod method = InputMethod.Touch;

    [SerializeField]
    private Transform stick;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        if (AppManager.theme)
            stick.GetComponentInChildren<SpriteRenderer>().color = AppManager.theme.ballColor;
    }

    void Update()
    {
        CheckClick();
        CheckTouch();

        CheckDirection();
    }

    public void CheckDirection()
    {
        if (phase == InputPhase.Started)
        {
            if (BallThrower.isThrowing)
            {
                Time.timeScale = 2;
            }
        }
        else if (phase == InputPhase.Held)
        {
            if(startTouchPosition == endTouchPosition)
            {
                throwDirection = Vector2.up;
            }
            else
            {
                // Calculate the throw direction and force
                throwDirection = startTouchPosition - endTouchPosition;
                throwDirection.Normalize();

                //Checks if pointing downwards, and clamps it to either side
                if (throwDirection.y <= 0.1)
                {
                    throwDirection.y = 0.1f;
                    throwDirection.x = throwDirection.x > 0 ? 0.9f : -0.9f;
                }
            }

            RotateStick(throwDirection);
        }
        if (phase == InputPhase.Ended)
        {
            if (GameManager.canThrow && Time.timeScale == 1)
                BallThrower.Instance.StartThrowing(throwDirection);
            Time.timeScale = 1;
            throwDirection = Vector2.up;
        }
    }

    private void CheckClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) //If clicked a UI element
                return;

            phase = InputPhase.Started;
            method = InputMethod.Mouse;

            startTouchPosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            phase = InputPhase.Held;
            endTouchPosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            phase = InputPhase.Ended;
        }
        else if (method == InputMethod.Mouse)
        {
            phase = InputPhase.None;
        }
    }


    private void CheckTouch()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            method = InputMethod.Touch;

            if (touch.phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) //If clicked a UI element
                    return;

                phase = InputPhase.Started;
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                phase = InputPhase.Held;
                endTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                phase = InputPhase.Ended;
            }
        }
        else if (method == InputMethod.Touch)
        {
            phase = InputPhase.None;
        }
    }

    private void RotateStick(Vector2 direction)
    {
        // Calculate the angle in radians
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // Apply the rotation
        stick.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void DisableStick(int balls)
    {
        stick.gameObject.SetActive(false);
    }

    private void OnStopRecalling()
    {
        if (!GameManager.gameLost)
            StartCoroutine(EnableStick(0.5f));
    }

    private IEnumerator EnableStick(float delay)
    {
        yield return new WaitForSeconds(delay);
        stick.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        GameEvents.e_StartedThrowing.AddListener(DisableStick);
        GameEvents.e_StoppedRecalling.AddListener(OnStopRecalling);
    }

    private void OnDisable()
    {
        GameEvents.e_StartedThrowing.RemoveListener(DisableStick);
        GameEvents.e_StoppedRecalling.RemoveListener(OnStopRecalling);
    }
}

public enum InputPhase
{
    Started, Held, Ended, None
}

public enum InputMethod
{
    Mouse, Touch
}
