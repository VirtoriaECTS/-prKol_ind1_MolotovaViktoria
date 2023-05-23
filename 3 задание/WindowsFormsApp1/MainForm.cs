using System;
using System.Windows.Forms;

namespace SubjectIndexForm
{
	public partial class MainForm : Form
	{
		private Slovar index;
		private readonly string LoadFile = "file1.txt";
		private readonly string SaveFile = "file2.txt";

		public MainForm()
		{
			InitializeComponent();
			index = new Slovar();
		}

		private void addButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(wordTextBox.Text) || string.IsNullOrEmpty(pageNumbersTextBox.Text)) 
				{ MessageBox.Show("Заполните все поля.", "Внимание!"); return; }

			string word = wordTextBox.Text.Trim();
			string[] pageNumberStrings = pageNumbersTextBox.Text.Split(new char[] { ',', ';' }, 
				StringSplitOptions.RemoveEmptyEntries);
			int[] pageNumbers = new int[pageNumberStrings.Length];
			for (int i = 0; i < pageNumberStrings.Length; i++)
			{
				if (!int.TryParse(pageNumberStrings[i], out pageNumbers[i]))
				{
					MessageBox.Show("Неверная строка чисел. Пример заполнения: '1, 2, 3, 4'.", "Ошибка.");
					return;
				}
			}

			int prevNumber = int.MinValue;
			foreach (int number in pageNumbers)
			{
				if (number <= prevNumber)
				{
					MessageBox.Show("Числа должны быть по возрастанию и не повторяться.", "Ошибка.");
					return;
				}
				prevNumber = number;
			}

			if (pageNumbers.Length > 10) { MessageBox.Show("Количество номеров страниц, относящихся к одному слову," +
														   " — от одного до десяти.", "Ошибка."); return; }

			index.AddEntry(word, pageNumbers);
			indexTextBox.Text = "";
			wordTextBox.Text = "";
			pageNumbersTextBox.Text = "";
			index.GetStringIndex(indexTextBox, word);
			index.FillListBox(listBoxOfWords);
		}

		private void removeButton_Click(object sender, EventArgs e)
		{
			string word = wordTextBox.Text.Trim();
			if (string.IsNullOrEmpty(word)) { MessageBox.Show("Для удаления слова, впишите его в поле 'Слово'.",
				"Внимание!"); return; }
			if (!index.Contains(word)) { MessageBox.Show($"Слово '{word}' не загружно. Его не удастся удалить.",
				"Внимание!"); return; }
			index.RemoveEntry(word);
			indexTextBox.Text = null;
			index.FillListBox(listBoxOfWords);
		}

		private void searchWord_Click(object sender, EventArgs e)
		{
			string word = wordTextBox.Text.Trim();
			if (index.Contains(word)) 
			{
				string pageNumbersString = string.Join(", ", index.GetPageNumbers(word).ToArray());
				MessageBox.Show($"Слово '{word}' имеется. Оно встречается на страницах: {pageNumbersString}",
					"Результат поиска.");
			} 
			else MessageBox.Show($"Слово '{word}' не имеется.", "Результат поиска.");
		}

		private void loadFromFile_Click(object sender, EventArgs e)
		{
			index.LoadFromFile(LoadFile);
			index.FillListBox(listBoxOfWords);
			MessageBox.Show($"Слова были загружены из файла: '{LoadFile}'.", "Внимание!");
		}

		private void saveToFile_Click(object sender, EventArgs e)
		{
			index.SaveToFile(SaveFile);
			index.FillListBox(listBoxOfWords);
			MessageBox.Show($"Слова были сохранены в файл: '{SaveFile}'.", "Внимание!");
		}
	}
}
