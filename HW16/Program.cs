using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW16
{
	/// <summary>
	/// Класс калькулятора с методом преобразования выражения 
	/// и вызова делегата.
	/// </summary>
	public class Calculator
	{
		public static double Calculate(string expr)
		{
			string[] args = expr.Split(' ');

			try
			{
				double operandA = double.Parse(args[0]);
				string operationExp = args[1];
				double operandB = double.Parse(args[2]);

				return Program.operations[operationExp](operandA, operandB);
			}
			catch (Exception) { throw; }
		}
	}

	class Program
	{
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
		/// В конструкторе класса Program происходит инициализация 
		/// словаря операциями, записанными через лямбда-выражения.
		/// </summary>
		static Program()
		{
			operations = new Dictionary<string, MathOperation>();
			operations.Add("+", (x, y) => { return x + y; });
			operations.Add("-", (x, y) => { return x - y; });
			operations.Add("*", (x, y) => { return x * y; });
			operations.Add("/", (x, y) => { return x / y; });
			operations.Add("^", (x, y) => { return Math.Pow(x, y); });
		}

		/// <summary>
		/// Метод, считывающий выражения из файла, вызывающий метод
		/// Calculate, вычисляющий их значения, и записывающий 
		/// результаты в файл.
		/// </summary>
		public static void FirstTask()
		{
			Console.WriteLine("running the first task");

			// Блок try catch для обработки файловых исключений.
			try
			{
				string[] expressions =
					File.ReadAllLines("../../../expressions.txt");

				File.WriteAllText("../../../answers.txt", "");

				foreach (string expr in expressions)
					try
					{
						File.AppendAllText("../../../answers.txt",
							$"{Calculator.Calculate(expr):f3}\n");
					}
					catch (Exception)
					{
						Console.WriteLine("Неверный формат данных");
					}

				Console.WriteLine("first task finished");
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("Файл не существует"
					+ Environment.NewLine + e);
			}
			catch (IOException e)
			{
				Console.WriteLine("Ошибка ввода/вывода"
					+ Environment.NewLine + e);
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("Ошибка доступа: у вас нет" +
					" разрешения на создание файла"
					+ Environment.NewLine + e);
			}
			catch (System.Security.SecurityException e)
			{
				Console.WriteLine("Ошибка безопасности"
					+ Environment.NewLine + e);
			}
		}

		/// <summary>
		/// Метод для сравнения результатов вычислений с 
		/// величинами из файла.
		/// </summary>
		/// <param name="left"> Значения из файла для
		/// проверки. </param>
		/// <param name="right"> Значения, вычисленные в 
		/// первом пункте. </param>
		/// <param name="errors"> Количество ошибок. </param>
		/// <returns> Метод возвращает строку с результатом 
		/// проверки. </returns>
		public static string Checker(double left, double right,
			ref int errors)
		{
			if (left == right) { return "OK"; }
			else
			{
				errors++;
			}
			return "Error";
		}

		/// <summary>
		/// Метод для второй части работы - проверки результатов.
		/// </summary>
		public static void SecondTask()
		{
			Console.WriteLine("running the second task");

			/// Блок try catch обрабатывает возможные 
			/// исключения, возникающие при работе с файлами.
			try
			{
				int errors = 0;
				string[] expressions = 
					File.ReadAllLines("../../../expressions_checker.txt");
				string[] answers = 
					File.ReadAllLines("../../../answers.txt");

				File.WriteAllText("../../../results.txt", "");
				try
				{
					for (int i = 0; i < expressions.Length; i++)
					{

						double left = double.Parse(expressions[i]);
						double right = double.Parse(answers[i]);

						File.AppendAllText("../../../results.txt",
							$"{Checker(left, right, ref errors)}\n");
					}

					File.AppendAllText("../../../results.txt",
					$"Ошибок: {errors}");
				}
				catch (Exception)
				{
					Console.WriteLine("Неверный формат данных");
				}

				Console.WriteLine("second task finished");
			}
			catch (FileNotFoundException e)
			{
				Console.WriteLine("Файл не существует"
					+ Environment.NewLine + e);
			}
			catch (IOException e)
			{
				Console.WriteLine("Ошибка ввода/вывода"
					+ Environment.NewLine + e);
			}
			catch (UnauthorizedAccessException e)
			{
				Console.WriteLine("Ошибка доступа: у вас нет " +
					"разрешения на создание файла"
					+ Environment.NewLine + e);
			}
			catch (System.Security.SecurityException e)
			{
				Console.WriteLine("Ошибка безопасности"
					+ Environment.NewLine + e);
			}
		}

		/// <summary>
		/// Метод с повтором решения и вызовом двух методов для 
		/// двух частей задания.
		/// </summary>
		static void Main()
		{
			do
			{
				FirstTask();

				SecondTask();

				Console.WriteLine("\nДля повторного решения нажмите Enter, " +
					"для выхода нажмите любой другой символ");

			} while (Console.ReadKey().Key == ConsoleKey.Enter);
		}

	}
}
