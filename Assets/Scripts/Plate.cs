using UnityEngine;

// Тарелка на кухне. На ней может лежать один продукт.
// Вешается на объект Plate.
public class Plate : MonoBehaviour
{
    // Картинка еды на тарелке. Сюда перетащить объект FoodOnPlate
    [SerializeField] private SpriteRenderer foodRenderer;

    private Sprite placeholderSprite; // кружок-заглушка, пока у продуктов нет рисунков

    // Что сейчас лежит на тарелке (null — ничего)
    public FoodItem CurrentFood { get; private set; }
    public bool IsEmpty => CurrentFood == null;

    private void Awake()
    {
        // Запоминаем заглушку и прячем еду: при запуске тарелка пустая
        placeholderSprite = foodRenderer.sprite;
        foodRenderer.enabled = false;
    }

    // Положить продукт на тарелку
    public void PutFood(FoodItem item)
    {
        CurrentFood = item;

        if (item.icon != null)
        {
            // Есть рисунок — показываем его без оттенка
            foodRenderer.sprite = item.icon;
            foodRenderer.color = Color.white;
        }
        else
        {
            // Рисунка нет — показываем кружок цвета продукта
            foodRenderer.sprite = placeholderSprite;
            foodRenderer.color = item.placeholderColor;
        }

        foodRenderer.enabled = true;
    }

    // Забрать продукт с тарелки (например, когда персонаж его съел).
    // Возвращает то, что лежало, или null, если тарелка была пустой
    public FoodItem TakeFood()
    {
        FoodItem food = CurrentFood;
        CurrentFood = null;
        foodRenderer.enabled = false;
        return food;
    }

    // Тестовая кнопка, пока персонаж не умеет есть
    [ContextMenu("Тест: убрать еду с тарелки")]
    private void TestClear()
    {
        TakeFood();
    }
}
