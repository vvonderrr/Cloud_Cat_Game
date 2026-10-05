using UnityEngine;
using UnityEngine.SceneManagement;

// Стартовый экран: кнопка "Играть" и окно настроек.
// Вешается на объект MenuController в сцене MainMenu.
public class MainMenu : MonoBehaviour
{
    // Имя сцены с игрой. Должно точно совпадать с именем файла сцены
    [SerializeField] private string gameSceneName = "Game";

    // Окно настроек. Сюда перетащить объект SettingsPanel
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        // При запуске окно настроек спрятано
        settingsPanel.SetActive(false);
    }

    // Кнопка "Играть": загружаем сцену с питомцем
    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    // Кнопка-шестерёнка: показываем окно настроек
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    // Кнопка ×: прячем окно настроек
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
