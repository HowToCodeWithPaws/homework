using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HW16
{
	/// <summary>
	/// Созданный тип делегата, относящегося к уведомлении об ошибках.
	/// </summary>
	/// <param name="message"> Принимает на вход строку - сообщение. </param>
	public delegate void ErrorNotificationType(string message);

	/// <summary>
	/// Класс калькулятора с методом преобразования выражения 
	/// и вызова делегата.
	/// </summary>
	
	class Program
	{
		/// <summary>
		/// StringBuilder для сохранения ответов.
		/// </summary>
		public static StringBuilder answers = new StringBuilder();

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

				// Вызов метода Calculate с дописыванием результата его работы в StringBuilder.
				foreach (string expr in expressions)
					try
					{
						answers.Append($"{Calculator.Calculate(expr):f3}\n");
					}
					catch (Exception)
					{
						// Это странный костыль, но по-другому как-то не работает.
						continue;
					}

				File.WriteAllText("../../../answers.txt", answers + Environment.NewLine);

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
		public static string Checker(string left, string right,
			ref int errors)
		{
			if (left == right) { return "OK"; }
			else
			{
				errors++;
				Console.WriteLine($"Ошибка:{left} != {right}");
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

				StringBuilder results = new StringBuilder();

				for (int i = 0; i < expressions.Length; i++)
				{
					results.Append($"{Checker(expressions[i], answers[i], ref errors)}\n");
				}

				File.WriteAllText("../../../results.txt",
					results + Environment.NewLine + $"Ошибок: {errors}");

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
			catch (Exception)
			{
				Console.WriteLine("что-то пошло не так");
			}
		}

		/// <summary>
		/// Метод, выводящий на консоль сообщение об исключительной
		/// ситуации, включая время поимки исключения.
		/// </summary>
		/// <param name="message"> Принимает на вход строку с сообщением. </param>
		public static void ConsoleErrorHandler(string message)
		{
			Console.WriteLine(message + " " + DateTime.Now);
		}

		/// <summary>
		/// Метод реагирования на событие, добавляет сообщение в String Builder.
		/// Альтернативно можно было бы сразу писать в файл или в обычную строку.
		/// </summary>
		/// <param name="message"> Принимает на вход строку с сообщением исключений, 
		/// по которой дифференцирует обработку. </param>
		public static void ResultErrorHandler(string message)
		{
			switch (message)
			{
				case "Выражение не является числом.":
					answers.Append("не число\n");
					return;

				case "Значение было недопустимо малым или " +
					"недопустимо большим для Double.":
					answers.Append("∞\n");
					return;

				case "Данный ключ отсутствует в словаре.":
					answers.Append("неверный оператор\n");
					return;

				case "Произошла попытка деления на 0.":
					answers.Append("bruh\n");
					return;
			}
		}

		/// <summary>
		/// Метод с повтором решения и вызовом двух методов для 
		/// двух частей задания.
		/// Подписка методов на событие.
		/// Удивительно, но все работает.
		/// </summary>
		static void Main()
		{
			do
			{
				// Подписка методов на событие.
				Calculator.ErrorNotification += ConsoleErrorHandler;
				Calculator.ErrorNotification += ResultErrorHandler;

				FirstTask();

				SecondTask();

				Console.WriteLine("\nДля повторного решения нажмите Enter, " +
					"для выхода нажмите любой другой символ");

			} while (Console.ReadKey().Key == ConsoleKey.Enter);
		}

	}
}
