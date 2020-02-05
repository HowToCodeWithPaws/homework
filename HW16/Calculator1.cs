using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW16
{
	public class Calculator
	{
		// Поле для события - уведомления об ошибке.
		public static event ErrorNotificationType ErrorNotification;

		/// <summary>
		/// Тип делегата, принимающего два вещественных числа 
		/// и возвращающего вещественное число.
		/// </summary>
		/// <param name="a"> Первое входное число. </param>
		/// <param name="b"> Второе входное число. </param>
		/// <returns></returns>
		public delegate double MathOperation(double a, double b);

		/// <summary>
		/// Словарь с ключами - символами операций и функциями - делегатами.
		/// </summary>
		public static Dictionary<String, MathOperation> operations;

		/// <summary>
		/// В конструкторе класса Calculator происходит инициализация 
		/// словаря операциями, записанными через лямбда-выражения.
		/// </summary>
		static Calculator()
		{
			operations = new Dictionary<string, MathOperation>();
			operations.Add("+", (x, y) => { return checked(x + y); });
			operations.Add("-", (x, y) => { return checked(x - y); });
			operations.Add("*", (x, y) => { return checked(x * y); });
			operations.Add("/", (x, y) => { return checked(x / y); });
			operations.Add("^", (x, y) => { return checked(Math.Pow(x, y)); });
		}

		/// <summary>
		/// Метод, в котором происходит парсинг выражений и вызов вычислений.
		/// </summary>
		/// <param name="expr"> Принимает на вход строку с выражением. </param>
		/// <returns> Возвращает вещественное число. </returns>
		public static double Calculate(string expr)
		{
			string[] args = expr.Split(' ');

			// Парсинг с проверкой на переполнение.
			// Обработка исключений с помощью оператора try-catch,
			// вызываюшего событие.
			try
			{
				double operandA = double.Parse(args[0]);
				string operationExp = args[1];
				double operandB = double.Parse(args[2]);

				// Выбрасываем свое исключение для делания на 0.
				if (operationExp == "/" && operandB == 0)
				{
					throw new Exception("Произошла попытка деления на 0.");
				}

				// Выбрасываем свое исключение для случая, когда результат - не число.
				double result = Math.Round(operations[operationExp](operandA, operandB), 3);
				if (double.IsNaN(result))
				{
					throw new Exception("Выражение не является числом.");
				}

				return result;
			}
			catch (Exception e) { ErrorNotification(e.Message); throw; }
		}
	}
}
