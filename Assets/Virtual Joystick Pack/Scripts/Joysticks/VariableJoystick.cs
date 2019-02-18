using UnityEngine;
using UnityEngine.EventSystems;

public class VariableJoystick : Joystick
{
    [Header("Variable Joystick Options")]
    public bool isFixed = true;
    public Vector2 fixedScreenPosition;
    [Range(0.01f,1)]
    public float snapDist = 0.05f;

    Vector2 joystickCenter = Vector2.zero;

    public override void Start()
    {
        base.Start();
        if (isFixed)
            OnFixed();
        else
            OnFloat();
    }

    public void ChangeFixed(bool joystickFixed)
    {
        if (joystickFixed)
            OnFixed();
        else
            OnFloat();
        isFixed = joystickFixed;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - joystickCenter;
        //if the joystick is being pulled further than it's max, move the background until that is not the case
        if(direction.magnitude > background.sizeDelta.x / 2f)
        {
            //move the background
            background.position = Vector3.Lerp(background.position, eventData.position - direction.normalized, snapDist);
            //set the input vector
            inputVector = direction.normalized;
            //move the centre position for the joystick
            joystickCenter = background.position;
            //clamp the background to the touch area
            //BindBackground();
        }
        else inputVector = direction / (background.sizeDelta.x / 2f);
        //if the magnitude of the direction is larger than the bacground clamp it via a normalisation
        //otherwise the direction vector is equal to the amount the joystick has moved within the backgrounds bounds
        //inputVector = (direction.magnitude > background.sizeDelta.x / 2f) ? direction.normalized : direction / (background.sizeDelta.x / 2f);
        ClampJoystick();
        handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!isFixed)
        {
            background.gameObject.SetActive(true);
            background.position = eventData.position;
            handle.anchoredPosition = Vector2.zero;
            joystickCenter = eventData.position;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!isFixed)
        {
            background.gameObject.SetActive(false);
        }
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    void OnFixed()
    {
        joystickCenter = fixedScreenPosition;
        background.gameObject.SetActive(true);
        handle.anchoredPosition = Vector2.zero;
        background.anchoredPosition = fixedScreenPosition;
    }

    void OnFloat()
    {
        handle.anchoredPosition = Vector2.zero;
        background.gameObject.SetActive(false);
    }
}