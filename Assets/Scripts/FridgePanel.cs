using UnityEngine;

// Окно холодильника: показывает продукты и покупает их за монетки.
// Купленный продукт кладётся на тарелку, окно закрывается.
// Вешается на объект GameManager.
public class FridgePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;          // объект FridgePanel на Canvas
    [SerializeField] private PlayerProgress progress;   // GameManager
    [SerializeField] private Plate plate;               // Plate на кухне
    [SerializeField] private ToggleVisual fridgeVisual; // Fridge: открыт / закрыт
    [SerializeField] private FoodSlotUI[] slots;        // все ячейки с продуктами

    private void Awake()
    {
        panel.SetActive(false);

        foreach (FoodSlotUI slot in slots)
        {
            slot.Init(this);
        }
    }

    private void OnEnable()
    {
        // Монетки изменились — пересчитываем, что можно купить
        progress.Changed += RefreshSlots;
    }

    private void OnDisable()
    {
        progress.Changed -= RefreshSlots;
    }

    // Открыть холодильник (подключается к нажатию на Fridge)
    public void Open()
    {
        panel.SetActive(true);
        fridgeVisual.SetOn(true);
        RefreshSlots();
    }

    // Закрыть холодильник (крестик в окне)
    public void Close()
    {
        panel.SetActive(false);
        fridgeVisual.SetOn(false);
    }

    // Купить продукт (вызывает ячейка при нажатии)
    public void Buy(FoodItem item)
    {
        if (!plate.IsEmpty)
        {
            Debug.Log("На тарелке уже есть еда");
            return;
        }

        if (!progress.TrySpendCoins(item.price))
        {
            Debug.Log("Не хватает монеток");
            return;
        }

        plate.PutFood(item);
        Close(); // закрываем окно, чтобы игрок увидел еду на тарелке
    }

    // Сделать доступными только те продукты, которые можно купить прямо сейчас
    private void RefreshSlots()
    {
        foreach (FoodSlotUI slot in slots)
        {
            bool canBuy = plate.IsEmpty && progress.Coins >= slot.Item.price;
            slot.SetAvailable(canBuy);
        }
    }
}
