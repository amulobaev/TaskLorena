using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace TaskLorena
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Создание таблицы "Salons" при клике на кнопку "create_table"
        private void create_table_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=MyDatabase.sqlite;Version=3;"; // Указание подключения к SQLite

            using (var connection = new SQLiteConnection(connectionString)) // Создание подключения к базе данных
            {
                connection.Open(); // Открытие подключения

                using (var command = new SQLiteCommand(connection)) // Создание команды для выполнения SQL-запросов
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Salons (Id INTEGER PRIMARY KEY, Name TEXT, Discount REAL, ParentId INTEGER, Dependency INTEGER CHECK(Dependency IN (0, 1)), Description TEXT CHECK(length(Name) <= 124))"; // Создание таблицы "Salons" с указанными полями и ограничениями
                    command.ExecuteNonQuery(); // Выполнение команды создания таблицы

                    // Проверяем, есть ли уже записи в таблице
                    command.CommandText = "SELECT COUNT(*) FROM Salons"; // SQL-запрос для подсчета количества записей в таблице "Salons"
                    int count = Convert.ToInt32(command.ExecuteScalar()); // Получение первого значения из результата запроса (количество записей)

                    if (count == 0)
                    {
                        // Добавляем базовые наборы данных

                        // Миасс
                        command.CommandText = "INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Миасс', 4, NULL, 0, @desc)"; // SQL-запрос для добавления записи о салоне "Миасс"
                        command.Parameters.AddWithValue("@desc", desc_text_1.Text); // Добавление значения из текстового поля "desc_text_1" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса
                        long miassId = connection.LastInsertRowId; // Получение идентификатора последней вставленной записи

                        // Амелия
                        command.CommandText = $"INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Амелия', 5, {miassId}, 1, @desc)"; // SQL-запрос для добавления записи о салоне "Амелия"
                        command.Parameters.AddWithValue("@desc", desc_text_2.Text); // Добавление значения из текстового поля "desc_text_2" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса
                        long ameliaId = connection.LastInsertRowId; // Получение идентификатора последней вставленной записи

                        // Тест1
                        command.CommandText = $"INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Тест1', 2, {ameliaId}, 1, @desc)"; // SQL-запрос для добавления записи о салоне "Тест1"
                        command.Parameters.AddWithValue("@desc", desc_text_3.Text); // Добавление значения из текстового поля "desc_text_3" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса

                        // Тест2
                        command.CommandText = $"INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Тест2', 0, {miassId}, 1, @desc)"; // SQL-запрос для добавления записи о салоне "Тест2"
                        command.Parameters.AddWithValue("@desc", desc_text_4.Text); // Добавление значения из текстового поля "desc_text_4" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса

                        // Курган
                        command.CommandText = "INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Курган', 11, NULL, 0, @desc)"; // SQL-запрос для добавления записи о салоне "Курган"
                        command.Parameters.AddWithValue("@desc", desc_text_5.Text); // Добавление значения из текстового поля "desc_text_5" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса

                        connection.Close(); // Закрытие подключения к базе данных

                        MessageBox.Show("Таблица Salons создана!");
                    }
                    else
                    {
                        // Показываем диалоговое окно с вопросом
                        DialogResult result = MessageBox.Show("Таблица Salons уже существвует! Удалить таблицу?", "Подтверждение удаления", MessageBoxButtons.YesNo);

                        // Проверяем, как ответил пользователь на вопрос о удалении таблицы
                        if (result == DialogResult.Yes)
                        {
                            // Если пользователь подтвердил удаление таблицы, выполняем соответствующий SQL-запрос
                            command.CommandText = "DROP TABLE Salons"; // SQL-запрос для удаления таблицы "Salons"
                            command.ExecuteNonQuery(); // Выполнение запроса удаления таблицы
                            MessageBox.Show("Таблица Salons удалена! Создайте новую, чтобы продолжить работу!");
                        }
                        else
                        {
                            // Если пользователь отменил удаление таблицы, закрываем окно приложения
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
