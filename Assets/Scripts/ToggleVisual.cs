using UnityEngine;

// Предмет с двумя состояниями: выключен / включён
// (лампа, телевизор, холодильник закрыт / открыт).
// Если заданы спрайты — меняет картинку, если нет — только цвет заглушки.
public class ToggleVisual : MonoBehaviour
{
    // Картинка предмета. Если пусто — берём с этого же объекта
    [SerializeField] private SpriteRenderer target;

    [Header("Спрайты (пока рисунков нет, оставить пустыми)")]
    [SerializeField] private Sprite offSprite;
    [SerializeField] private Sprite onSprite;

    [Header("Цвета для заглушек")]
    [SerializeField] private Color offColor = Color.gray;
    [SerializeField] private Color onColor = Color.yellow;

    [SerializeField] private bool isOn = false; // состояние при запуске

    public bool IsOn => isOn; // другие скрипты могут узнать состояние

    private void Awake()
    {
        if (target == null)
        {
            target = GetComponent<SpriteRenderer>();
        }
    }

    private void Start()
    {
        Apply();
    }

    // Переключить: был выключен — включить, и наоборот.
    // Этот метод подключается в On Click у Interactable
    public void Toggle()
    {
        SetOn(!isOn);
    }

    // Включить или выключить явно (пригодится, например, для сна)
    public void SetOn(bool value)
    {
        isOn = value;
        Apply();
    }

    // Показать текущее состояние
    private void Apply()
    {
        Sprite sprite = isOn ? onSprite : offSprite;

        if (sprite != null)
        {
            // Есть рисунок: меняем картинку, цвет оставляем белым (без оттенка)
            target.sprite = sprite;
            target.color = Color.white;
        }
        else
        {
            // Рисунка нет: красим заглушку
            target.color = isOn ? onColor : offColor;
        }
    }
}
