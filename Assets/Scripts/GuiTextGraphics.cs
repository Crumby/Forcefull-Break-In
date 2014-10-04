using UnityEngine;
using System.Collections;

public class GuiTextGraphics : MonoBehaviour
{

    public Color buttonSecondColor = Color.white;
    public int buttonHoverResize = 5;
    private Color buttonOriginalColor;

    public void Start()
    {
        buttonOriginalColor = guiText.color;
    }

    public void OnMouseEnter()
    {
        this.guiText.color = buttonSecondColor;
        this.guiText.fontSize = guiText.fontSize + buttonHoverResize;
    }

    public void OnMouseExit()
    {
        this.guiText.color = buttonOriginalColor;
        this.guiText.fontSize = guiText.fontSize - buttonHoverResize;
    }
}
