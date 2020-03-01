using System.Collections.Generic;
using Assets.TeamProjects.GamePrimal.Proxies;
using Assets.TeamProjects.GamePrimal.SeparateComponents.EventsStructs;
using Assets.TeamProjects.HudInterface.MainMenu;
using UnityEngine;

public class VelocityObjectTrigger : MonoBehaviour
{
    private List<MenuBulletCollision> _mbc;
    private Vector3 _initPos;
    private bool _onLoose = false;
    private Quaternion _possibleRot;
    private CandleLightHolder[] _candleHolder;
    private MenuBulletCollision _mbcLocalOne;

    void Start()
    {
        _mbc = new List<MenuBulletCollision>(FindObjectsOfType<MenuBulletCollision>());
        _initPos = transform.position;
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        eulerAngles.y += 45;
        _possibleRot = Quaternion.Euler(eulerAngles);
        _candleHolder = FindObjectsOfType<CandleLightHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !_onLoose)
            foreach (MenuBulletCollision mbcPublic in _mbc)
                if (mbcPublic.PickRightNow)
                {
                    _mbcLocalOne = mbcPublic;

                    gameObject.AddComponent<Rigidbody>();

                    _initPos.z += mbcPublic.Position;
                    transform.position = _initPos;
                    _onLoose = true;
                }

        if (_onLoose)
            transform.rotation = Quaternion.Lerp(transform.rotation, _possibleRot, Time.deltaTime * 3);
    }

    private void OnTriggerEnter()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());

        foreach (CandleLightHolder candle in _candleHolder) 
            candle.gameObject.SetActive(false);

        if (_mbcLocalOne.ButtonPurpose == MenuButtonStates.SingleGame)
            Invoke("ShiftScene", 1);
        else if (_mbcLocalOne.ButtonPurpose == MenuButtonStates.CoopButton)
            Invoke("ShiftScene", 1);
        else if (_mbcLocalOne.ButtonPurpose == MenuButtonStates.ExitButton)
            Application.Quit();
        else
            Invoke("ShiftScene", 1);
    }

    private void ShiftScene()
    {
        StaticProxyEvent.EMatchHasComeToAnEnd.Invoke(new EventMatchHasComeToAnEndParams());
//        ControllerSceneShift css = FindObjectOfType<ControllerSceneShift>();
//
//        if (css)
//            css.LoadSceneByIndex(1);
    }
}
