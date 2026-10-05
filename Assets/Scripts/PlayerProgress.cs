using System;
using UnityEngine;

// Прогресс игрока: монетки, опыт и уровень.
// Сам ничего не показывает — только считает и сообщает об изменениях.
// Вешается на объект GameManager.
public class PlayerProgress : MonoBehaviour
{
    // Стартовые монетки задаются в SavesData.cs (поле coins)

    [Header("Уровни")]
    [SerializeField] private int baseXpPerLevel = 100; // сколько опыта нужно с 1 на 2 уровень
    [SerializeField] private int extraXpPerLevel = 50; // на сколько больше нужно каждый следующий уровень
    [SerializeField] private int coinsPerLevelUp = 25; // награда монетками за новый уровень

    // Текущие значения: читать могут все, менять — только через методы ниже
    public int Coins { get; private set; }
    public int Level { get; private set; } = 1;
    public int Xp { get; private set; } // опыт, набранный внутри текущего уровня

    // Сколько опыта нужно, чтобы перейти на следующий уровень.
    // 1 → 2: 100, 2 → 3: 150, 3 → 4: 200 и так далее
    public int XpToNextLevel => baseXpPerLevel + (Level - 1) * extraXpPerLevel;

    // События: на них подписываются другие скрипты (панель, сохранения, эффекты)
    public event Action Changed;        // что-то изменилось
    public event Action<int> LeveledUp; // получен новый уровень (передаётся его номер)

    // Подставить значения из сохранения (вызывает SaveManager при запуске)
    public void LoadValues(int coins, int level, int xp)
    {
        Coins = Mathf.Max(0, coins);
        Level = Mathf.Max(1, level);
        Xp = Mathf.Max(0, xp);

        Changed?.Invoke();
    }

    // Добавить монетки (за мини-игру, за уровень...)
    public void AddCoins(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Coins += amount;
        Changed?.Invoke(); // ?. — вызвать, только если кто-то подписан
    }

    // Попробовать потратить монетки.
    // Возвращает true, если хватило, и false, если нет (тогда ничего не списывается)
    public bool TrySpendCoins(int amount)
    {
        if (amount < 0 || Coins < amount)
        {
            return false;
        }

        Coins -= amount;
        Changed?.Invoke();
        return true;
    }

    // Добавить опыт. Если его хватает на новый уровень — повышаем,
    // причём сразу на несколько уровней, если опыта много
    public void AddXp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        Xp += amount;

        while (Xp >= XpToNextLevel)
        {
            Xp -= XpToNextLevel; // остаток опыта переходит на новый уровень
            Level++;
            Coins += coinsPerLevelUp;
            LeveledUp?.Invoke(Level);
        }

        Changed?.Invoke();
    }

    // Тестовые кнопки в меню компонента (три точки справа)
    [ContextMenu("Тест: +50 монеток")]
    private void TestAddCoins()
    {
        AddCoins(50);
    }

    [ContextMenu("Тест: +40 опыта")]
    private void TestAddXp()
    {
        AddXp(40);
    }

    [ContextMenu("Тест: потратить 30 монеток")]
    private void TestSpendCoins()
    {
        bool success = TrySpendCoins(30);
        Debug.Log(success ? "Потрачено 30 монеток" : "Не хватает монеток");
    }
}
