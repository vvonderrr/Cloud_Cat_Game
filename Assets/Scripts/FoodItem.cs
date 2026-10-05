using UnityEngine;

// Данные одного продукта: картинка, цена, польза.
// Это не компонент, а файл-ассет: создаётся в окне Project
// через Create → Cloud Pet → Food Item.
[CreateAssetMenu(fileName = "Food_", menuName = "Cloud Pet/Food Item")]
public class FoodItem : ScriptableObject
{
    [Tooltip("Название латиницей, без пробелов. Пригодится для сохранений и рецептов")]
    public string id;

    [Header("Внешний вид")]
    public Sprite icon;                          // рисунок продукта (пока пусто)
    public Color placeholderColor = Color.white; // цвет кружка-заглушки, пока рисунка нет

    [Header("Цена и польза")]
    public int price = 5;        // сколько стоит в монетках
    public float satiety = 15f;  // сколько сытости даёт
    public int xp = 2;           // сколько опыта даёт
}
