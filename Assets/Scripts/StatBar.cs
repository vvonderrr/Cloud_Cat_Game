using UnityEngine;
using UnityEngine.UI;

// Одна шкала на экране (сытость, чистота, энергия...).
// Только показывает значение, сама ничего не считает.
// Вешается на объект StatBar_...
public class StatBar : MonoBehaviour
{
    // Сюда в Inspector перетащить объект Fill этой шкалы
    [SerializeField] private Image fill;

    // Показать значение от 0 (пусто) до 1 (полная шкала)
    public void SetValue(float value01)
    {
        fill.fillAmount = Mathf.Clamp01(value01);
    }
}
