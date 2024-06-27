using System.Collections.Generic;
using System.Data.SQLite;

namespace TaskLorena
{
    public class Salon
    {
        public string Name;
        public double Discount;
        public int ParentId;
        public string Description;
        public bool Dependency;

        public List<Salon> salons = new List<Salon>();

        // Метод добавляет салон в список "salons" с указанными параметрами
        public void AddToSalon(string name, double discount, int parentId, bool dependency, string description)
        {
            salons.Add(new Salon { Name = name, Discount = discount, ParentId = parentId, Dependency = dependency, Description = description }); // Создаем новый объект типа Salon с указанными параметрами
        }

        // Метод возвращает список идентификаторов родительских салонов
        public List<int> GetSalonParentIds()
        {
            List<int> parentIds = new List<int>();

            for (int i = 0; i < salons.Count; i++)
            {
                parentIds.Add(salons[i].ParentId);
            }

            return parentIds;
        }

        // Метод возвращает список скидок салонов (действия идентичны методу, подробнее описанному выше)
        public List<double> GetSalonDiscounts()
        {
            List<double> discounts = new List<double>();

            for (int i = 0; i < salons.Count; i++)
            {
                discounts.Add(salons[i].Discount);
            }

            return discounts;
        }

        // Метод возвращает список зависимостей салонов (действия идентичны методу, подробнее описанному выше)
        public List<bool> GetSalonDependency()
        {
            List<bool> dependency = new List<bool>();

            for (int i = 0; i < salons.Count; i++)
            {
                dependency.Add(salons[i].Dependency);
            }

            return dependency;
        }

        // Метод вычисляет скидку родителя для салона с указанным индексом
        public double CalculateDiscountParent(List<int> parentIds, List<double> discounts, List<bool> dependency, int currentIndex)
        {
            // Проверяем, является ли currentIndex корректным и есть ли зависимость для текущего салона
            if (currentIndex < 0 || currentIndex >= parentIds.Count || !dependency[currentIndex])
            {
                return 0; // Возвращаем 0, если индекс выходит за пределы списка или нет зависимости
            }

            // Получаем идентификатор текущего салона и его индекс в списке parentIds
            int parentId = parentIds[currentIndex];
            int parentIndex = parentIds.IndexOf(parentId) - 1;

            // Проверяем, есть ли предок для текущего салона
            if (parentIndex == -1)
            {
                return 0; // Если предок не найден, чтобы избежать бесконечной рекурсии
            }

            // Вычисляем скидку родителя, используя рекурсию
            return discounts[parentIndex] + CalculateDiscountParent(parentIds, discounts, dependency, parentIndex);
        }

        // Метод вычисляет цену по формуле, с учетом скидок
        public double CalculatePrice(double price, double discount, double discountParent)
        {
            return price - (price * ((discount + discountParent) / 100));
        }

        // Метод сохраняет результат вычисления скидки в новую таблицу
        public void SaveCalculationResultToDatabase(int parentId, double discount, bool hasDependency, double price, double discountParent, double calculatedPrice)
        {
            string connectionString = "Data Source=MyDatabase.sqlite;Version=3;"; // Указание подключения к SQLite

            using (SQLiteConnection connection = new SQLiteConnection(connectionString)) // Создание подключения к базе данных
            {
                using (SQLiteCommand command = new SQLiteCommand(connection)) // Создание команды для выполнения SQL-запросов
                {
                    connection.Open(); // Открытие подключения

                    // Создаем таблицу Results, если ее нет в базе данных
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Results (ParentId INTEGER, Discount REAL, HasDependency TEXT, Price REAL, DiscountParent REAL, CalculatedPrice REAL)";
                    command.ExecuteNonQuery();

                    // SQL-запрос для вставки результатов вычисления скидки в таблицу Results
                    command.CommandText = "INSERT INTO Results (ParentId, Discount, HasDependency, Price, DiscountParent, CalculatedPrice) " +
                                         "VALUES (@ParentId, @Discount, @HasDependency, @Price, @DiscountParent, @CalculatedPrice)";

                    // Добавляем значения параметров в команду
                    command.Parameters.AddWithValue("@ParentId", parentId);
                    command.Parameters.AddWithValue("@Discount", discount);
                    command.Parameters.AddWithValue("@HasDependency", hasDependency);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@DiscountParent", discountParent);
                    command.Parameters.AddWithValue("@CalculatedPrice", calculatedPrice);

                    // Выполнение запроса вставки результатов
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
