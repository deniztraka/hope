using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButtonBehaviour : MonoBehaviour
{


    public void OnClick()
    {
        var results = Resources.FindObjectsOfTypeAll<ExitGameCanvasPopUp>();
        var exitGameCanvas = results[0].gameObject;
        exitGameCanvas.SetActive(true);
    }
}
