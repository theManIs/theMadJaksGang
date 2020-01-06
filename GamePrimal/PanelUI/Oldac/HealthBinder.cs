using Assets.GamePrimal.Mono;
using Assets.TeamProjects.GamePrimal.MainScene;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.PanelUI
{
    public class HealthBinder : MonoBehaviour
    {
        private Slider _healthSlider;
        private MainScene _ms;

        // Start is called before the first frame update
        void Start()
        {
            _healthSlider = GetComponent<Slider>();
            _ms = FindObjectOfType<MainScene>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_ms.GetFocus())
                if (_ms.GetFocus().GetComponent<MonoAmplifierRpg>())
                    _healthSlider.value = _ms.GetFocus().GetComponent<MonoAmplifierRpg>().ViewHealth();
        }
    }
}
