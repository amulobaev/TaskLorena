using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace TaskLorena
{
    public partial class Form1 : Form
    {
        Salon salon = new Salon();

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
                        salon.AddToSalon("Миасс", 4, -1, false, desc_text_1.Text);

                        // Амелия
                        command.CommandText = $"INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Амелия', 5, {miassId}, 1, @desc)"; // SQL-запрос для добавления записи о салоне "Амелия"
                        command.Parameters.AddWithValue("@desc", desc_text_2.Text); // Добавление значения из текстового поля "desc_text_2" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса
                        long ameliaId = connection.LastInsertRowId; // Получение идентификатора последней вставленной записи
                        salon.AddToSalon("Амелия", 5, Convert.ToInt32(miassId), true, desc_text_2.Text);

                        // Тест1
                        command.CommandText = $"INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Тест1', 2, {ameliaId}, 1, @desc)"; // SQL-запрос для добавления записи о салоне "Тест1"
                        command.Parameters.AddWithValue("@desc", desc_text_3.Text); // Добавление значения из текстового поля "desc_text_3" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса
                        salon.AddToSalon("Тест1", 2, Convert.ToInt32(ameliaId), true, desc_text_3.Text);

                        // Тест2
                        command.CommandText = $"INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Тест2', 0, {miassId}, 1, @desc)"; // SQL-запрос для добавления записи о салоне "Тест2"
                        command.Parameters.AddWithValue("@desc", desc_text_4.Text); // Добавление значения из текстового поля "desc_text_4" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса
                        salon.AddToSalon("Тест2", 0, Convert.ToInt32(miassId), true, desc_text_4.Text);

                        // Курган
                        command.CommandText = "INSERT INTO Salons (Name, Discount, ParentId, Dependency, Description) VALUES ('Курган', 11, NULL, 0, @desc)"; // SQL-запрос для добавления записи о салоне "Курган"
                        command.Parameters.AddWithValue("@desc", desc_text_5.Text); // Добавление значения из текстового поля "desc_text_5" в параметр запроса "@desc"
                        command.ExecuteNonQuery(); // Выполнение запроса
                        salon.AddToSalon("Курган", 11, -1, false, desc_text_5.Text);

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

        // Обработчик события клика по кнопке "Выполнить расчет стоимости"
        private void cost_calculation_Click(object sender, EventArgs e)
        {
            // Получаем в списки id родителя, скидки и зависимости 
            List<int> parentIds = salon.GetSalonParentIds();
            List<double> discounts = salon.GetSalonDiscounts();
            List<bool> dependency = salon.GetSalonDependency();

            // Получаем цену, введенную пользователем
            List<double> prices = GetPriceFromUser();

            List<double> calculatedPrices = new List<double>(); // список для хранения рассчитанных цен

            // Проходим по каждому салону в списке
            for (int i = 0; i < parentIds.Count; i++)
            {
                if (parentIds[i] != -1) // Если есть зависимость от предка
                {
                    // Вычисляем скидку родителя и рассчитываем цену после учета скидок
                    double discountParent = salon.CalculateDiscountParent(parentIds, discounts, dependency, i);
                    double calculatedPrice = salon.CalculatePrice(prices[i], discounts[i], discountParent);

                    // Добавляем рассчитанную цену в список calculatedPrices
                    calculatedPrices.Add(calculatedPrice);

                    // Сохраняем результат вычисления скидки в базе данных
                    salon.SaveCalculationResultToDatabase(parentIds[i], discounts[i], dependency[i], prices[i], discountParent, calculatedPrice);
                }
                else
                {
                    // Если салон не имеет родителя, скидка родителя равна 0
                    double discountParent = 0;

                    // Рассчитываем цену после учета скидки и добавляем ее в список calculatedPrices
                    double calculatedPrice = salon.CalculatePrice(prices[i], discounts[i], discountParent);
                    calculatedPrices.Add(calculatedPrice);

                    // Сохраняем результат вычисления скидки в базе данных
                    salon.SaveCalculationResultToDatabase(parentIds[i], discounts[i], dependency[i], prices[i], discountParent, calculatedPrice);
                }
            }

            MessageBox.Show("Расчет завершен. Результат сохранен в базе данных.");
        }

        // Метод возвращает список цен салонов (думаю, здесь без комментариев)
        public List<double> GetPriceFromUser()
        {
            List<double> prices = new List<double>();

            prices.Add(Convert.ToDouble(price_text_1.Text));
            prices.Add(Convert.ToDouble(price_text_2.Text));
            prices.Add(Convert.ToDouble(price_text_3.Text));
            prices.Add(Convert.ToDouble(price_text_4.Text));
            prices.Add(Convert.ToDouble(price_text_5.Text));

            return prices;
        }
    }
}
