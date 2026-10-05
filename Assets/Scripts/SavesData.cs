namespace YG
{
    // Данные, которые сохраняются в облако Яндекс Игр.
    // partial значит, что это кусочек класса SavesYG из плагина:
    // мы добавляем к нему свои поля, не меняя файлы самого плагина.
    // Значения после "=" игрок получит при самом первом запуске.
    public partial class SavesYG
    {
        // Питомец (от 0 до 100)
        public float satiety = 100f;
        public float cleanliness = 100f;
        public float energy = 100f;

        // Прогресс игрока
        public int coins = 50; // стартовые монетки
        public int level = 1;
        public int xp = 0;

        // Когда игра сохранялась последний раз:
        // количество секунд с 1 января 1970 года (так называемое Unix-время).
        // 0 значит "ещё ни разу не сохранялась"
        public long lastSaveTime = 0;
    }
}
