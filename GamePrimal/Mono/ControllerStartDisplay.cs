using System.Collections;
using System.Collections.Generic;
using Assets.TeamProjects.GamePrimal.SeparateComponents.ListsOfStuff;
using UnityEngine;

public class ControllerStartDisplay : MonoBehaviour
{
    void Start()
    {
        if (!FindObjectOfType<StartCanvasHoler>())
            Instantiate(Resources.Load<StartCanvasHoler>(ResourcesList.StartDisplay));
    }
}
