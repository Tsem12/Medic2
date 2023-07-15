using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NaughtyAttributes;

namespace Assets.SimpleLocalization
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    public class LocalizedText : MonoBehaviour
    {
        public string LocalizationKey;
        private TMPro.TMP_FontAsset _baseFont;
        private Font _basebaseFont;

        public void Start()
        {
            Localize();
            LocalizationManager.LocalizationChanged += Localize;
        }
        private void Reset()
        {
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

        [Button]
        private void Test()
        {
            if (GetComponent<TextMeshProUGUI>() != null)
            {
                if (SaveSystem.Load().Language == "Chinois" || SaveSystem.Load().Language == "Japonais")
                {
                    GetComponent<TextMeshProUGUI>().font = null;
                }
                else
                {
                    GetComponent<TextMeshProUGUI>().font = _baseFont;
                }
            }
            else
            {
                if (SaveSystem.Load().Language == "Chinois" || SaveSystem.Load().Language == "Japonais")
                {
                    GetComponent<Text>().font = null;
                }
                else
                {
                    GetComponent<Text>().font = _basebaseFont;
                }
            }
        }
    }
}