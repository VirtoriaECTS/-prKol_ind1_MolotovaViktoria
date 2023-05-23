using System;
using System.Collections;
using System.Linq;

namespace ConsoleApp1
{
	class Program
	{
		static Hashtable catalog = new Hashtable();

		static void Main(string[] args)
		{
			bool exit = false;

			while (!exit)
			{
				
				Console.WriteLine("\nВыберите действие:\n" +
								  "1. Добавить диск\n" +
								  "2. Удалить диск\n" +
								  "3. Добавить песню на диск\n" +
								  "4. Удалить песню с диска\n" +
								  "5. Просмотреть содержимое каталога\n" +
								  "6. Просмотреть содержимое диска\n" +
								  "7. Поиск всех записей заданного исполнителя\n" +
								  "8. Выйти.");

				string choise = Console.ReadLine();

				switch (choise)
				{
					case "1": AddDisk(); break;
					case "2": DeleteDisk(); break;
					case "3": AddSong(); break;
					case "4": DeleteSong(); break;
					case "5": DisplayCatalog(); break;
					case "6": DisplayDisk(); break;
					case "7": SearchByArtist(); break;
					case "8": exit = true; break;
					default: 
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.Clear(); Console.WriteLine("Некорректный ввод. Попробуйте снова.");
						Console.ForegroundColor = ConsoleColor.White;
					} break;
				}
				
			}
		}

		private static void SearchByArtist()
		{
			Console.Clear();
			if (catalog.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Каталог пуст.");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				Console.Write("Введите имя исполнителя для поиска записей: ");
				string artistName = Console.ReadLine();
				bool found = false;
				int i = 0;

				foreach (ArrayList songs in catalog.Values)
				{
			
					foreach (string songName in songs)
					{
						if (songName.Contains(artistName))
						{
							Console.WriteLine($"- {songName}");
							found = true; i++;
						}
					}
				}

				if (!found)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Записи исполнителя не найдены.");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine($"Найдены записи в количестве: {i}");
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
		}

		private static void DisplayDisk()
		{
			Console.Clear();
			if (catalog.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Каталог пуст.");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				Console.Write("Введите название диска для просмотра его содержимого: ");
				string diskName = Console.ReadLine();

				if (catalog.ContainsKey(diskName))
				{
					ArrayList songs = (ArrayList)catalog[diskName];

					if (songs.Count == 0)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"Содержимое диска `{diskName}` пустое.");
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.DarkYellow;
						Console.WriteLine($"Содержимое диска `{diskName}`: ");
						Console.ForegroundColor = ConsoleColor.White;

						int i = 1;
						foreach (string songName in songs) { Console.WriteLine($"{i}: {songName}"); i++; }
					}
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Диска с таким именем не существует.");
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
		}

		private static void DisplayCatalog()
		{
			Console.Clear();

			if (catalog.Keys.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Каталог пуст.");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else 
			{
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine("Содержимое каталога:");
				Console.ForegroundColor = ConsoleColor.White;

				int i = 1;
				foreach (string diskName in catalog.Keys) { Console.WriteLine($"{i}: {diskName}"); i++; }
			}
		}

		private static void DeleteSong()
		{
			Console.Clear();
			if (catalog.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Каталог пуст.");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				Console.Write("Введите название диска, с которого нужно удалить песню: ");
				string diskName = Console.ReadLine();

				if (catalog.Contains(diskName))
				{
					ArrayList songs = (ArrayList)catalog[diskName];

					if (songs.Count == 0)
					{
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine($"Диск `{diskName}` уже пустой.");
						Console.ForegroundColor = ConsoleColor.White;
					}
					else
					{
						Console.Write("Введите название песни: ");
						string songName = Console.ReadLine();

						if (songs.Contains(songName))
						{
							songs.Remove(songName);
							Console.ForegroundColor = ConsoleColor.DarkYellow;
							Console.WriteLine("Песня была удалена!");
							Console.ForegroundColor = ConsoleColor.White;
						}
						else
						{
							Console.ForegroundColor = ConsoleColor.Red;
							Console.WriteLine($"Песня с таким именем не существует на диске `{diskName}`.");
							Console.ForegroundColor = ConsoleColor.White;
						}
					}
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Диска с таким именем не существует.");
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
		}

		private static void AddSong()
		{
			Console.Clear();
			Console.Write("Введите название диска, на котором нужно добавить песню: ");
			string diskName = Console.ReadLine();

			if (catalog.ContainsKey(diskName))
			{
				ArrayList songs = (ArrayList)catalog[diskName];

				Console.Write("Введите название песни: ");
				string songName = Console.ReadLine();

				if (songs.Contains(songName)) 
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Песня с таким именем уже существует на данном диске.");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					songs.Add(songName);
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine("Песня была добавлена!");
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Диска с таким именем не существует.");
				Console.ForegroundColor = ConsoleColor.White;
			}
		}

		private static void DeleteDisk()
		{
			Console.Clear();
			if (catalog.Count == 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Каталог пуст.");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				Console.Write("Введите название диска для удаления: ");
				string diskName = Console.ReadLine();

				if (catalog.ContainsKey(diskName))
				{
					catalog.Remove(diskName);
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine("Диск был удален!");
					Console.ForegroundColor = ConsoleColor.White;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Диска с таким именем не существует.");
					Console.ForegroundColor = ConsoleColor.White;
				}
			}
		}

		private static void AddDisk()
		{
			Console.Clear();
			Console.Write("Введите название диска: ");
			string diskName = Console.ReadLine();

			if (catalog.ContainsKey(diskName))
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Диск с таким именем уже существует.");
				Console.ForegroundColor = ConsoleColor.White;
			}
			else
			{
				catalog[diskName] = new ArrayList();
				Console.ForegroundColor = ConsoleColor.DarkYellow;
				Console.WriteLine("Диск был добавлен!");
				Console.ForegroundColor = ConsoleColor.White;
			}
		}
	}
}
