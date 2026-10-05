using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Одна ячейка с продуктом в окне холодильника: картинка и цена.
// Нажатие — попытка купить. Вешается на каждую ячейку Slot_...
[RequireComponent(typeof(Button))]
public class FoodSlotUI : MonoBehaviour
{
    [SerializeField] private FoodItem item;       // какой продукт в этой ячейке
    [SerializeField] private Image icon;          // картинка продукта (объект Icon)
    [SerializeField] private TMP_Text priceText;  // цена (объект Price)

    private Button button;
    private FridgePanel fridge;

    public FoodItem Item => item;

    // Настроить ячейку. Вызывает FridgePanel при запуске игры
    public void Init(FridgePanel owner)
    {
        fridge = owner;
        button = GetComponent<Button>();

        // Подписываем кнопку из кода — вместо настройки On Click в Inspector
        button.onClick.AddListener(OnClick);

        if (item.icon != null)
        {
            icon.sprite = item.icon;
            icon.color = Color.white;
        }
        else
        {
            icon.color = item.placeholderColor;
        }

        priceText.text = item.price.ToString();
    }

    // Можно ли сейчас купить: если нет, кнопка становится серой и не нажимается
    public void SetAvailable(bool available)
    {
        if (button != null)
        {
            button.interactable = available;
        }
    }

    private void OnClick()
    {
        fridge.Buy(item);
    }
}
