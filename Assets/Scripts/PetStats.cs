using UnityEngine;

// Состояние питомца: сытость, чистота, энергия.
// Значения от 0 до 100, со временем убывают.
// Вешается на объект Pet.
public class PetStats : MonoBehaviour
{
    // Максимальное значение любой шкалы
    public const float MaxValue = 100f;

    [Header("Шкалы на экране")]
    [SerializeField] private StatBar satietyBar;     // сытость
    [SerializeField] private StatBar cleanlinessBar; // чистота
    [SerializeField] private StatBar energyBar;      // энергия

    [Header("Скорость убывания, единиц в минуту")]
    [SerializeField] private float satietyDecay = 2f;
    [SerializeField] private float cleanlinessDecay = 1f;
    [SerializeField] private float energyDecay = 1.5f;

    [Header("Пока игра закрыта")]
    // Множитель скорости убывания в оффлайне: 0.5 — вдвое медленнее, чем в игре
    [SerializeField] private float offlineDecayMultiplier = 0.5f;

    // Текущие значения. Читать их могут все скрипты,
    // а менять только через методы ниже
    public float Satiety { get; private set; } = MaxValue;
    public float Cleanliness { get; private set; } = MaxValue;
    public float Energy { get; private set; } = MaxValue;

    // Update вызывается Unity каждый кадр
    private void Update()
    {
        // Time.deltaTime — сколько секунд прошло с прошлого кадра.
        // Делим скорость на 60, потому что она задана в минутах
        Satiety = Decrease(Satiety, satietyDecay);
        Cleanliness = Decrease(Cleanliness, cleanlinessDecay);
        Energy = Decrease(Energy, energyDecay);

        UpdateBars();
    }

    // Уменьшить значение за прошедший кадр, но не ниже нуля
    private float Decrease(float value, float decayPerMinute)
    {
        return Mathf.Max(0f, value - decayPerMinute / 60f * Time.deltaTime);
    }

    // Передать значения шкалам (они ждут число от 0 до 1)
    private void UpdateBars()
    {
        satietyBar.SetValue(Satiety / MaxValue);
        cleanlinessBar.SetValue(Cleanliness / MaxValue);
        energyBar.SetValue(Energy / MaxValue);
    }

    // Подставить значения из сохранения (вызывает SaveManager при запуске)
    public void LoadValues(float satiety, float cleanliness, float energy)
    {
        Satiety = Mathf.Clamp(satiety, 0f, MaxValue);
        Cleanliness = Mathf.Clamp(cleanliness, 0f, MaxValue);
        Energy = Mathf.Clamp(energy, 0f, MaxValue);

        UpdateBars();
    }

    // Уменьшить шкалы за время, пока игра была закрыта
    public void ApplyOfflineTime(float minutesAway)
    {
        float minutes = minutesAway * offlineDecayMultiplier;

        Satiety = Mathf.Max(0f, Satiety - satietyDecay * minutes);
        Cleanliness = Mathf.Max(0f, Cleanliness - cleanlinessDecay * minutes);
        Energy = Mathf.Max(0f, Energy - energyDecay * minutes);

        UpdateBars();
    }

    // Методы для еды, мытья и сна. amount может быть и отрицательным.
    // Mathf.Clamp не даёт значению выйти за пределы 0..100
    public void ChangeSatiety(float amount)
    {
        Satiety = Mathf.Clamp(Satiety + amount, 0f, MaxValue);
    }

    public void ChangeCleanliness(float amount)
    {
        Cleanliness = Mathf.Clamp(Cleanliness + amount, 0f, MaxValue);
    }

    public void ChangeEnergy(float amount)
    {
        Energy = Mathf.Clamp(Energy + amount, 0f, MaxValue);
    }

    // Тестовая кнопка: появляется в меню компонента (три точки справа).
    // Удобно проверять, работает ли изменение шкал, пока нет холодильника
    [ContextMenu("Тест: покормить на 30")]
    private void TestFeed()
    {
        ChangeSatiety(30f);
    }
}
