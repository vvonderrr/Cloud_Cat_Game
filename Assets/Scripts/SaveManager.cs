using System;
using UnityEngine;
using YG;

// Сохранение и загрузка игры через PluginYG2.
// При запуске переносит данные из сохранения в PetStats и PlayerProgress
// и уменьшает шкалы за время, пока игра была закрыта.
// Во время игры сохраняет сама: регулярно, после изменения монеток и уровня
// и когда игрок переключается на другую вкладку.
// Вешается на объект GameManager.
public class SaveManager : MonoBehaviour
{
    [SerializeField] private PetStats petStats;      // сюда перетащить GameManager
    [SerializeField] private PlayerProgress progress; // и сюда тоже GameManager

    [Header("Автосохранение, в секундах")]
    [SerializeField] private float autoSaveInterval = 30f; // сохранять не реже, чем раз в столько секунд
    [SerializeField] private float minSaveGap = 3f;        // и не чаще, чем раз в столько секунд

    private bool isLoaded;          // пока сохранение не загружено, сохранять нельзя,
                                    // иначе затрём данные игрока значениями по умолчанию
    private bool saveRequested;     // кто-то попросил сохранить "при первой возможности"
    private float lastSaveRealtime; // когда сохраняли последний раз (время с начала игры)
    private bool isQuitting;        // игра закрывается

    private void OnEnable()
    {
        // onGetSDKData — событие плагина: данные платформы (и сохранения) готовы
        YG2.onGetSDKData += Load;
        progress.Changed += RequestSave;
    }

    // Unity вызывает это при закрытии игры (и при остановке Play в редакторе),
    // раньше, чем OnDisable
    private void OnApplicationQuit()
    {
        isQuitting = true;
    }

    private void OnDisable()
    {
        // Уходим со сцены — сохраняемся напоследок.
        // Но не при закрытии игры: плагин в этот момент уже выключается
        if (!isQuitting)
        {
            Save();
        }

        YG2.onGetSDKData -= Load;
        progress.Changed -= RequestSave;
    }

    private void Start()
    {
        // Если плагин уже успел загрузить данные (например, пока был открыт стартовый экран),
        // событие уже прошло — загружаемся сами
        if (YG2.isSDKEnabled)
        {
            Load();
        }
    }

    private void Update()
    {
        if (!isLoaded)
        {
            return;
        }

        float sinceLastSave = Time.unscaledTime - lastSaveRealtime;

        bool requestedAndAllowed = saveRequested && sinceLastSave >= minSaveGap;
        bool timeForAutoSave = sinceLastSave >= autoSaveInterval;

        if (requestedAndAllowed || timeForAutoSave)
        {
            Save();
        }
    }

    // Игрок переключился на другую вкладку или окно — сохраняемся
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            Save();
        }
    }

    // То же для мобильных устройств, когда игру сворачивают
    private void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            Save();
        }
    }

    // Перенести данные из сохранения в игру
    private void Load()
    {
        SavesYG saves = YG2.saves;

        petStats.LoadValues(saves.satiety, saves.cleanliness, saves.energy);
        progress.LoadValues(saves.coins, saves.level, saves.xp);

        // Сколько времени игрок отсутствовал
        if (saves.lastSaveTime > 0)
        {
            long secondsAway = GetUnixTimeNow() - saves.lastSaveTime;

            // Отрицательное время бывает, если на устройстве переводили часы — тогда пропускаем
            if (secondsAway > 0)
            {
                petStats.ApplyOfflineTime(secondsAway / 60f);
            }
        }

        isLoaded = true;
        Save(); // сразу запоминаем новое время и уменьшенные шкалы
    }

    // Попросить сохранить при первой возможности
    private void RequestSave()
    {
        saveRequested = true;
    }

    // Перенести данные из игры в сохранение и отправить в облако
    public void Save()
    {
        // Не сохраняем, пока не загрузились или если плагин не готов
        if (!isLoaded || !YG2.isSDKEnabled)
        {
            return;
        }

        SavesYG saves = YG2.saves;

        saves.satiety = petStats.Satiety;
        saves.cleanliness = petStats.Cleanliness;
        saves.energy = petStats.Energy;

        saves.coins = progress.Coins;
        saves.level = progress.Level;
        saves.xp = progress.Xp;

        saves.lastSaveTime = GetUnixTimeNow();

        YG2.SaveProgress();

        saveRequested = false;
        lastSaveRealtime = Time.unscaledTime;
    }

    // Текущее время в секундах (Unix-время, по часам устройства)
    private static long GetUnixTimeNow()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    // Тестовая кнопка: начать игру "с нуля".
    // Плагин сбросит сохранение и вызовет onGetSDKData, после чего сработает Load
    [ContextMenu("Тест: сбросить сохранения")]
    private void TestResetSaves()
    {
        YG2.SetDefaultSaves();
    }
}
