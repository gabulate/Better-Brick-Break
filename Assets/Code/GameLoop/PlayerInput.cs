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
    private Transform stick;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        if (GameManager.canThrow)
        {
            CheckClick();
            CheckTouch();
        }
    }

    private void CheckClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) //If clicked a UI element
            return;
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
                // Record the starting touch position
                startTouchPosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetKey(KeyCode.Mouse0))
        {
            // Record the ending touch position
            endTouchPosition = _cam.ScreenToWorldPoint(Input.mousePosition);

            // Calculate the throw direction and force
            throwDirection = startTouchPosition - endTouchPosition;
            throwDirection.Normalize();

            //Checks if pointing downwards, and clamps it to either side
            if (throwDirection.y <= 0.1)
            {
                throwDirection.y = 0.1f;
                throwDirection.x = throwDirection.x > 0 ? 0.9f : -0.9f;
            }
                

            RotateStick(throwDirection);
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            BallThrower.Instance.StartThrowing(throwDirection);
        }
    }

    private void CheckTouch()
    {
        // Check for touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Record the starting touch position
                startTouchPosition = touch.position;
            }
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Record the ending touch position
                endTouchPosition = touch.position;

                // Calculate the throw direction and force
                throwDirection = startTouchPosition - endTouchPosition;
                throwDirection.Normalize();

                //Checks if pointing downwards, and clamps it to either side
                if (throwDirection.y <= 0.1)
                {
                    throwDirection.y = 0.1f;
                    throwDirection.x = throwDirection.x > 0 ? 0.9f : -0.9f;
                }

                RotateStick(throwDirection);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                BallThrower.Instance.StartThrowing(throwDirection);
            }
        }
    }

    public bool CheckUITouch()
    {
        if (Input.touchCount > 0)
        {
            var touchPosition = Input.GetTouch(0).position;
            Debug.Log(touchPosition.y > 1000);
            return touchPosition.y > 1000;
        }
        return false;
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
        if(!GameManager.gameLost)
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