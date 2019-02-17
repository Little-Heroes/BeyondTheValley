using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [Header("Options")]
    [Range(0f, 2f)] public float handleLimit = 1f;
    public JoystickMode joystickMode = JoystickMode.AllAxis;

    protected Vector2 inputVector = Vector2.zero;

    [Header("Components")]
    public RectTransform background;
    public RectTransform handle;
    public RectTransform touchArea;

    public float Horizontal { get { return inputVector.x; } }
    public float Vertical { get { return inputVector.y; } }
    public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

    protected void BindBackground()
    {
        //clamping the base of the joystick to the right of the touch area
        if (background.position.x > touchArea.offsetMax.x)
            background.position = new Vector2(touchArea.offsetMax.x , background.position.y);
        //clamping the base of the joystick to the top of the touch area
        if (background.position.y > touchArea.offsetMax.y)
            background.position = new Vector2(background.position.x, touchArea.offsetMax.y);
        //clamping the base of the joystick to the left of the touch area
        if (background.position.x < touchArea.offsetMin.x)
            background.position = new Vector2(touchArea.offsetMin.x, background.position.y);
        //clamping the base of the joystick to the bottom of the touch area
        if (background.position.y < touchArea.offsetMin.y)
            background.position = new Vector2(background.position.x, touchArea.offsetMin.y);
    }

    public virtual void Start()
    {
        //touchArea = GetComponent<RectTransform>();
    }

    public virtual void OnDrag(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }

    protected void ClampJoystick()
    {
        if (joystickMode == JoystickMode.Horizontal)
            inputVector = new Vector2(inputVector.x, 0f);
        if (joystickMode == JoystickMode.Vertical)
            inputVector = new Vector2(0f, inputVector.y);
    }
}

public enum JoystickMode { AllAxis, Horizontal, Vertical}
