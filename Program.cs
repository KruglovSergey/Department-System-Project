using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using Enin_Homework_08;


namespace Homework_08
{
    #nullable enable
    class Program
    {        
        static void Main(string[] args)
        {
            List<Department> departments = new List<Department>();  //Список с существующими департаментами
            Director director = new Director();                          //Директор департамента
            Department common = new Department("Common", director);      //Департамент со всеми сотрудниками
            #region ConsConfig
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Title = "DEPARTMENTS & WORKERS";
            Console.SetWindowSize(100, 30);
            #endregion
            while (true)                                            //Бесконечный цикл пока пользователь не выйдет из главного меню
            {
                MainMenu(ref departments, ref common, ref director);
            };
        }
        /// <summary>
        /// Метод для главного меню, с которого начинается программа
        /// </summary>
        /// <param name="departments">Список департаментов</param>
        /// <param name="common">Общий департамент</param>
        public static void MainMenu(ref List<Department> departments, ref Department common, ref Director director)
        {
            Console.Clear();    //Очистка консоли          
            int mainChoiсe;
            if (departments.Count == 0)    //Если не открыт не открыта БД, то выбор ограничен
            {
                Console.WriteLine(
                "1 - Create Department.\n" +
                "7 - Import Departments.\n\n"+
                "8 - Exit.\n"
                );
            }
            else
            { 
            Console.WriteLine(
                "1 - Create Department.\n" +
                "2 - Show All Departments.\n" +
                "3 - Delete Department.\n" +
                "4 - Edit Department.\n" +
                "5 - Show All Workers.\n" +
                "6 - Streamline Workers.\n" +
                "7 - Import / Export Departments.\n\n" +
                "8 - Exit.\n"
                ); 
            }
            Console.Write("Enter you choice: ");
            Int32.TryParse(Console.ReadLine(), out mainChoiсe);
            Console.WriteLine();
            switch (mainChoiсe)
            {
                case 1:   //Создать департамент

                    Console.Clear();
                    CreateDepartment(ref departments);
                    break;

                case 2: //Показать существующие департаменты

                    Console.Clear();
                    if (departments.Count == 0)
                        break;
                    else
                    {
                        ShowAllDepartments(ref departments);
                        common.PrintDepInfo();
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadKey();
                    }
                    break;

                case 3: //Удалить департамент

                    Console.Clear();
                    if (departments.Count == 0)
                        break;
                    ShowAllDepartments(ref departments);
                    DeleteDepartment(ref departments,ref common);                    
                    break;

                case 4: //Редактировать департамент

                    Console.Clear();
                    if (departments.Count == 0) 
                        break;
                    EditDepartment(ref departments, ref common, ref director);
                    break;

                case 5: //Отобразить список всех сотрудников

                    if (common.workers.Count == 0)
                        break;
                    Console.Clear();
                    ShowAllWorkers(common);
                    break;

                case 6: //Упорядочить сотрудников

                    if (common.workers.Count == 0)
                        break;
                    char ch = 'e';
                    do
                    {
                        StreamLineWorkers(common);
                        Console.Write("Back? (y/n): ");
                        ch = Console.ReadKey().KeyChar;
                    }
                    while (ch != 'y');
                    break;

                case 7: //Импорт и экспорт JSON и XML

                    int intForSelect;
                    do
                    {
                        Console.Clear();
                        if (departments.Count == 0)
                        {
                            Console.WriteLine(
                            "1 - Open XML.\n" +
                            "3 - Open JSON.\n" +
                            "0 - Back.\n");
                        }
                        else
                        {
                            Console.WriteLine(
                                "1 - Open XML.\n" +
                                "2 - Save as XML.\n" +
                                "3 - Open JSON.\n" +
                                "4 - Save as JSON.\n\n" +
                                "0 - Back.\n");
                        }
                        Console.Write("Select item: ");
                        Int32.TryParse(Console.ReadLine(), out intForSelect);
                        if (intForSelect < 0 || intForSelect > 4)
                        {
                            Console.WriteLine("Input Error. Enter a number from the valid range. Try again.");
                            Console.ReadKey();
                        }
                    } 
                    while (intForSelect < 0 || intForSelect > 4);
                    if (intForSelect == 0)
                        break;
                    switch (intForSelect)
                    {
                        case 1: //Открыть (импортировать) XML

                            char chForImportXML;
                            if (departments.Count != 0)
                            {
                                Console.Write
                                  ("Сurrent data will be overwritten.\n" +
                                  "It is recommended to export them before importing new ones.\n" +
                                  "import anyway? (y/n): ");
                                chForImportXML = Console.ReadKey().KeyChar;
                                if (chForImportXML == 'y')
                                {
                                    for (int i = common.workers.Count; i > 0; i--)
                                    {
                                        common.workers.RemoveAt(i - 1);
                                    }
                                    ImportFromXML(ref departments, out departments, ref common);
                                }
                            }
                            else ImportFromXML(ref departments, out departments, ref common);
                            break;

                        case 2: //Сохранить (экспортировать) XML

                            ExportToXML(ref departments);
                            break;

                        case 3: //Открыть (импортировать) JSON

                            char chForImportJSON; 
                            if (departments.Count != 0)
                            {
                                Console.Write
                                  ("Сurrent data will be overwritten.\n" +
                                  "It is recommended to export them before importing new ones.\n" +
                                  "import anyway? (y/n): ");
                                chForImportJSON = Console.ReadKey().KeyChar;
                                if (chForImportJSON == 'y')
                                {
                                    for (int i = common.workers.Count; i > 0; i--)
                                    {
                                        common.workers.RemoveAt(i - 1);
                                    }
                                    ImportFromJSON(ref departments, out departments, ref common);
                                }
                            }
                            else ImportFromJSON(ref departments, out departments, ref common);
                            break;

                        case 4: //Сохранить (экспортировать) JSON

                            ExportToJSON(ref departments);                            
                            break;
                    }
                    break;

                case 8: //Выход
                    if (departments.Count != 0)
                    {
                        char chForSave = 'o';
                        Console.Write("Save before exit? (y/n): "); //Предложение сохранить перед выходом
                        chForSave = Console.ReadKey().KeyChar;
                        if (chForSave == 'y')
                        {
                            Console.WriteLine(
                                "\n1 - Save XML\n" +
                                "2 - Save JSON\n");
                            int intforSave;
                            Console.Write("Select item: ");
                            Int32.TryParse(Console.ReadLine(), out intforSave);
                            switch (intforSave)
                            {
                                case 1:
                                    ExportToXML(ref departments);
                                    Environment.Exit(0);
                                    break;
                                case 2:
                                    ExportToJSON(ref departments);
                                    Environment.Exit(0);
                                    break;
                            }
                        }
                        else Environment.Exit(0);
                    }
                    else Environment.Exit(0);
                    break;
            }
        }

        /// <summary>
        /// Метод для создания нового департамента
        /// </summary>
        /// <param name="departments"></param>
        public static void CreateDepartment(ref List<Department> departments)
        {
            string name;
            do
            {
                Console.Write("Enter the name: ");
                name = Console.ReadLine();
                if (name == "") //Проверка на пустую строку
                {
                    Console.Clear();
                    Console.WriteLine("The name cannot be empty. Try it again");
                }
            }
            while (name == "");
            Console.Write($"\nAdd Director Departament: \n");
            var director = AddDirector(); //Создаем директора
            Department dep = new Department(name, director);
            departments.Add(dep);   //Добавляю департамент
            dep.PrintDepInfo();
            Console.ReadKey();
            Console.Clear();
            Console.Write($"Department {departments[departments.Count - 1].Name} has been added.\nPress Enter to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Метод для добавления директора департамента. Возвращает объект директора
        /// </summary>
        /// <param name="department"></param>
        public static Director AddDirector()
        {
            //Переменные для директора
            string fname, lname;
            int age, pay;
            //Даллее проверка на пустую строку, либо параметр на диапазон
            //Если корректный ввод, то значение присваивается соответсвующему полю Worker
            do
            {
                Console.Write("First Name: ");
                fname = Console.ReadLine();
                if (fname == "")
                {
                    Console.WriteLine("The field cannot be empty.\nTry again.");
                }
            }
            while (fname == "");
            do
            {
                Console.Write("Last Name: ");
                lname = Console.ReadLine();
                if (lname == "")
                {
                    Console.WriteLine("The field cannot be empty.\nTry again.");
                }
            }
            while (lname == "");
            do
            {
                Console.Write("Age (15-100):");
                Int32.TryParse(Console.ReadLine(), out age);
                if (age > 100 || age < 15)
                {
                    Console.WriteLine("Age must be real.\nTry again.");
                }
            }
            while (age > 100 || age < 15);
            do
            {
                Console.Write("Pay (10000-600000):");
                Int32.TryParse(Console.ReadLine(), out pay);
                if (pay > 600_000 || pay < 10_000)
                {
                    Console.WriteLine("Pay must be real.\nTry again.");
                }
            }
            while (pay > 600_000 || pay < 10_000);
            //Создаю и возвращаю оъект
            Director w = new Director(fname, lname, age, pay);
            return w;
        }

        /// <summary>
        /// Метод для удаления департамента
        /// </summary>
        /// <param name="departments"></param>
        public static void DeleteDepartment(ref List<Department> departments, ref Department common)
        {
            int intforDelDep;
            do
            {
                //Выбо сотрудника для удаления
                Console.Write($"Choose Department for Delete (1-{departments.Count}): ");
                Int32.TryParse(Console.ReadLine(), out intforDelDep);
                //Проверка выбора на вхождение в диапазон
                if (intforDelDep < 1 || intforDelDep > departments.Count || intforDelDep == 0)
                    Console.WriteLine("Input Error. The value entered must be a number from the valid range! Try again.");
            } 
            while (intforDelDep < 1 || intforDelDep > departments.Count || intforDelDep == 0);
            //Переменная с именем удаляемого департамента
            string deletedDep = departments[intforDelDep - 1].Name;
            //Сперва удаляю всех сотрудников для корректного смещения ID
            for (int i = departments[intforDelDep - 1].workers.Count; i > 0; i--)
            {
                DeleteWorker(departments[intforDelDep - 1], common, i);
            }
            //Перезаписываю колличество сотрудников в общем списке
            common.NumbOfWorkers = common.workers.Count;
            //Удаляю департамент
            departments.RemoveAt(intforDelDep - 1);
            Console.WriteLine($"Department {deletedDep} has been removed.\nPress Enter to continue.");
            Console.ReadKey();
        }


        /// <summary>
        /// Метод редактироваиния департамента
        /// </summary>
        /// <param name="departments">Список департаментов</param>
        /// <param name="common">Общий департамент</param>
        /// <param name="director">Директор департамента</param>
        public static void EditDepartment(ref List<Department> departments, ref Department common, ref Director director)
        {
            Console.Clear();
            int intforEditDep;
            do
            {
                //Показываю все департаметы и предлагаю выбрать, что делать.
                ShowAllDepartments(ref departments);
                Console.Write($"Select department (1-{departments.Count})\nOr enter 0 to return: ");
                Int32.TryParse(Console.ReadLine(), out intforEditDep);
                //Проверка выбора на вхождение в диапазон
                if (intforEditDep > departments.Count - 1 || intforEditDep < 0)
                { 
                    Console.Clear(); 
                    Console.WriteLine("Input Error. Enter a number from the valid range.");
                }
            } 
            while (intforEditDep > departments.Count || intforEditDep < 0);
            Console.Clear();
            //Переменная для выбора
            int choice;
            do
            {
                //Если 0, то возврат
                if (intforEditDep == 0)
                { 
                    break;
                }
                Console.Clear();
                //Для наглядности вывожу информацию департамента
                departments[intforEditDep - 1].PrintDepInfo();
                Console.WriteLine(
               "0 - Edit Department Director.\n" +
               "1 - Rename Department.\n" +
               "2 - Show Workers.\n" +
               "3 - Add Worker.\n" +
               "4 - Edit Worker.\n" +
               "5 - Delete Worker.\n\n" +
               "6 - Back.\n");
                do
                {
                    //Выбор, что сделать
                    Console.Write("Enter you choice (0-6): ");
                    Int32.TryParse(Console.ReadLine(), out choice);
                    if (choice < 0 || choice > 6)
                    {
                        Console.WriteLine("Сhoose a number from the interval.");
                    }
                } 
                while (choice < 0 || choice > 6);
                switch (choice)
                {
                    case 0:
                        Console.Clear();
                        EditDirector(departments[intforEditDep - 1]); //Редактирование директора
                        break;
                    case 1: //Переименование департамента

                        Console.Clear();
                        string newName;
                        do
                        {
                            Console.Write("Enter new name: ");
                            newName = Console.ReadLine();
                            if (newName == "") 
                            {
                                Console.WriteLine("The field cannot be empty."); 
                            }
                        } 
                        while (newName == "");
                        //Если предыдущая проверка не выявила пустую строку с ввода, то переименовываю
                        RenameDepartment(departments[intforEditDep - 1], common, newName);
                        break;

                    case 2: //Вывод списка сотрудников

                        Console.Clear();
                        //Если есть сотрудники, то вывожу на консоль, иначе - оповещение
                        if (departments[intforEditDep - 1].workers.Count > 0)
                        {
                            departments[intforEditDep - 1].PrintWorkerList();
                            Console.WriteLine("Press Enter to continue.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("List of Workers is empty\nPress Enter to continue.");
                            Console.ReadKey();
                        }
                        break;

                    case 3: //Добавление сотрудника
                        if (common.workers.Count < 1000000)
                        {
                            char repeat;
                            do
                            {
                                Console.Clear();
                                Console.WriteLine($"<<Add Worker to {departments[intforEditDep - 1].Name}>>");
                                //Записываю в переменную объект нового сотрудника
                                Worker temp = AddWorker(departments[intforEditDep - 1], common);
                                //Добавляю в текущий и общий департамент и корректирую поля с колличеством сотрудников
                                departments[intforEditDep - 1].workers.Add(temp);
                                departments[intforEditDep - 1].NumbOfWorkers = departments[intforEditDep - 1].workers.Count;
                                common.workers.Add(temp);
                                common.NumbOfWorkers = common.workers.Count;
                                Console.Clear();
                                Console.WriteLine("Worker has been added!");
                                Console.WriteLine($"Total workers in {departments[intforEditDep - 1].Name} now: {departments[intforEditDep-1].workers.Count}");
                                //Можно повторить ввод
                                Console.Write("Add more? (y/n)");
                                repeat = Console.ReadKey().KeyChar;
                            }
                            while (repeat == 'y');
                        }
                        break;

                    case 4: //Редактирование сотрудника
                        //Если список сотрудников пуст, то вывод оповещения и возврат в меню

                        Console.Clear();
                        if (departments[intforEditDep - 1].workers.Count == 0)
                        {
                            Console.WriteLine("List of Workers is empty. Press Enter.");
                            Console.ReadKey();
                            break;
                        }
                        int workerNum;
                        //Иначе вывод списка на консоль и предложение выбрать сотрудника для редактирования
                        departments[intforEditDep - 1].PrintWorkerList();
                        do
                        {
                            //Фильтр для введенного значения
                            Console.Write($"Enter the number of the employee to be edited (1-{departments[intforEditDep - 1].workers.Count}) ");
                            Int32.TryParse(Console.ReadLine(), out workerNum);
                            if (workerNum == 0)
                            { 
                                break; 
                            }
                            if (workerNum < 1 || workerNum > departments[intforEditDep - 1].workers.Count)
                            { 
                                Console.WriteLine("An Worker with this number does not exist."); 
                            }
                        } 
                        while (workerNum < 1 || workerNum > departments[intforEditDep - 1].workers.Count);
                        Console.Clear();
                        if (workerNum == 0)
                        { 
                            break;
                        }
                        //Если проверка на ввод прошла успешно, то вызов метода для редактирвоания сотрдника
                        EditWorker(departments[intforEditDep - 1], common, workerNum);
                        break;

                    case 5: //Удаление сотрудника
                        //Если сотрудникв нет, то возврат

                        Console.Clear();
                        if (departments[intforEditDep - 1].workers.Count == 0)
                        {
                            Console.WriteLine("List of Workers is empty. Press Enter.");
                            Console.ReadKey();
                            break;
                        }
                        int intforWorkerDel;
                        departments[intforEditDep - 1].PrintWorkerList();
                        //Иначе предложение ввести номер сотрудника для удаления, фильтрация ввода и далее вызов метода для удаления
                        do
                        {
                            Console.Write
                                ($"Enter the number of the " +
                                $"employee to be deleted (1-{departments[intforEditDep - 1].workers.Count})" +
                                $"\nOr enter 0 to return: ");
                            Int32.TryParse(Console.ReadLine(), out intforWorkerDel);
                            if (intforWorkerDel == 0)
                            { 
                                break;
                            }
                            if (intforWorkerDel > departments[intforEditDep - 1].workers.Count  || intforWorkerDel < 0)
                                Console.WriteLine("Input Error.The value entered must be a number from the valid range! Try again.");
                        } 
                        while (intforWorkerDel > departments[intforEditDep - 1].workers.Count || intforWorkerDel < 0);
                        if (intforWorkerDel == 0) 
                        { 
                            break;
                        }

                        DeleteWorker(departments[intforEditDep - 1], common, intforWorkerDel);
                        Console.WriteLine("Worker removed\nPress Enter to continue");
                        Console.ReadKey();
                        break;
                }
            } 
            while (choice != 6);

        }

        /// <summary>
        /// Метод для переименовывания департамента
        /// </summary>
        /// <param name="department">Редактируемый департамент</param>
        /// <param name="common">Общий департамент</param>
        /// <param name="name">Новое имя</param>
        public static void RenameDepartment(Department department, Department common, string name)
        {
            //Переменная для текущего имени
            string temp = department.Name;
            //Для всех работников в департаменте присваиваем новое имя департамента
            foreach (Worker w1 in department.workers)
            {
                w1.DepartmentIN = name;
            }
            //Меняем имя
            department.Name = name;
            //Так же меняем имя департамента для всех сотрудников в общем отделе
            foreach (Worker w in common.workers)
            {
                if (w.DepartmentIN == temp)
                    w.DepartmentIN = name;
            }
            Console.WriteLine("Name has been changed.\nPress Enter to continiue");
            Console.ReadKey();
        }

        /// <summary>
        /// Метод для добавления сотрудника. Возвращает объект сотрудника
        /// </summary>
        /// <param name="departments">Список департаментов</param>
        /// <param name="department"></param>
        /// <param name="common"></param>
        /// <returns></returns>
        public static Worker AddWorker(Department department, Department common)
        {
            //Переменные для нового сотрудника
            string wFName, wLName; int wAge, wPay, wProj;
            //Даллее проверка на пустую строку, либо параметр на диапазон
            //Если корректный ввод, то значение присваивается соответсвующему полю Worker
            do
            {
                Console.Write("First Name: "); 
                wFName = Console.ReadLine();
                if (wFName == "") 
                { 
                    Console.WriteLine("The field cannot be empty.\nTry again.");
                }
            } 
            while (wFName == "");
            do
            {
                Console.Write("Last Name: "); 
                wLName = Console.ReadLine();
                if (wLName == "") 
                { 
                    Console.WriteLine("The field cannot be empty.\nTry again."); 
                }
            } 
            while (wLName == "");
            do
            {
                Console.Write("Age (15-100):"); 
                Int32.TryParse(Console.ReadLine(), out wAge);
                if (wAge > 100 || wAge < 15) 
                { 
                    Console.WriteLine("Age must be real.\nTry again."); 
                }
            } 
            while (wAge > 100 || wAge < 15);
            do
            {
                Console.Write("Pay (10000-300000):"); 
                Int32.TryParse(Console.ReadLine(), out wPay);
                if (wPay > 300_000 || wPay < 10_000) 
                { 
                    Console.WriteLine("Pay must be real.\nTry again."); 
                }
            } 
            while (wPay > 300_000 || wPay < 10_000);
            do
            {
                Console.Write("Number of Projects (1-10):"); 
                Int32.TryParse(Console.ReadLine(), out wProj);
                if (wProj > 10 || wProj < 1) 
                { 
                    Console.WriteLine("Number of projects must be real.\nTry again.");
                }
            } 
            while (wProj > 10 || wProj < 1);
            //Переменная для поля ID
            int numbOfWorker;
            //Если оба департамента пусты, то присваиваю 1
            if (department.workers.Count == 0 && common.workers.Count == 0)
            {
                numbOfWorker = 1;
            }
            //Иначе, номер следующий после максимальнго текущего
            else
            {
                numbOfWorker = common.workers.Max(x => x.ID) + 1;
            }
            //Создаею и возвращаю оъект
            Worker w = new Worker(numbOfWorker, wFName, wLName, wAge, department.Name, wPay, wProj);
            return w;
        }


        //Метод редактирования директора 
        public static void EditDirector(Department department)
        {
            //Переменные для записи новых значений полей объекта директора
            string fName, lName; int Age, Pay;

            int forWEdit;
            do
            {
                //Выводим информацию по выбранному сотруднику и предоставлеям выбор поля для редактирования
                Console.Clear();
                department.director.PrintDirector();
                Console.WriteLine(
                    $"1 - First name\n" +
                    $"2 - Last name\n" +
                    $"3 - Age\n" +
                    $"4 - Pay\n" +
                    $"5 - Back");
                Console.Write("Select option to edit: ");
                Int32.TryParse(Console.ReadLine(), out forWEdit);
                switch (forWEdit)
                {
                    //В зависимости от выбора предлагается ввести новое значение
                    //Новое значение принимается, если прошло проверку на корректность

                    case 1: //Редактирование имени

                        do
                        {
                            Console.Write("\nFirst Name:");
                            fName = Console.ReadLine();
                            if (fName == "")
                            {
                                Console.WriteLine("The field cannot be empty.\nTry again.");
                            }
                            else
                            {
                                department.director.FirstName = fName;
                            }
                        }
                        while (fName == "");
                        break;

                    case 2: //Редактирование фамилии

                        do
                        {
                            Console.Write("\nLast Name:");
                            lName = Console.ReadLine();
                            if (lName == "")
                            {
                                Console.WriteLine("The field cannot be empty.\nTry again.");
                            }
                            else
                            {
                                department.director.LastName = lName;
                                                            }
                        }
                        while (lName == "");
                        break;

                    case 3: //Редактирование возраста

                        do
                        {
                            Console.Write("\nAge (15-100):");
                            Int32.TryParse(Console.ReadLine(), out Age);
                            if (Age > 100 || Age < 15)
                            {
                                Console.WriteLine("Age must be real.\nTry again.");
                            }
                            else
                            {
                                department.director.Age = Age;
                            }
                        }
                        while (Age > 100 || Age < 15);
                        break;

                    case 4: //Редактирование зарплаты

                        do
                        {
                            Console.Write("\nPay (10000-600000):");
                            Int32.TryParse(Console.ReadLine(), out Pay);
                            if (Pay > 600_000 || Pay < 10_000)
                            {
                                Console.WriteLine("Pay must be real.\nTry again.");
                            }
                            else
                            {
                                department.director.Pay = Pay;
                            }
                        }
                        while (Pay > 600_000 || Pay < 10_000);
                        break;

                    }
            }
            while (forWEdit != 5);
        }

        /// <summary>
        /// Метод для удаления сотрудника
        /// </summary>
        /// <param name="department">Текущий департамент</param>
        /// <param name="common">Общий департамент</param>
        /// <param name="workerNumb">Номер сотрудника</param>
        public static void DeleteWorker(Department department, Department common, int workerNumb)
        {
            Worker temp1 = department.workers[workerNumb - 1];
            //Удаляем сотрудника из общего департамента
            common.workers.Remove(temp1);
            //Удалем из текущего департамента
            department.workers.RemoveAt(workerNumb - 1);
            //Присваиваем департаментам новое значение колличества сотрудников
            common.NumbOfWorkers = common.workers.Count;
            department.NumbOfWorkers = department.workers.Count;
            //В обоих департамментах для всех полседующих содрудников уменьшаем ID на 1
            //Решил так сделать, чтобы освобождалось место для новых, если значение ID подойдет к пределу
            foreach (Worker w in department.workers)
            {
                if (w.ID > temp1.ID)
                    w.ID--;
            }
            foreach (Worker w in common.workers)
            {
                if (w.ID > temp1.ID)
                    w.ID--;
            }
        }

        /// <summary>
        /// Метод для редактирования сотрудника
        /// </summary>
        /// <param name="department">Текущий департамент</param>
        /// <param name="common">Общий департамент</param>
        /// <param name="workerNumb">Номер редактируемого сотрудника</param>
        public static void EditWorker(Department department, Department common, int workerNumb)
        {
            //Переменные для записи новых значений полей объекта сотрудника
            string wFName, wLName; int wAge, wPay, wProj;

            int forWEdit;
            do
            {
                //Выводим информацию по выбранному сотруднику и предоставлеям выбор поля для редактирования
                Console.Clear();
                department.workers[workerNumb - 1].LukePrintWorker();
                Console.WriteLine(
                    $"1 - First name\n" +
                    $"2 - Last name\n" +
                    $"3 - Age\n" +
                    $"4 - Pay\n" +
                    $"5 - Number of projects\n\n" +
                    $"6 - Back");
                Console.Write("Select option to edit: ");                
                Int32.TryParse(Console.ReadLine(), out forWEdit);
                switch (forWEdit)
                {
                    //В зависимости от выбора предлагается ввести новое значение
                    //Новое значение принимается, если прошло проверку на корректность
                    //Значение применяется к записи сотрудника в его департаменте,
                    //а так же для его записи в общем департаменте
                    case 1: //Редактирование имени

                        do
                        {
                            Console.Write("\nFirst Name:"); 
                            wFName = Console.ReadLine();
                            if (wFName == "")
                            { 
                                Console.WriteLine("The field cannot be empty.\nTry again."); 
                            }
                            else
                            {
                                department.workers[workerNumb - 1].FirstName = wFName;
                                common.workers[department.workers[workerNumb - 1].ID - 1].FirstName = wFName;
                            }
                        } 
                        while (wFName == "");
                        break;

                    case 2: //Редактирование фамилии

                        do
                        {
                            Console.Write("\nLast Name:"); 
                            wLName = Console.ReadLine();
                            if (wLName == "")
                            { 
                                Console.WriteLine("The field cannot be empty.\nTry again."); 
                            }
                            else
                            {
                                department.workers[workerNumb - 1].LastName = wLName;
                                common.workers[department.workers[workerNumb - 1].ID - 1].LastName = wLName;
                            }
                        } 
                        while (wLName == "");
                        break;

                    case 3: //Редактирование возраста

                        do
                        {
                            Console.Write("\nAge (15-100):"); 
                            Int32.TryParse(Console.ReadLine(), out wAge);
                            if (wAge > 100 || wAge < 15) 
                            { 
                                Console.WriteLine("Age must be real.\nTry again."); 
                            }
                            else
                            {
                                department.workers[workerNumb - 1].Age = wAge;
                                common.workers[department.workers[workerNumb - 1].ID - 1].Age = wAge;
                            }
                        } 
                        while (wAge > 100 || wAge < 15);
                        break;

                    case 4: //Редактирование зарплаты

                        do
                        {
                            Console.Write("\nPay (10000-300000):"); 
                            Int32.TryParse(Console.ReadLine(), out wPay);
                            if (wPay > 300_000 || wPay < 10_000)
                            { 
                                Console.WriteLine("Pay must be real.\nTry again.");
                            }
                            else
                            {
                                department.workers[workerNumb - 1].Pay = wPay;
                                common.workers[department.workers[workerNumb - 1].ID - 1].Pay = wPay;
                            }
                        } 
                        while (wPay > 300_000 || wPay < 10_000);
                        break;

                    case 5: //Редактирование колличеств проектов

                        do
                        {
                            Console.Write("\nNumber of Projects (1-10):"); 
                            Int32.TryParse(Console.ReadLine(), out wProj);
                            if (wProj > 10 || wProj < 1) 
                            { 
                                Console.WriteLine("Number of projects must be real.\nTry again."); 
                            }
                            else
                            {
                                department.workers[workerNumb - 1].NumbOfProjects = wProj;
                                common.workers[department.workers[workerNumb - 1].ID - 1].NumbOfProjects = wProj;
                            }
                        } 
                        while (wProj > 10 || wProj < 1);
                        break;
                }
            } 
            while (forWEdit != 6);
        }

        /// <summary>
        /// Метод вывода на консоль всех имеющихся департаментов
        /// </summary>
        /// <param name="departments"></param>
        public static void ShowAllDepartments(ref List<Department> departments)
        {
            int count = 1;
            //Вызов метода PrintDepInfo() для всех департаментов
            foreach (Department d in departments)
            {
                Console.Write($"{count} ");
                d.PrintDepInfo(); 
                count++;
            }
        }

        /// <summary>
        /// Метод, отображающий всех сотрудников из общего департамента
        /// </summary>
        /// <param name="common"></param>
        public static void ShowAllWorkers(Department common)
        {
            Console.WriteLine("All Workers>>>\n");
            //Вызов метода PrintWorkerList() для общего департамента
            common.PrintWorkerList();
            Console.WriteLine("Press Enter to continiue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Метод для упорядочивания списка сотрудников
        /// </summary>
        /// <param name="common">Общий департамент</param>
        public static void StreamLineWorkers(Department common)
        {
            int intforSelect;
            do
            {
                //Вывод всех сотрудников и предложение выбора как сортировать
                Console.Clear();
                common.PrintWorkerList();
                Console.WriteLine(
                    "1 - Sort by pay.\n" +
                    "2 - Sort by age.\n" +
                    "3 - Sort by pay and age within the department.\n" +
                    "4 - Back.\n");
                Console.Write("Select the ordering option (1-3): ");
                Int32.TryParse(Console.ReadLine(), out intforSelect);
                if(intforSelect < 1 || intforSelect > 4)
                {
                    Console.WriteLine("Сhoose a number from the range.");
                    Console.ReadKey();
                }
            }
            while (intforSelect < 1 || intforSelect > 4);
            //Временный департамент
            Department tempForStreamLine = new Department();
            switch (intforSelect)
            {
                case 1: //Упорядочить сотрудников по оплате труда  

                    Console.Clear();
                    Console.WriteLine("Sort by pay.\n");
                    var SLByPay = common.workers.OrderBy(x => x.Pay);
                    tempForStreamLine.workers = SLByPay.ToList();
                    tempForStreamLine.PrintWorkerList();
                    Console.WriteLine("Press Enter to continue: ");
                    Console.ReadKey();
                    break;

                case 2: //Упорядочить сотрудников по возрасту

                    Console.Clear();
                    Console.WriteLine("Sort by age.\n");
                    var SLByAge = common.workers.OrderBy(x => x.Age);
                    tempForStreamLine.workers = SLByAge.ToList();
                    tempForStreamLine.PrintWorkerList();
                    Console.WriteLine("Press Enter to continue: ");
                    Console.ReadKey();
                    break;

                case 3: //Упорядочить сотрудников по возрасту и оплате труда в пределах одного департамента
                    
                    Console.Clear();
                    Console.WriteLine("Sort by pay and age within the department.\n");
                    var SLByPayAge = common.workers.OrderBy(x => x.DepartmentIN).ThenBy(x=>x.Age).ThenBy(x=>x.Pay);
                    tempForStreamLine.workers = SLByPayAge.ToList();
                    tempForStreamLine.PrintWorkerList();
                    Console.WriteLine("Press Enter to continue: ");
                    Console.ReadKey();
                    break;

                case 4:
                    break;
            }
        }

        /// <summary>
        /// Метод для экспорта департаментов в XML
        /// </summary>
        /// <param name="departments">Департаменты для экспорта</param>
        public static void ExportToXML (ref List<Department> departments)
        {
            DirectoryInfo diXMLSave = new DirectoryInfo(Environment.CurrentDirectory);  //DI каталога приложения
            FileInfo[] fiXMLSave = diXMLSave.GetFiles();    //Файлы каталога приложения
            Console.WriteLine("\nExisting files: ");
            foreach(FileInfo fileInfo in fiXMLSave)
            {
                if (fileInfo.Name.Contains(".xml")) 
                    Console.WriteLine(fileInfo.Name);
            }
            Console.Write("\nEnter file name: "); //Предложение ввести имя
            string xmlfileNameSave = Console.ReadLine();
            string pathToSaveXML = $"{xmlfileNameSave}.xml";   //Создание имени с расширением
            
            foreach (FileInfo f in fiXMLSave)   //Если файл с таким именем уже существует, то имя редактируется
            {
                if (f.Name == pathToSaveXML)
                {
                    char charForOverwrite;
                    Console.WriteLine("A file with the same name already exists. Overwrite? (y/n): ");
                    charForOverwrite = Console.ReadKey().KeyChar;
                    if (charForOverwrite != 'y')
                        pathToSaveXML = $"{xmlfileNameSave} (COPY_{DateTime.Now.ToLongTimeString()}).xml".Replace(':','_');
                }
            }
            //Сериализуем по указанному пути
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));
            try
            {
                using (Stream fStream = new FileStream(pathToSaveXML, FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fStream, departments);
                }
                Console.Write($"File {pathToSaveXML} successfully created.\nPress Enter to continiue.");
            }
            catch (Exception e) //Если не получилось, то ошибка
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to create file");
            }
            Console.ReadKey();       
        }
        
        /// <summary>
        /// Метод для экспорта департаментов в JSON
        /// </summary>
        /// <param name="departments">Департаменты для экспорта</param>
        public static void ExportToJSON (ref List<Department> departments)
        {
            DirectoryInfo diJSONSave = new DirectoryInfo(Environment.CurrentDirectory); //Директория приложения
            FileInfo[] fiJSONSave = diJSONSave.GetFiles();
            Console.WriteLine("\nExisting files: ");
            foreach (FileInfo fileInfo in fiJSONSave)   //Вывод списка имеющихся файлов
            {
                if (fileInfo.Name.Contains(".json"))
                    Console.WriteLine(fileInfo.Name);
            }
            Console.Write("\nEnter file name: ");
            string fileNameSaveJS = Console.ReadLine();
            string pathToSaveJS = $"{fileNameSaveJS}.json";           
            foreach (FileInfo f in fiJSONSave)  //Если файл с таким именем уже существует, то имя редактируется
            {
                if (f.Name == pathToSaveJS)
                {
                    char charForOverwrite;
                    Console.WriteLine("A file with the same name already exists. Overwrite? (y/n): ");
                    charForOverwrite = Console.ReadKey().KeyChar;
                    if(charForOverwrite!='y')
                        pathToSaveJS = $"{fileNameSaveJS} (COPY_{DateTime.Now.ToLongTimeString()}).json".Replace(':', '_');
                }
            }
            string json;
            try
            {
                json = JsonConvert.SerializeObject(departments);
                File.WriteAllText(pathToSaveJS, json);
                Console.Write($"File {pathToSaveJS} successfully created.\nPress Enter to continiue.");
            }
            catch (Exception e) //Если не получилось, то ошибка
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Failed to create file");
            }
            Console.ReadKey();            
        }
       
        /// <summary>
        /// Метод для импорта департаментов из XML
        /// </summary>
        /// <param name="departmentsReserve">Ссылка на департаменты, понадобится в случае неудачного импорта</param>
        /// <param name="departments">Ссылка не департамент для перезаписи</param>
        /// <param name="common">Общий департамент</param>
        public static void ImportFromXML(ref List<Department> departmentsReserve,out List<Department> departments, ref Department common)
        {
            List<Department> reserve = departmentsReserve;
            DirectoryInfo diXMLOpen = new DirectoryInfo(Environment.CurrentDirectory);  //Директория приложения
            FileInfo[] fiXMLOpen = diXMLOpen.GetFiles();
            Console.WriteLine("\nExisting XML files: ");
            foreach (FileInfo fileInfo in fiXMLOpen)    //Список имеющихся файлов
            {
                if (fileInfo.Name.Contains(".xml"))
                    Console.WriteLine(fileInfo.Name.Remove(fileInfo.Name.Length-4));
            }
            Console.Write("\nFile name: ");
            string xmlFileNameOpen = Console.ReadLine();
            string pathToOpenXML = $"{xmlFileNameOpen}.xml";
            bool xmlOpen = false;
            foreach (FileInfo f in fiXMLOpen)   //Проверка наличия файла
            {
                if (f.Name == pathToOpenXML) xmlOpen = true;
            }
            if (xmlOpen)    //Если файл найден, то десериализуем
            {
                try
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Department>));
                    using (Stream fStream = new FileStream(pathToOpenXML, FileMode.Open, FileAccess.Read))
                    {
                        departments = xmlSerializer.Deserialize(fStream) as List<Department> ?? default!;
                        foreach (Department d in departments)
                        {
                            foreach (Worker w in d.workers)
                            {
                                common.workers.Add(w);
                            }
                        }
                        common.NumbOfWorkers = common.workers.Count;
                    }
                    
                    Console.Write($"File {pathToOpenXML} successfully opened!\nPress Enter to continiue.");
                    Console.ReadKey();
                }
                catch (Exception e) //В случае неудачи - возвращаю департаменты и вывод ошибки 
                {
                    departments = reserve;
                    foreach (Department d in reserve)
                    {
                        foreach (Worker w in d.workers)
                        {
                            common.workers.Add(w);
                        }
                    }
                    Console.WriteLine(e.Message);
                    Console.WriteLine("failed to upload file");
                }
            }
            else    //Иначе сообщаем, что файл не найден и возвращаем департаменты
            {
                departments = reserve;
                foreach (Department d in reserve)
                {
                    foreach (Worker w in d.workers)
                    {
                        common.workers.Add(w);
                    }
                }
                Console.WriteLine($"File {pathToOpenXML} not found.");
                Console.ReadKey();
            }           
        }
        
        /// <summary>
        /// Метод для импорта департаментов из JSON
        /// </summary>
        /// <param name="departments1">Ссылка на департаменты, понадобится в случае неудачного импорта</param>
        /// <param name="departments">Ссылка не департамент для перезаписи</param>
        /// <param name="common">Общий департамент</param>
        public static void ImportFromJSON(ref List<Department> departmentsReserve ,out List<Department> departments, ref Department common)
        {
            List<Department> reserve = departmentsReserve;
            DirectoryInfo diJSONOpen = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] fiJSONOpen = diJSONOpen.GetFiles();
            Console.WriteLine("\nExisting JSON files: ");
            foreach (FileInfo fileInfo in fiJSONOpen)    //Список имеющихся файлов
            {
                if (fileInfo.Name.Contains(".json"))
                    Console.WriteLine(fileInfo.Name.Remove(fileInfo.Name.Length - 5));
            }
            Console.Write("\nFile name: ");
            string jsonfileNameOpen = Console.ReadLine();
            string pathToOpenJSON = $"{jsonfileNameOpen}.json";
            bool jsonOpen = false;
            
            foreach (FileInfo f in fiJSONOpen)  
            {
                if (f.Name == pathToOpenJSON) 
                    jsonOpen = true;
            }
            if (jsonOpen) //Если файл найден, то десериализуем
            {
                try
                {
                    string json = File.ReadAllText(pathToOpenJSON);
                    departments = JsonConvert.DeserializeObject<List<Department>>(json) ?? default!;
                    foreach (Department d in departments)
                    {
                        foreach (Worker w in d.workers)
                        {
                            common.workers.Add(w);
                        }
                    }
                    common.NumbOfWorkers = common.workers.Count;
                    Console.Write($"File {pathToOpenJSON} successfully opened!\nPress Enter to continiue.");
                    Console.ReadKey();
                }
                catch(Exception e)
                {
                    departments = reserve;
                    foreach (Department d in reserve)
                    {
                        foreach (Worker w in d.workers)
                        {
                            common.workers.Add(w);
                        }
                    }
                    Console.WriteLine(e.Message);
                    Console.WriteLine("failed to upload file");
                }
            }
            else //Иначе сообщаем, что файл не найден и возвращаем департаменты
            {
                departments = reserve;
                foreach (Department d in reserve)
                {
                    foreach (Worker w in d.workers)
                    {
                        common.workers.Add(w);
                    }
                }
                Console.WriteLine($"File {pathToOpenJSON} not found.");
                Console.ReadKey();                
            }            
        }        
    }
#nullable restore
}
