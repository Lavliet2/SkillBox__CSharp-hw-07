// See https://aka.ms/new-console-template for more information
class Repository
{
    private const string FilePath = "DataBase.txt";
    private string title = String.Format("{0,-5} {1,-20} {2,-30} {3,-10} {4,-10} {5,-15} {6,-20}",
            "ID", "Добавлен", "Ф.И.О.", "Возраст", "Рост", "Дата рождения", "Место рождения");
    private Worker[] workers;
    public Repository()
    {
        CreateFileIfNotExists();
        GetAllWorkers();
    }

    #region Public методы
    /// <summary>
    /// Парсинг файла (база данных)
    /// </summary>
    /// <returns>Массив сотрудников</returns>
    public Worker[] GetAllWorkers()
    {
        string[] lines = File.ReadAllLines(FilePath);
        workers = new Worker[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            workers[i] = ParseWorkerFromString(lines[i]);
        }

        return workers;
    }
   
    /// <summary>
    /// Вывод данных всех сотрудников
    /// </summary>
    /// <returns>Строку со всеми работниками</returns>
    public void PrintAllWorkers()
    {
        // Добавляем заголовки
        Console.WriteLine(title);
        GetAllWorkers();
        foreach (Worker worker in workers) 
        {
            PrintWorker(worker);
        }
    }

    /// <summary>
    /// Вывод информации о сотрудника
    /// </summary>
    /// <param name="worker"></param>
    /// <param name="needTitle"></param>
    public void PrintWorker(Worker worker, bool needTitle = false)
    {
        if ( needTitle) Console.WriteLine(title);

        Console.WriteLine("{0,-5} {1,-20} {2,-30} {3,-10} {4,-10} {5,-15} {6,-20}",
            worker.ID, worker.AddedDateTime.ToString("dd.MM.yyyy HH:mm"), worker.FIO,
            worker.Age, worker.Height, worker.DateOfBirth.ToString("dd.MM.yyyy"), worker.PlaceOfBirth);
    }

    /// <summary>
    /// Найти сотридника по ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Worker</returns>
    public Worker? GetWorkerById(int id)
    {
        foreach (Worker worker in workers) 
        {
            if (worker.ID == id) return worker;
        }
        return null;
        
    }

    /// <summary>
    /// Добавить соткудника в базу
    /// </summary>
    /// <param name="worker"></param>
    public void AddWorker(Worker worker)
    {
        worker.ID = GetUniqueWorkerId();
        worker.AddedDateTime = DateTime.Now;

        StreamWriter writer = new StreamWriter(FilePath, true);
        writer.WriteLine(WorkerToString(worker));
        writer.Close();

        GetAllWorkers();
    }

    public void EditWorker(int id, Worker newWorker)
    {
        bool found = false;

        for (int i = 0; i < workers.Length; i++)
        {
            if (workers[i].ID == id)
            {
                workers[i] = newWorker;
                found = true;
                break;
            }
        }

        if (found)
        {
            // Перезаписываем все данные в файл после редактирования
            RewriteDataToFile();
            Console.WriteLine($"Сотрудник с ID {id} успешно отредактирован.");
        }
        else
        {
            Console.WriteLine($"Сотрудник с ID {id} не найден.");
        }
    }

    /// <summary>
    /// Удаленрие сотрудника и перезапись файла (БД)
    /// </summary>
    /// <param name="id"></param>
    public void DeleteWorker(int id)
    {
        string[] lines = File.ReadAllLines(FilePath);
        StreamWriter writer = new StreamWriter(FilePath);

        foreach (string line in lines)
        {
            Worker worker = ParseWorkerFromString(line);
            if (worker.ID != id)
            {
                writer.WriteLine(line);
            }
        }
        writer.Close();

        GetAllWorkers();
    }

    /// <summary>
    /// Загрузка записей в выбранном диапазоне дат.
    /// </summary>
    /// <param name="dateFrom"></param>
    /// <param name="dateTo"></param>
    /// <returns></returns>
    public Worker[] GetWorkersBetweenTwoDates(DateTime dateFrom, DateTime dateTo)
    {
        int count = 0;
        Worker[] searchWorkers = new Worker[2];
        foreach (Worker worker in workers)
        {
            if (worker.AddedDateTime >= dateFrom && worker.AddedDateTime <= dateTo)
            {
                workers[count++] = worker;
            }
        }

        Array.Resize(ref workers, count);
        return workers;
    }

    /// <summary>
    /// Сортирповка по колонке
    /// </summary>
    /// <param name="sortBy"></param>
    public void PrintAllWorkersSorted(string sortBy)
    {
        // Добавляем заголовки
        Console.WriteLine(title);

        switch (sortBy)
        {
            case "ID":
                Array.Sort(workers, (x, y) => x.ID.CompareTo(y.ID));
                break;
            case "AddedDateTime":
                Array.Sort(workers, (x, y) => x.AddedDateTime.CompareTo(y.AddedDateTime));
                break;
            case "FIO":
                Array.Sort(workers, (x, y) => String.Compare(x.FIO, y.FIO, StringComparison.Ordinal));
                break;
            case "Age":
                Array.Sort(workers, (x, y) => x.Age.CompareTo(y.Age));
                break;
            case "Height":
                Array.Sort(workers, (x, y) => x.Height.CompareTo(y.Height));
                break;
            case "DateOfBirth":
                Array.Sort(workers, (x, y) => x.DateOfBirth.CompareTo(y.DateOfBirth));
                break;
            case "PlaceOfBirth":
                Array.Sort(workers, (x, y) => String.Compare(x.PlaceOfBirth, y.PlaceOfBirth, StringComparison.Ordinal));
                break;
            default:
                Console.WriteLine($"Некорректный критерий сортировки: {sortBy}");
                return;
        }

        foreach (Worker worker in workers)
        {
            PrintWorker(worker);
        }
    }
    #endregion

    #region Private методы
    /// <summary>
    /// Парсинг данных о сотруднике из строки
    /// </summary>
    private Worker ParseWorkerFromString(string line)
    {
        string[] parts = line.Split('#');
        return new Worker
        {
            ID = int.Parse(parts[0]),
            AddedDateTime = DateTime.Parse(parts[1]),
            FIO = parts[2],
            Age = int.Parse(parts[3]),
            Height = float.Parse(parts[4]),
            DateOfBirth = DateOnly.Parse(parts[5]),
            PlaceOfBirth = parts[6]
        };
    }

    /// <summary>
    /// Преобразование "Worker" в строку для записи в файл
    /// </summary>
    /// <param name="worker"></param>
    /// <returns></returns>
    private string WorkerToString(Worker worker)
    {
        return $"{worker.ID}#{worker.AddedDateTime:dd.MM.yyyy HH:mm}#{worker.FIO}#{worker.Age}#{worker.Height}#{worker.DateOfBirth}#{worker.PlaceOfBirth}";
    }

    /// <summary>
    /// Получить новый ID сотрудника
    /// </summary>
    /// <returns></returns>
    private int GetUniqueWorkerId()
    {
        Worker[] workers = GetAllWorkers();
        int maxId = 0;

        foreach (Worker worker in workers)
        {
            if (worker.ID > maxId)
            {
                maxId = worker.ID;
            }
        }

        return maxId + 1;
    }

    /// <summary>
    /// Проверка есть ли файл с базой, если нет то создать новый
    /// </summary>
    public void CreateFileIfNotExists()
    {
        if (!File.Exists(FilePath))
        {
            using (FileStream fs = File.Create(FilePath)) { }
        }
    }
    
    /// <summary>
    /// Перезапись сотрудника
    /// </summary>
    private void RewriteDataToFile()
    {
        using (StreamWriter writer = new StreamWriter(FilePath, false))
        {
            foreach (Worker worker in workers)
            {
                writer.WriteLine(WorkerToString(worker));
            }
        }
    }
    #endregion
}
