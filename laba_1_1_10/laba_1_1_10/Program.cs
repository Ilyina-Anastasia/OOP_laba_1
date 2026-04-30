using System;
using System.Text;

namespace laba_1_1_10
{
    public sealed class Polynomial
    {
        private readonly double[] _coefficients;
        private const double Epsilon = 1e-10;

        // Автоматическое свойство только для чтения
        public int Degree { get; }

        // Конструктор с параметрами
        public Polynomial(params double[] coefficients)
        {
            if (coefficients == null || coefficients.Length == 0)
            {
                _coefficients = new[] { 0.0 };
                Degree = 0;
                return;
            }

            // Убираем незначащие нулевые коэффициенты при старших степенях
            int effectiveDegree = coefficients.Length - 1;
            while (effectiveDegree > 0 && Math.Abs(coefficients[effectiveDegree]) < Epsilon)
            {
                effectiveDegree--;
            }

            _coefficients = new double[effectiveDegree + 1];
            Array.Copy(coefficients, _coefficients, effectiveDegree + 1);
            Degree = effectiveDegree;
        }

        // Индексатор для доступа к коэффициентам
        public double this[int index]
        {
            get => (index < 0 || index > Degree) ? 0 : _coefficients[index];
        }

        // Метод вычисления значения 
        public double Evaluate(double x)
        {
            double result = 0;
            for (int i = Degree; i >= 0; i--)
            {
                result = result * x + _coefficients[i];
            }
            return result;
        }

        #region Арифметические операции (Перегрузка операторов)

        public static Polynomial operator +(Polynomial left, Polynomial right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            int maxDegree = Math.Max(left.Degree, right.Degree);
            double[] result = new double[maxDegree + 1];
            for (int i = 0; i <= maxDegree; i++)
                result[i] = left[i] + right[i];
            return new Polynomial(result);
        }

        public static Polynomial operator -(Polynomial left, Polynomial right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            int maxDegree = Math.Max(left.Degree, right.Degree);
            double[] result = new double[maxDegree + 1];
            for (int i = 0; i <= maxDegree; i++)
                result[i] = left[i] - right[i];
            return new Polynomial(result);
        }

        public static Polynomial operator *(Polynomial left, Polynomial right)
        {
            if (left is null) throw new ArgumentNullException(nameof(left));
            if (right is null) throw new ArgumentNullException(nameof(right));

            double[] result = new double[left.Degree + right.Degree + 1];
            for (int i = 0; i <= left.Degree; i++)
                for (int j = 0; j <= right.Degree; j++)
                    result[i + j] += left[i] * right[j];
            return new Polynomial(result);
        }

        public static bool operator ==(Polynomial left, Polynomial right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
                return true;

            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return false;

            if (left.Degree != right.Degree)
                return false;

            for (int i = 0; i <= left.Degree; i++)
            {
                if (Math.Abs(left._coefficients[i] - right._coefficients[i]) > Epsilon)
                    return false;
            }
            return true;
        }

        // Перегрузка оператора неравенства (должен быть вместе с ==)
        public static bool operator !=(Polynomial left, Polynomial right)
        {
            return !(left == right);
        }

        #endregion

        #region Сравнение

        public override bool Equals(object obj)
        {
            return this == (obj as Polynomial);
        }

        // Переопределение ToString 
        public override string ToString()
        {
            if (Degree == 0 && Math.Abs(_coefficients[0]) < Epsilon) return "0";

            StringBuilder sb = new StringBuilder();
            for (int i = Degree; i >= 0; i--)
            {
                double coeff = _coefficients[i];
                if (Math.Abs(coeff) < Epsilon) continue;

                // Обработка знака
                if (coeff > 0 && sb.Length > 0)
                    sb.Append(" + ");
                else if (coeff < 0)
                    sb.Append(sb.Length > 0 ? " - " : "-");

                double absCoeff = Math.Abs(coeff);

                // Вывод коэффициента
                bool isOne = Math.Abs(absCoeff - 1) < Epsilon;
                if (!isOne || i == 0)
                    sb.Append(absCoeff);

                // Вывод переменной x и степени
                if (i > 0)
                {
                    sb.Append("x");
                    if (i > 1)
                        sb.Append($"^{i}");
                }
            }
            return sb.ToString();
        }

        #endregion
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ОТЧЕТ ПО РАБОТЕ КЛАССА POLYNOMIAL ===\n");

            var p1 = new Polynomial(-1, 3, 2);     
            var p2 = new Polynomial(-4, 0, 0, 1);
            var p3 = new Polynomial(-1, 3, 2);
            var p4 = new Polynomial(0, 0, 0);      
            var p5 = new Polynomial(5);           
            var p6 = new Polynomial(2, 3, -1);

            Console.WriteLine("1. Созданы объекты:");
            Console.WriteLine($"   P1 (коэфф: -1, 3, 2) -> {p1}");
            Console.WriteLine($"   P2 (коэфф: -4, 0, 0, 1) -> {p2}");
            Console.WriteLine($"   P3 (коэфф: -1, 3, 2) -> {p3}");
            Console.WriteLine($"   P4 (нулевой) -> {p4}");
            Console.WriteLine($"   P5 (константа) -> {p5}");
            Console.WriteLine($"   P6 (коэфф: 2, 3, -1) -> {p6}");

            Console.WriteLine($"\n2. Свойства (Только для чтения):");
            Console.WriteLine($"   Степень P1: {p1.Degree}");
            Console.WriteLine($"   Степень P2: {p2.Degree}");

            double x = 2.0;
            Console.WriteLine($"\n3. Метод Evaluate (x = {x}):");
            Console.WriteLine($"   Значение P1({x}) = {p1.Evaluate(x)}");
            Console.WriteLine($"   Значение P2({x}) = {p2.Evaluate(x)}");

            Console.WriteLine($"\n4. Математические операции:");
            Console.WriteLine($"   P1 + P2 = {p1 + p2}");
            Console.WriteLine($"   P2 - P1 = {p2 - p1}");
            Console.WriteLine($"   P1 * P2 = {p1 * p2}");
            
            Console.WriteLine($"\n5. Проверка операторов сравнения:");
            Console.WriteLine($"   P1 == P3? {p1 == p3}");
            Console.WriteLine($"   P1 != P3? {p1 != p3}");
            Console.WriteLine($"   P1 == P2? {p1 == p2}");
            Console.WriteLine($"   P1 != P2? {p1 != p2}");
            Console.WriteLine($"   P1 == P6? {p1 == p6}");
            Console.WriteLine($"   P1 == null? {p1 == null}");
            Console.WriteLine($"   null == null? {null == null}");

            Console.WriteLine($"\n   Проверка Equals:");
            Console.WriteLine($"   P1.Equals(P3) = {p1.Equals(p3)}");
            Console.WriteLine($"   P1.Equals(P2) = {p1.Equals(p2)}");

            Console.WriteLine($"\n6. Граничные случаи:");
            Console.WriteLine($"   Нулевой многочлен: {p4}");
            Console.WriteLine($"   Константа: {p5}");
            Console.WriteLine($"   P5 + P4 = {p5 + p4}");
            Console.WriteLine($"   P5 * P1 = {p5 * p1}");

            Console.WriteLine($"\n7. Демонстрация неизменяемости:");
            Console.WriteLine($"   P1 до сложения: {p1}");
            var dummy = p1 + p2;
            Console.WriteLine($"   P1 после сложения (не изменился): {p1}");
            Console.WriteLine($"   Новый объект (результат сложения): {dummy}");

            Console.WriteLine($"   P2 после операций (не изменился): {p2}");

            Console.WriteLine("\n=== КОНЕЦ ВЫВОДА ===");
            Console.ReadKey();
        }
    }
}