using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    public class LocalizedText : MonoBehaviour
    {
        public string LocalizationKey;

        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
            if(GetComponent<Text>() == null)
            {
                gameObject.AddComponent<TextMeshProUGUI>();
            }
        }

        public void OnDestroy()
        {
            LocalizationManager.LocalizationChanged -= Localize;
        }

        public void Localize()
        {
            if(GetComponent<TextMeshProUGUI>() != null)
            {
                GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(LocalizationKey);
            }
            else
            {
                GetComponent<Text>().text = LocalizationManager.Localize(LocalizationKey);
            }
        }
    }
}