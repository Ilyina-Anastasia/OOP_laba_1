using System;
using System.Collections.Generic;

namespace laba_1_3_2
{
    public class TreeNode
    {
        // Свойства только для чтения - неизменяемость узла
        public string Value { get; }
        public IReadOnlyList<TreeNode> Children => _children.AsReadOnly();

        // Внутренний список для хранения потомков
        private readonly List<TreeNode> _children;

        /// <summary>
        /// Конструктор узла с обязательным значением
        /// </summary
        public TreeNode(string value)
        {
            // Проверка на null или пустую строку
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Значение узла не может быть null или пустым", nameof(value));

            Value = value;
            _children = new List<TreeNode>();
        }

        /// <summary>
        /// Добавление дочернего узла
        /// </summary>
        public void AddChild(TreeNode child)
        {
            if (child == null)
                throw new ArgumentNullException(nameof(child), "Дочерний узел не может быть null");

            _children.Add(child);
        }

        /// <summary>
        /// Добавление нескольких дочерних узлов
        /// </summary>
        public void AddChildren(params TreeNode[] children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
        }

        /// <summary>
        /// Рекурсивный вывод дерева с отображением уровня вложенности
        /// </summary>
        public void PrintTree(int level = 0)
        {
            // Создаём отступ из пробелов в зависимости от уровня
            string indent = new string(' ', level * 2);

            // Выводим текущий узел с отступом
            Console.WriteLine($"{indent}├─ {Value}");

            // Рекурсивно выводим всех потомков
            foreach (var child in Children)
            {
                child.PrintTree(level + 1);
            }
        }

        /// <summary>
        /// Поиск первого узла по значению (рекурсивный обход в глубину)
        /// </summary>
        public TreeNode FindNode(string searchValue)
        {
            // Проверка на null или пустую строку
            if (string.IsNullOrWhiteSpace(searchValue))
                throw new ArgumentException("Значение для поиска не может быть null или пустым", nameof(searchValue));

            // Если текущий узел подходит, возвращаем его
            if (Value.Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                return this;

            // Рекурсивно ищем среди потомков
            foreach (var child in Children)
            {
                TreeNode foundNode = child.FindNode(searchValue);
                if (foundNode != null)
                    return foundNode;
            }

            // Если ничего не нашли, возвращаем null
            return null;
        }

        /// <summary>
        /// Получение всех узлов дерева (рекурсивный обход)
        /// </summary>
        public List<TreeNode> GetAllNodes()
        {
            List<TreeNode> result = new List<TreeNode>();
            GetAllNodesRecursive(result);
            return result;
        }

        /// <summary>
        /// Рекурсивный метод для сбора всех узлов
        /// </summary>
        private void GetAllNodesRecursive(List<TreeNode> nodes)
        {
            nodes.Add(this);

            foreach (var child in Children)
            {
                child.GetAllNodesRecursive(nodes);
            }
        }

        /// <summary>
        /// Переопределение ToString для удобного вывода
        /// </summary>
        public override string ToString()
        {
            return Value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Демонстрация работы дерева с поиском узла ===\n");

            // Создаём дерево с 3 уровнями вложенности
            TreeNode root = CreateSampleTree();

            // Выводим структуру дерева
            Console.WriteLine("--- Структура дерева ---");
            root.PrintTree();

            // Выводим все узлы дерева
            Console.WriteLine("\n--- Все узлы дерева ---");
            List<TreeNode> allNodes = root.GetAllNodes();
            for (int i = 0; i < allNodes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allNodes[i].Value}");
            }

            // Демонстрация поиска существующего узла
            Console.WriteLine("\n--- Поиск существующего узла ---");
            string existingValue = "Подкатегория 1.1";
            TreeNode foundNode = root.FindNode(existingValue);

            if (foundNode != null)
            {
                Console.WriteLine($" Узел '{existingValue}' найден!");
                Console.WriteLine($"  Значение: {foundNode.Value}");
                Console.WriteLine($"  Количество потомков: {foundNode.Children.Count}");

                // Выводим поддерево найденного узла
                if (foundNode.Children.Count > 0)
                {
                    Console.WriteLine($"  Потомки узла '{existingValue}':");
                    foreach (var child in foundNode.Children)
                    {
                        Console.WriteLine($"    - {child.Value}");
                    }
                }
                else
                {
                    Console.WriteLine($"  Узел '{existingValue}' не имеет потомков (лист)");
                }
            }
            else
            {
                Console.WriteLine($" Узел '{existingValue}' не найден!");
            }

            // Демонстрация поиска несуществующего узла
            Console.WriteLine("\n--- Поиск несуществующего узла ---");
            string nonExistingValue = "Такой категории нет";
            TreeNode notFoundNode = root.FindNode(nonExistingValue);

            if (notFoundNode != null)
            {
                Console.WriteLine($" Узел '{nonExistingValue}' найден!");
            }
            else
            {
                Console.WriteLine($" Узел '{nonExistingValue}' не найден!");
                Console.WriteLine($"  Программа корректно сообщила, что узел отсутствует в дереве.");
            }

            // Дополнительная демонстрация: поиск узла на разных уровнях
            Console.WriteLine("\n--- Дополнительные тесты поиска ---");
            TestSearch(root, "Корневая категория");
            TestSearch(root, "Подкатегория 2.1");
            TestSearch(root, "Товар 2.1.1");
            TestSearch(root, "Пустой поиск");

            // Демонстрация обработки ошибок
            Console.WriteLine("\n--- Проверка обработки ошибок ---");
            try
            {
                TreeNode invalidNode = new TreeNode("");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($" Ошибка при создании узла с пустым значением: {ex.Message}");
            }

            try
            {
                root.FindNode("");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($" Ошибка при поиске пустой строки: {ex.Message}");
            }

            Console.WriteLine("\n=== Программа завершена ===");
        }

        /// <summary>
        /// Создание примера дерева с 3 уровнями вложенности
        /// </summary>
        static TreeNode CreateSampleTree()
        {
            // Уровень 3: листья (товары)
            TreeNode product111 = new TreeNode("Товар 1.1.1");
            TreeNode product112 = new TreeNode("Товар 1.1.2");
            TreeNode product211 = new TreeNode("Товар 2.1.1");
            TreeNode product212 = new TreeNode("Товар 2.1.2");

            // Уровень 2: подкатегории
            TreeNode subCategory11 = new TreeNode("Подкатегория 1.1");
            subCategory11.AddChildren(product111, product112);

            TreeNode subCategory12 = new TreeNode("Подкатегория 1.2");

            TreeNode subCategory21 = new TreeNode("Подкатегория 2.1");
            subCategory21.AddChildren(product211, product212);

            TreeNode subCategory22 = new TreeNode("Подкатегория 2.2");

            // Уровень 1: основные категории
            TreeNode category1 = new TreeNode("Категория 1");
            category1.AddChildren(subCategory11, subCategory12);

            TreeNode category2 = new TreeNode("Категория 2");
            category2.AddChildren(subCategory21, subCategory22);

            TreeNode category3 = new TreeNode("Категория 3");
            // Категория 3 без подкатегорий

            // Уровень 0: корневой узел
            TreeNode root = new TreeNode("Корневая категория");
            root.AddChildren(category1, category2, category3);

            return root;
        }

        /// <summary>
        /// Вспомогательный метод для тестирования поиска
        /// </summary>
        static void TestSearch(TreeNode root, string searchValue)
        {
            Console.Write($"Поиск '{searchValue}': ");
            TreeNode result = root.FindNode(searchValue);
            if (result != null)
            {
                Console.WriteLine($"Найден (значение: {result.Value})");
            }
            else
            {
                Console.WriteLine($"Не найден");
            }
        }
    }
}
