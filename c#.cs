using System;

public class Worker
{
    // Имя
    public string Name { get; }

    // Фамилия
    public string Surname { get; }

    // Ставка за день (только целое число)
    private int _rate;
    public int Rate
    {
        get => _rate;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Rate), "Rate must be non-negative integer.");
            _rate = value;
        }
    }

    // Количество отработанных дней
    private int _days;
    public int Days
    {
        get => _days;
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(Days), "Days must be non-negative integer.");
            _days = value;
        }
    }

    public Worker(string name, string surname, int rate, int days)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(surname)) throw new ArgumentNullException(nameof(surname));
        if (rate < 0) throw new ArgumentOutOfRangeException(nameof(rate), "Rate must be non-negative integer.");
        if (days < 0) throw new ArgumentOutOfRangeException(nameof(days), "Days must be non-negative integer.");

        Name = name;
        Surname = surname;
        _rate = rate;
        _days = days;
    }

    // Возвращает "Фамилия Имя"
    public string GetFullName()
    {
        return $"{Surname} {Name}";
    }

    // Возвращает зарплату: ставка * дни
    public int GetSalary()
    {
        return Rate * Days;
    }
}
class Program
{
    static void Main()
    {
        var worker = new Worker("Иван", "Иванов", 1000, 20);
        Console.WriteLine(worker.GetFullName());        // Иванов Иван
        Console.WriteLine(worker.GetSalary());          // 20000

        // Можно изменить ставку/дни через свойства
        worker.Rate = 1200;
        worker.Days = 22;
        Console.WriteLine(worker.GetFullName());        // Иванов Иван
        Console.WriteLine(worker.GetSalary());          // 26400
    }
}