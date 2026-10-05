using System;
using UnityEngine;
using UnityEngine.UI;

// Переключение комнат по иконкам на нижней панели.
// Включает нужную комнату, выключает остальные,
// ставит персонажа на точку PetSpot этой комнаты и подсвечивает её иконку.
// Вешается на объект GameManager.
public class RoomManager : MonoBehaviour
{
    // Всё, что относится к одной комнате.
    // [Serializable] нужен, чтобы этот класс показывался в Inspector
    [Serializable]
    public class Room
    {
        public string name;        // подпись для удобства, на игру не влияет
        public GameObject root;    // объект комнаты: MainRoom, Kitchen...
        public Transform petSpot;  // точка, куда встаёт персонаж в этой комнате
        public Image icon;         // иконка комнаты на нижней панели
    }

    [SerializeField] private Transform pet;   // сюда перетащить Pet
    [SerializeField] private Room[] rooms;    // все комнаты по порядку
    [SerializeField] private int startRoom = 0; // с какой комнаты начинается игра

    [Header("Вид иконок")]
    [SerializeField] private Color activeIconColor = Color.white;
    [SerializeField] private Color inactiveIconColor = new Color(1f, 1f, 1f, 0.5f); // полупрозрачная
    [SerializeField] private float activeIconScale = 1.15f; // текущая иконка чуть крупнее

    // Номер текущей комнаты. -1 значит "ещё ни одна не выбрана"
    public int CurrentRoom { get; private set; } = -1;

    private void Start()
    {
        GoToRoom(startRoom);
    }

    // Перейти в комнату с номером index (0 — первая в списке).
    // Этот метод вызывают кнопки-иконки
    public void GoToRoom(int index)
    {
        // Защита от неправильного номера
        if (index < 0 || index >= rooms.Length)
        {
            Debug.LogWarning("Нет комнаты с номером " + index);
            return;
        }

        // Уже здесь — ничего не делаем
        if (index == CurrentRoom)
        {
            return;
        }

        CurrentRoom = index;

        for (int i = 0; i < rooms.Length; i++)
        {
            bool isActive = (i == index);

            // Включаем только нужную комнату
            rooms[i].root.SetActive(isActive);

            // Подсвечиваем иконку текущей комнаты
            if (rooms[i].icon != null)
            {
                rooms[i].icon.color = isActive ? activeIconColor : inactiveIconColor;
                rooms[i].icon.transform.localScale = Vector3.one * (isActive ? activeIconScale : 1f);
            }
        }

        // Ставим персонажа на его место в этой комнате
        if (rooms[index].petSpot != null)
        {
            pet.position = rooms[index].petSpot.position;
        }
    }
}
