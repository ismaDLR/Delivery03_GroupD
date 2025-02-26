using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LanguageButton : MonoBehaviour, IPointerClickHandler
{
    public Language Language;
    private Text _localizedText;

    public void Start()
    {
        _localizedText = gameObject.GetComponentInChildren<Text>();
        _localizedText.text = Localizer.GetLanguage().ToString();
        Language = Localizer.GetLanguage();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Language += 1;
        if (Language > Language.Spanish) Language = Language.English;

        Localizer.SetLanguage(Language);

        // Make sure button displays current language text
        _localizedText.text = Language.ToString();
        Localizer.SetLanguage(Localizer.GetLanguage());
    }
}
