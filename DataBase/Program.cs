// See https://aka.ms/new-console-template for more information
using System;
using System.IO;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

class Program
{
    //static string filePath = "DataBase.txt";

    static void Main()
    {
        // Создаем файл, если он не существует
        //CreateFileIfNotExists();
        Repository repository = new Repository();

        while (true)
        {
            Console.WriteLine("{0, 15}", "Меню");
            Console.WriteLine("1 - Посмотреть все записи");
            Console.WriteLine("2 - Посмотреть запись по ID");
            Console.WriteLine("3 - Отсортировыать по колонке");
            Console.WriteLine("4 - Добавить сотрудника");
            Console.WriteLine("5 - Редактировать сотрудника");
            Console.WriteLine("6 - Удалить сотрудника");
            Console.WriteLine("7 - Загрузка записей в выбранном диапазоне дат");
            Console.WriteLine("8 - Выход");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    repository.PrintAllWorkers();
                    break;

                case "2":
                    ShowWorkerById(repository);
                    break;
                case "3":
                    SortByColumn(repository);
                    break;
                case "4":
                    AddNewWorker(repository);
                    break;
                case "5":
                    EditWorker(repository);
                    break;
                case "6":
                    DeleteWorkerById(repository);
                    break;
                case "7":
                    SearchWorkersBetweenTwoDates(repository);
                    break;
                case "8":
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                    break;
            }
        }
    }

    /// <summary>
    /// Посмотреть запись о сотруднеке по ID
    /// </summary>
    /// <param name="repository"></param>
    static void ShowWorkerById(Repository repository)
    {
        int id;
        do
        {
            Console.WriteLine("Введите ID сотрудника");
        } while (!int.TryParse(Console.ReadLine(), out id));

        Worker? foundWorker = repository.GetWorkerById(id);
        if (foundWorker != null)
        {
            repository.PrintWorker(foundWorker.Value, true);
        }
        else
        {
            Console.WriteLine($"Сотрудник с ID {id} не найден.");
        }        
    }

    /// <summary>
    /// Добавить нового сотрудника
    /// </summary>
    /// <param name="repository"></param>
    static void AddNewWorker(Repository repository)
    {
        Console.WriteLine("Введите данные нового сотрудника:");

        Worker newWorker = new Worker();

        Console.Write("Ф. И. О.: ");
        newWorker.FIO = Console.ReadLine();

        int age;
        bool isOk = false;
        do
        {
            Console.Write("Возраст: ");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out age) && age > 0)
            {
                isOk = true;
                newWorker.Age = age;
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
            }
        } while (!isOk);


        float height;
        isOk = false;
        do
        {
            Console.Write("Рост: ");
            string userInput = Console.ReadLine();

            if (float.TryParse(userInput, out height) && height > 0)
            {
                isOk = true;
                newWorker.Height = height;
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
            }
        } while (!isOk);

        DateOnly birthDate;
        isOk = false;
        do
        {
            Console.Write("Дата рождения: ");
            string userInput = Console.ReadLine();

            if (DateOnly.TryParse(userInput, out birthDate))
            {
                isOk = true;
                newWorker.DateOfBirth = birthDate;
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите дату в формате (ДД.ММ.ГГГГ).");
            }
        } while (!isOk);


        Console.Write("Место рождения: ");
        newWorker.PlaceOfBirth = Console.ReadLine();

        repository.AddWorker(newWorker);
    }
    
    /// <summary>
    /// Редактирование сотрудника
    /// </summary>
    /// <param name="repository"></param>
    static void EditWorker(Repository repository)
    {
        int id;
        do
        {
            Console.WriteLine("Введите ID сотрудника для редактирования");
        } while (!int.TryParse(Console.ReadLine(), out id));

        Worker? foundWorker = repository.GetWorkerById(id);
        if (foundWorker != null)
        {
            repository.PrintWorker(foundWorker.Value, true);
            Worker newWorker = foundWorker.Value;
            /*
            Worker newWorker = new Worker()
            {
                ID = id,
                AddedDateTime = foundWorker.Value.AddedDateTime,
            };
            */

            while (true)
            {
                Console.WriteLine("{0, 15}", "Выберите поле для редактирования");
                Console.WriteLine("1 - Ф.И.О.");
                Console.WriteLine("2 - Возраст");
                Console.WriteLine("3 - Рост");
                Console.WriteLine("4 - Дата рождения");
                Console.WriteLine("5 - Место рождения");
                Console.WriteLine("6 - Вернуться в меню");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                    {
                       Console.Write("Введите ФИО сотрудника: ");
                        newWorker.FIO = Console.ReadLine();
                        break;
                    }
                    case "2":
                    {
                        int age;
                        bool isOk = false;
                        do
                        {
                            Console.Write("Введите возраст сотрудника: ");
                            string userInput = Console.ReadLine();

                            if (int.TryParse(userInput, out age) && age > 0)
                            {
                                isOk = true;
                                newWorker.Age = age;
                            }
                            else
                            {
                                Console.WriteLine("Некорректный ввод. Пожалуйста, введите целое число.");
                            }
                        } while (!isOk);
                        break;
                    }                        
                    case "3":
                    {
                        float height;
                        bool isOk = false;
                        do
                        {
                            Console.Write("Введите рост сотрудника: ");
                            string userInput = Console.ReadLine();

                            if (float.TryParse(userInput, out height) && height > 0)
                            {
                                isOk = true;
                                newWorker.Height = height;
                            }
                            else
                            {
                                Console.WriteLine("Некорректный ввод. Пожалуйста, введите число.");
                            }
                        } while (!isOk);
                        break;
                    }
                    case "4":
                    {
                        DateOnly birthDate;
                        bool isOk = false;
                        do
                        {
                            Console.Write("Дата рождения: ");
                            string userInput = Console.ReadLine();

                            if (DateOnly.TryParse(userInput, out birthDate))
                            {
                                isOk = true;
                                newWorker.DateOfBirth = birthDate;
                            }
                            else
                            {
                                Console.WriteLine("Некорректный ввод. Пожалуйста, введите дату в формате (ДД.ММ.ГГГГ).");
                            }
                        } while (!isOk);
                        break;
                    }                        
                    case "5":
                    {
                        Console.Write("Место рождения: ");
                        newWorker.PlaceOfBirth = Console.ReadLine();
                        break;
                    }
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                        break;
                }
                repository.EditWorker(id, newWorker);
            }
        }
        else
        {
            Console.WriteLine($"Сотрудник с ID {id} не найден.");
        }
    }

    /// <summary>
    /// Удалить сотрудника из базы по ID
    /// </summary>
    /// <param name="repository"></param>
    static void DeleteWorkerById(Repository repository)
    {
        int id;
        do
        {
            Console.WriteLine("Введите ID сотрудника для удаления");
        } while (!int.TryParse(Console.ReadLine(), out id));

        Worker? foundWorker = repository.GetWorkerById(id);
        if (foundWorker != null)
        {
            Console.WriteLine($"Сотрудник: {foundWorker.Value.FIO} c ID: {id} успешно удалён из базы");
            repository.DeleteWorker(id);            
        }
        else
        {
            Console.WriteLine($"Сотрудник с ID {id} не найден.");
        }
    }

    /// <summary>
    /// Загрузка записей в выбранном диапазоне дат.
    /// </summary>
    /// <param name="repository"></param>
    static void SearchWorkersBetweenTwoDates(Repository repository)
    {
        DateTime dateFrom;
        bool isOk = false;
        do
        {
            Console.WriteLine("Введите с какой даты выгрузить список");
            string userInput = Console.ReadLine();


            if (DateTime.TryParse(userInput, out dateFrom))
            {
                isOk = true;
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, дату в формате (ДД.ММ.ГГГГ)");
            }
        } while (!isOk);

        DateTime dateTo;
        isOk = false;
        do
        {
            Console.WriteLine("Введите по какую дату выгрузить список");
            string userInput = Console.ReadLine();


            if (DateTime.TryParse(userInput, out dateTo))
            {
                isOk = true;
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, дату в формате (ДД.ММ.ГГГГ)");
            }
        } while (!isOk);

        var tempWorkers = repository.GetWorkersBetweenTwoDates(dateFrom, dateTo);

        foreach(var worker in tempWorkers)
        {
            repository.PrintWorker(worker);
        }
    }

    /// <summary>
    /// Сортировка по колонке 
    /// </summary>
    /// <param name="repository"></param>
    static void SortByColumn(Repository repository)
    {
        while (true)
        {
            Console.WriteLine("{0, 15}", "Выберите колонку для сортировки");
            Console.WriteLine("1 - ID");
            Console.WriteLine("2 - Дата и время добавления записи");
            Console.WriteLine("3 - Ф.И.О.");
            Console.WriteLine("4 - Возраст");
            Console.WriteLine("5 - Рост");
            Console.WriteLine("6 - Дата рождения");
            Console.WriteLine("7 - Место рождения");
            Console.WriteLine("8 - Вернуться в меню");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    repository.PrintAllWorkersSorted("ID");
                    break;
                case "2":
                    repository.PrintAllWorkersSorted("AddedDateTime");
                    break;
                case "3":
                    repository.PrintAllWorkersSorted("FIO");
                    break;
                case "4":
                    repository.PrintAllWorkersSorted("Age");
                    break;
                case "5":
                    repository.PrintAllWorkersSorted("Height");
                    break;
                case "6":
                    repository.PrintAllWorkersSorted("DateOfBirth");
                    break;
                case "7":
                    repository.PrintAllWorkersSorted("PlaceOfBirth");
                    break;
                case "8":
                    return;

                default:
                    Console.WriteLine("Некорректный ввод. Попробуйте снова.");
                    break;
            }
        }
    }
}
