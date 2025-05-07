using Game;
using SmartMVC;
using StarworkGC.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AttentionPanel : View
{
    public static AttentionPanel Singleton { get; private set; }
    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text text;
    [SerializeField] Button btn_open_option;

    public static bool openFromAttentionPanel;

    private void Awake()
    {
        Singleton = this;
    }

    internal void ActiveAttentionPanel(string text = "pressTheButton")
    {
        panel.gameObject.SetActive(true);
        this.text.text = Localization.Get(text);
    }

    private void OnEnable()
    {
        btn_open_option.onClick.RemoveAllListeners();
        btn_open_option.onClick.AddListener(delegate {
            GameApplication.Singleton.view.BookView.saveScrollValue = GameApplication.Singleton.view.BookView.scrollRect.verticalScrollbar.value;
            SettingsMenuView.TurnOnLanguageButton = false;
            panel.gameObject.SetActive(false);
            BookView.gameOpenedFromSettingsInGame = true;
            openFromAttentionPanel = true;
            Notify(MVCEvents.OPEN_SETTING);
        });
    }
}
