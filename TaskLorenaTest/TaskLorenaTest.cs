using TaskLorena;

namespace TaskLorenaTest
{
    public class Tests
    {
        // Тестовый метод для проверки корректности вычисления цены для салона "Миасс"
        [Test]
        public void CalculatePriceMiass()
        {
            // Подготовка (Arrange)
            double price = 57470; // Цена салона "Миасс"
            double discount = 4; // Скидка салона "Миасс"
            double discountParent = 0; // Скидка родителя (в данном случае 0, так как салон "Миасс" не имеет родителя)
            double exceptedResult = 55171.2; // Ожидаемый результат после вычисления цены

            // Действие (Act)
            Salon salon = new Salon(); // Создаем объект класса Salon
            double result = salon.CalculatePrice(price, discount, discountParent); // Вычисляем цену салона с учетом скидки и скидки родителя

            // Проверка (Assert)
            Assert.That(exceptedResult, Is.EqualTo(result)); // Проверяем, что результат вычисления цены совпадает с ожидаемым результатом
        }

        [Test]
        public void CalculatePriceAmelia()
        {
            // Arrange 
            double price = 5360;
            double discount = 5;
            double discountParent = 4;
            double exceptedResult = 4877.6;

            // Act
            Salon salon = new Salon();
            double result = salon.CalculatePrice(price, discount, discountParent);

            // Assert
            Assert.That(exceptedResult, Is.EqualTo(result));
        }

        [Test]
        public void CalculatePriceTest1()
        {
            // Arrange 
            double price = 136540;
            double discount = 2;
            double discountParent = 9; // В данном случае 9, так как салон "Тест1" имеет 2-х родителей - Амелия со скидкой 5% и Миасс со скидкой 4%
            double exceptedResult = 121520.6;

            // Act
            Salon salon = new Salon();
            double result = salon.CalculatePrice(price, discount, discountParent);

            // Assert
            Assert.That(exceptedResult, Is.EqualTo(result));
        }

        [Test]
        public void CalculatePriceTest2()
        {
            // Arrange 
            double price = 54054;
            double discount = 0;
            double discountParent = 4;
            double exceptedResult = 51891.84;

            // Act
            Salon salon = new Salon();
            double result = salon.CalculatePrice(price, discount, discountParent);

            // Assert
            Assert.That(exceptedResult, Is.EqualTo(result));
        }

        [Test]
        public void CalculatePriceKyrgan()
        {
            // Arrange 
            double price = 57850;
            double discount = 11;
            double discountParent = 0;
            double exceptedResult = 51486.5;

            // Act
            Salon salon = new Salon();
            double result = salon.CalculatePrice(price, discount, discountParent);

            // Assert
            Assert.That(exceptedResult, Is.EqualTo(result));
        }
    }
}