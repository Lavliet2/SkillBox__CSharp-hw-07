// See https://aka.ms/new-console-template for more information
using System;

/// <summary>
/// Структура, описывающая рабочего
/// </summary>
struct Worker
{
    #region Конструкторы
    /// <summary>
    /// Создание сотрудника (конструктор)
    /// </summary>
    public Worker(int ID, DateTime AddedDateTime, string FIO, int Age, float Height, DateOnly DateOfBirth, string PlaceOfBirth)
    {
        this.ID = ID;
        this.AddedDateTime = AddedDateTime;
        this.FIO = FIO;
        this.Age = Age;
        this.Height = Height;
        this.DateOfBirth = DateOfBirth;
        this.PlaceOfBirth = PlaceOfBirth;
    }


    #endregion

    #region Свойства
    /// <summary>
    /// Идентификатор рабочего
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// Дата и время добавления записи
    /// </summary>
    public DateTime AddedDateTime { get; set; }

    /// <summary>
    /// Ф.И.О работника
    /// </summary>
    public string FIO { get; set; }

    /// <summary>
    /// Возраст
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Рост
    /// </summary>
    public float Height { get; set; }

    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Место рождения
    /// </summary>
    public string PlaceOfBirth { get; set; }
    #endregion

    #region Методы
    /// <summary>
    /// Вывод данных о сотруднике
    /// </summary>
    public string Print()
    {
        string printValue = $"Идентификатор: {ID}" +
            $" Дата и время добавления записи: {AddedDateTime}" +
            $" Ф.И.О.: {FIO}" +
            $" Возраст: {Age}" +
            $" Рост: {Height}" +
            $" День рождения: {DateOfBirth}" +
            $" Место рождения: {PlaceOfBirth}";
        return printValue;
    }
    #endregion
}
