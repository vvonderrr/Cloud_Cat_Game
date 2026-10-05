using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

// Предмет в комнате, на который можно нажать (мышкой или пальцем).
// Сам ничего не делает: при нажатии слегка "пружинит"
// и вызывает всё, что подключено в списке On Click в Inspector.
// Нужен коллайдер (Box Collider 2D или Circle Collider 2D)
// и Physics 2D Raycaster на камере.
[RequireComponent(typeof(Collider2D))]
public class Interactable : MonoBehaviour, IPointerClickHandler
{
    // Список действий при нажатии — такой же, как у кнопок интерфейса
    [SerializeField] private UnityEvent onClick;

    [Header("Пружинка при нажатии")]
    [SerializeField] private float squashScale = 0.9f;     // насколько сжимается
    [SerializeField] private float squashDuration = 0.08f; // сколько секунд сжат

    private Vector3 normalScale;
    private bool isSquashing;

    private void Awake()
    {
        // Запоминаем обычный размер предмета
        normalScale = transform.localScale;
    }

    // Этот метод вызывает система событий Unity, когда по коллайдеру нажали.
    // Он обязателен, потому что класс реализует интерфейс IPointerClickHandler
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isSquashing)
        {
            StartCoroutine(Squash());
        }

        onClick.Invoke();
    }

    // Сжать предмет на долю секунды и вернуть обратно
    private IEnumerator Squash()
    {
        isSquashing = true;
        transform.localScale = normalScale * squashScale;
        yield return new WaitForSeconds(squashDuration);
        transform.localScale = normalScale;
        isSquashing = false;
    }
}
