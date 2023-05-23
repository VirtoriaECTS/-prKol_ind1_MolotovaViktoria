using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SubjectIndexForm
{
	public class Slovar
	{
		// словарь для хранения слов и соответствующих страниц
		private Dictionary<string, ArrayList> index;

		public Slovar() { index = new Dictionary<string, ArrayList>(); }

		// метод для добавления
		public void AddEntry(string word, int[] pageNumbers)
		{
			if (index.ContainsKey(word)) 
			{
				ArrayList pages = index[word];
				foreach (int pageNumber in pageNumbers)
				{
					if (!pages.Contains(pageNumber)) { pages.Add(pageNumber); }
				}
			}
			else
			{
				ArrayList pages = new ArrayList();
				pages.AddRange(pageNumbers);
				index.Add(word, pages);
			}
		}

		// метод для удаления слова
		public void RemoveEntry(string word) { index.Remove(word); }

		// загрузка из файла
		public void LoadFromFile(string fileName)
		{
			try
			{
				if (File.Exists(fileName))
				{
					string[] lines = File.ReadAllLines(fileName);
					foreach (string line in lines)
					{
						bool key = false;

						string[] parts = line.Split(':');
						string word = parts[0];
						string[] pageNumbersString = parts[1].Split(',');
						ArrayList pageNumbers = new ArrayList();
						foreach (string pageNumberString in pageNumbersString)
						{
							if (int.TryParse(pageNumberString, out int pageNumber)) { pageNumbers.Add(pageNumber); }
						}

						int prevNumber = int.MinValue;
						foreach (int number in pageNumbers)
						{
							if (number <= prevNumber)
							{
								MessageBox.Show($"Ошибка в слове '{word}'. Числа должны быть по возрастанию и не повторяться. (Слово не загрузилось)",
									"Внимание!");
								key = true;
								continue;
							}
							prevNumber = number;
						}
						if (pageNumbers.Count > 10) { MessageBox.Show($"Ошибка в слове '{word}'. " +
																	  $"Количество номеров страниц, относящихся к одному слову, — от одного до десяти. " +
																	  $"(Слово не загрузилось)", "Внимание!"); continue; }
						if (key == true) continue;
						index.Add(word, pageNumbers);
					}
				}
				else { MessageBox.Show("Файл не найден.", "Ошибка"); }
			} 
			catch (Exception ex) { MessageBox.Show($"{ex}", "Ошибка"); }
		}

		// сохранение в файл
		public void SaveToFile(string fileName)
		{
			StringBuilder sb = new StringBuilder();
			foreach (KeyValuePair<string, ArrayList> entry in index)
			{
				sb.Append(entry.Key + ": ");
				sb.Append(string.Join(", ", entry.Value.ToArray()));
				sb.AppendLine();
			}
			File.WriteAllText(fileName, sb.ToString());
		}
		
		// нахождение слова
		public bool Contains(string word) { return index.ContainsKey(word); }

		// отображение в listBox
		public void FillListBox(ListBox listBox)
		{
			listBox.Items.Clear();
			foreach (string word in GetWords())
			{
				string pageNumbersString = string.Join(", ", GetPageNumbers(word).ToArray());
				string itemString = $"Слово '{word}' втречается на стр.: {pageNumbersString}";
				listBox.Items.Add(itemString);
			}
		}

		// метод для вывода страниц
		public ArrayList GetPageNumbers(string word)
		{
			ArrayList pages = new ArrayList();
			if (index.ContainsKey(word)) { pages = index[word]; }
			return pages;
		}

		// метод для вывода слов
		private List<string> GetWords()
		{
			List<string> words = index.Keys.ToList();
			return words;
		}

		// отображение текущего указателя в textBox
		public void GetStringIndex(TextBox textBox, string word)
		{
			textBox.Text = null;
			string pageNumbersString = string.Join(", ", GetPageNumbers(word).ToArray());
			string itemString = $"{word}: {pageNumbersString}";
			textBox.Text = itemString;
		}
	}
}
