using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Верхняя панель: счётчик монеток, номер уровня и полоска опыта.
// Только показывает данные из PlayerProgress и обновляется сама, когда они меняются.
// Вешается на Canvas.
public class TopBarUI : MonoBehaviour
{
    [SerializeField] private PlayerProgress progress; // сюда перетащить GameManager
    [SerializeField] private TMP_Text coinsText;      // CoinText
    [SerializeField] private TMP_Text levelText;      // LevelText
    [SerializeField] private Image xpFill;            // XpFill

    // OnEnable вызывается, когда объект включается: подписываемся на изменения
    private void OnEnable()
    {
        progress.Changed += Refresh;
    }

    // OnDisable — когда выключается: отписываемся, чтобы не было ошибок
    private void OnDisable()
    {
        progress.Changed -= Refresh;
    }

    // Первый раз показываем значения сразу при запуске
    private void Start()
    {
        Refresh();
    }

    // Обновить всё, что на панели
    private void Refresh()
    {
        coinsText.text = progress.Coins.ToString();
        levelText.text = progress.Level.ToString();

        // (float) нужен, иначе деление целых чисел даст 0
        xpFill.fillAmount = (float)progress.Xp / progress.XpToNextLevel;
    }
}
