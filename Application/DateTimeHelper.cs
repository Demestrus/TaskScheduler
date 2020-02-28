using System;

namespace Application
{
    // в реальном проекте такой класс стоило бы вынести в отдельный проект с shared кодом,
    // но для упрощения оставил здесь. Класс вынесен для предотвращения "копипасты" и улучшения
    // читаемости инициализации данных
    public static class DateTimeHelper
    {
        public static DateTime SetTodayTime(string timeSpan) => DateTime.UtcNow.Date.Add(TimeSpan.Parse(timeSpan));
    }
}