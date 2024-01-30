using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button axisYForLeftButton;
    public Button axisYForRightButton;
    public Button axisXForUpButton;
    public Button axisXForDownButton;

    private void UpdateButtonStates ()
    {
        bool isRotating = ShipComponent.IsRotating;
        axisYForLeftButton.enabled = isRotating;
        axisYForRightButton.enabled = isRotating;
        axisXForUpButton.enabled = isRotating;
        axisXForDownButton.enabled = isRotating;
    }
}
