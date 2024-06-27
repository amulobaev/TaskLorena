using TaskLorena;

namespace TaskLorenaTest
{
    public class Tests
    {
        // �������� ����� ��� �������� ������������ ���������� ���� ��� ������ "�����"
        [Test]
        public void CalculatePriceMiass()
        {
            // ���������� (Arrange)
            double price = 57470; // ���� ������ "�����"
            double discount = 4; // ������ ������ "�����"
            double discountParent = 0; // ������ �������� (� ������ ������ 0, ��� ��� ����� "�����" �� ����� ��������)
            double exceptedResult = 55171.2; // ��������� ��������� ����� ���������� ����

            // �������� (Act)
            Salon salon = new Salon(); // ������� ������ ������ Salon
            double result = salon.CalculatePrice(price, discount, discountParent); // ��������� ���� ������ � ������ ������ � ������ ��������

            // �������� (Assert)
            Assert.That(exceptedResult, Is.EqualTo(result)); // ���������, ��� ��������� ���������� ���� ��������� � ��������� �����������
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
            double discountParent = 9; // � ������ ������ 9, ��� ��� ����� "����1" ����� 2-� ��������� - ������ �� ������� 5% � ����� �� ������� 4%
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