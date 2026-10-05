using System.Collections;
using UnityEngine;

// Моргание персонажа: через случайные промежутки времени глаза ненадолго закрываются.
// Вешается на объект Pet.
public class PetBlink : MonoBehaviour
{
    [Header("Глаза")]
    // Сюда в Inspector перетащить Eye_L и Eye_R
    [SerializeField] private SpriteRenderer[] eyes;

    // Спрайт закрытых глаз. Пока рисунков нет, оставляем пустым:
    // тогда глаза будут "закрываться" сплющиванием по вертикали
    [SerializeField] private Sprite closedEyesSprite;

    [Header("Время, в секундах")]
    [SerializeField] private float minInterval = 2f;       // минимальная пауза между морганиями
    [SerializeField] private float maxInterval = 5f;       // максимальная пауза между морганиями
    [SerializeField] private float closedDuration = 0.12f; // сколько глаза остаются закрытыми

    // Запоминаем, как глаза выглядели открытыми, чтобы потом вернуть
    private Sprite[] openSprites;
    private Vector3[] openScales;

    // Start вызывается Unity один раз, перед первым кадром
    private void Start()
    {
        openSprites = new Sprite[eyes.Length];
        openScales = new Vector3[eyes.Length];

        for (int i = 0; i < eyes.Length; i++)
        {
            openSprites[i] = eyes[i].sprite;
            openScales[i] = eyes[i].transform.localScale;
        }

        // Запускаем бесконечный цикл моргания
        StartCoroutine(BlinkLoop());
    }

    // Корутина: функция, которая умеет "ждать", не останавливая игру
    private IEnumerator BlinkLoop()
    {
        while (true)
        {
            // Ждём случайное время
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Закрываем глаза, ждём долю секунды и открываем обратно
            SetEyesClosed(true);
            yield return new WaitForSeconds(closedDuration);
            SetEyesClosed(false);
        }
    }

    // Закрыть или открыть глаза
    private void SetEyesClosed(bool closed)
    {
        for (int i = 0; i < eyes.Length; i++)
        {
            if (closedEyesSprite != null)
            {
                // Есть настоящий рисунок: просто меняем картинку
                eyes[i].sprite = closed ? closedEyesSprite : openSprites[i];
            }
            else
            {
                // Рисунка нет: сплющиваем заглушку по вертикали
                Vector3 scale = openScales[i];
                if (closed)
                {
                    scale.y *= 0.15f;
                }
                eyes[i].transform.localScale = scale;
            }
        }
    }
}
