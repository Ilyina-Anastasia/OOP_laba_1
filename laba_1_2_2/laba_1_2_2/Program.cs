using System;

namespace laba_1_2_2
{
    // ==================== Абстрактные продукты ====================
    public interface IButton
    {
        void Render();
    }

    public interface ITextBox
    {
        void Render();
    }

    public interface ILabel
    {
        void Render();
    }

    // ==================== Конкретные продукты для Light темы ====================
    public class LightButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("Кнопка в светлой теме: белый фон, чёрный текст, серая рамка.");
        }
    }

    public class LightTextBox : ITextBox
    {
        public void Render()
        {
            Console.WriteLine("Текстовое поле в светлой теме: белый фон, чёрный текст.");
        }
    }

    public class LightLabel : ILabel
    {
        public void Render()
        {
            Console.WriteLine("Метка в светлой теме: чёрный текст на белом фоне.");
        }
    }

    // ==================== Конкретные продукты для Dark темы ====================
    public class DarkButton : IButton
    {
        public void Render()
        {
            Console.WriteLine("Кнопка в тёмной теме: чёрный фон, белый текст, тёмно-серая рамка.");
        }
    }

    public class DarkTextBox : ITextBox
    {
        public void Render()
        {
            Console.WriteLine("Текстовое поле в тёмной теме: чёрный фон, белый текст.");
        }
    }

    public class DarkLabel : ILabel
    {
        public void Render()
        {
            Console.WriteLine("Метка в тёмной теме: белый текст на чёрном фоне.");
        }
    }

    // ==================== Абстрактная фабрика ====================
    public interface IThemeFactory
    {
        IButton CreateButton();
        ITextBox CreateTextBox();
        ILabel CreateLabel();
    }

    // ==================== Конкретные фабрики ====================
    public class LightThemeFactory : IThemeFactory
    {
        public IButton CreateButton()
        {
            return new LightButton();
        }

        public ITextBox CreateTextBox()
        {
            return new LightTextBox();
        }

        public ILabel CreateLabel()
        {
            return new LightLabel();
        }
    }

    public class DarkThemeFactory : IThemeFactory
    {
        public IButton CreateButton()
        {
            return new DarkButton();
        }

        public ITextBox CreateTextBox()
        {
            return new DarkTextBox();
        }

        public ILabel CreateLabel()
        {
            return new DarkLabel();
        }
    }

    // ==================== Клиентский код ====================
    public class Application
    {
        private readonly IButton _button;
        private readonly ITextBox _textBox;
        private readonly ILabel _label;

        public Application(IThemeFactory factory)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            _button = factory.CreateButton();
            _textBox = factory.CreateTextBox();
            _label = factory.CreateLabel();
        }

        public void RenderUI()
        {
            Console.WriteLine("\n--- Отрисовка интерфейса ---");
            _button.Render();
            _textBox.Render();
            _label.Render();
        }

        // Дополнительный метод для демонстрации работы с абстракциями
        public void ShowProductTypes()
        {
            Console.WriteLine($"  Тип кнопки: {_button.GetType().Name}");
            Console.WriteLine($"  Тип текстового поля: {_textBox.GetType().Name}");
            Console.WriteLine($"  Тип метки: {_label.GetType().Name}");
        }
    }

    // ==================== Демонстрация ====================
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Абстрактная фабрика — пример с темами ===\n");

            // Демонстрация светлой темы
            Console.WriteLine("1. Создание светлой темы:");
            IThemeFactory lightFactory = new LightThemeFactory();
            Application lightApp = new Application(lightFactory);
            lightApp.RenderUI();

            // Демонстрация тёмной темы
            Console.WriteLine("\n2. Создание тёмной темы:");
            IThemeFactory darkFactory = new DarkThemeFactory();
            Application darkApp = new Application(darkFactory);
            darkApp.RenderUI();

            // Доказательство: клиент работает только через абстракции
            Console.WriteLine("\n=== ДОКАЗАТЕЛЬСТВО: клиент работает только через абстракции ===");
            Console.WriteLine("Клиент (Application) использует:");
            lightApp.ShowProductTypes();

            Console.WriteLine("\nКлиент не знает о конкретных классах:");
            Console.WriteLine($"  Фабрика: {lightFactory.GetType().Name} - подставляется извне");
            Console.WriteLine($"  Application не содержит прямых ссылок на LightButton, DarkButton и т.д.");

            // Проверка на ошибки
            Console.WriteLine("\n=== ПРОВЕРКА ОБРАБОТКИ ОШИБОК ===");
            try
            {
                Application badApp = new Application(null);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"  Ошибка перехвачена: {ex.Message}");
            }

            Console.WriteLine("\n=== КОНЕЦ ДЕМОНСТРАЦИИ ===");
            Console.ReadKey();
        }
    }
}
