using System.Collections;
using System.Collections.Generic;
using Assets.GamePrimal.Controllers;
using UnityEngine;
using Assets.GamePrimal.MainScene;
using UnityEngine.UI;

public class LoadBattleSceneMarker : MonoBehaviour
{
    public ControllerSceneShift _ControllerSceneShift;

    void Start()
    {
        _ControllerSceneShift = Object.FindObjectOfType<ControllerSceneShift>();

        Button start = GetComponent<Button>();

        start.onClick.AddListener(() => _ControllerSceneShift.LoadBattleScene());
    }
}
