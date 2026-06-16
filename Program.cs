using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Transactions;
using System.Linq;

namespace ExpenseTracker
{
    enum TransactionType
    {
        Income, // доход
        Expense // расход
    }
    enum IncomeCategory
    {
        Salary,
        ScholarShip,
        Investment
    }
    enum ExpenseCategory
    {
        Products,
        Restaurant,
        Car,
        Wife,
        Children,
        Education,
        Entartaiment
    }
    class Transaction
    {
        public decimal Amount { get; set; }
        public string Category { get; set; }
        public TransactionType Type { get; set; }
        public DateTime Date { get; set; }
        public Transaction(decimal amount, string category, TransactionType type, DateTime date)
        {
            Amount = amount;
            Category = category;
            Type = type;
            Date = date;
        }
    }
    class Program
    {
        static List<Transaction> transactions = new List<Transaction>();
        static bool isRunning = true;
        static string filePath = "transaction.json";
        static void Main()
        {
            try
            {
                LoadData();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            while (isRunning) {
                Console.Clear();
                Console.WriteLine("===Менеджер Личных Финансов===");
                Console.WriteLine("--1-- Добавть Доход/Расход");
                Console.WriteLine("--2-- Показать историю транзакций");
                Console.WriteLine("--3-- Удалить транзакцию");
                Console.WriteLine("--4-- Редактировать транзакцию");
                Console.WriteLine("--5-- Вывести текущий баланс");
                Console.WriteLine("--6-- Выход");
                Console.WriteLine("--Выберите действие--");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddTransaction(); break;
                    case "2": ShowHistory(); break;
                    case "3": DeleteTransaction(); break;
                    case "4":EditTransaction();break;
                    case "5": ShowBalance(); break;
                    case "6": Exit(); break;
                    default: break;
                }
            }
        }
        static void AddTransaction()
        {
            Console.Clear();
            var (type, finalCategory) = GetTransactionTypeCategory();
            Console.Clear();
            decimal amount = GetDecimal("Введите сумму!");
            Transaction newTransaction = new Transaction(amount, finalCategory, type, DateTime.Now);
            transactions.Add(newTransaction);
            SaveData();
            Console.Clear();
            Console.WriteLine("Запись успешно добавлена!");
            Console.ReadKey();
        }
       public static (TransactionType Type, string Category) GetTransactionTypeCategory()
        {
            Console.WriteLine("--- 1 Доход ---");
            Console.WriteLine("--- 2 Расход ---");
            int choice = GetRightType("Выберите тип ");
            TransactionType type = choice == 1 ? TransactionType.Income : TransactionType.Expense;
            string finalCategory = "Неизвестно";
            if (choice == 1)
            {
                Console.Clear();
                Console.WriteLine("\n--- Категории доходов ---");
                Console.WriteLine("1. Зарплата");
                Console.WriteLine("2. Стипендия");
                Console.WriteLine("3. Инвестиции");
                int incomType = GetIncomeType("Выберите способ заработка!");
                switch (incomType)
                {
                    case 1: finalCategory = IncomeCategory.Salary.ToString(); break;
                    case 2: finalCategory = IncomeCategory.ScholarShip.ToString(); break;
                    case 3: finalCategory = IncomeCategory.Investment.ToString(); break;
                }

            }
            else if (choice == 2)
            {
                Console.Clear();
                Console.WriteLine("\n--- Категории расходов ---");
                Console.WriteLine("1. Продукты\n2. Рестораны\n3. Машина\n4. " +
                    "Жена\n5. Дети\n6. Образование\n7. Развлечения");
                int expenseType = GetExpenseType("Выберите способ трат");
                switch (expenseType)
                {
                    case 1: finalCategory = ExpenseCategory.Products.ToString(); break;
                    case 2: finalCategory = ExpenseCategory.Restaurant.ToString(); break;
                    case 3: finalCategory = ExpenseCategory.Car.ToString(); break;
                    case 4: finalCategory = ExpenseCategory.Wife.ToString(); break;
                    case 5: finalCategory = ExpenseCategory.Children.ToString(); break;
                    case 6: finalCategory = ExpenseCategory.Education.ToString(); break;
                    case 7: finalCategory = ExpenseCategory.Entartaiment.ToString(); break;

                }
            }
            return (type, finalCategory);
        }
        static void ShowHistory()
        {
            Console.Clear();
            
            if (transactions.Count > 0) {
                foreach (var transaction in transactions)
                {
                    Console.WriteLine($"-------{ transaction.Category,15} | {transaction.Amount,15} | {transaction.Type,15}-------");
                }
            }
            else
            {
                Console.WriteLine("Список операций пуст");
            }
            
            Console.ReadKey();
        }
        static void ShowBalance()
        {
            Console.Clear();
            Console.WriteLine("=== Текущий баланс ===");
            decimal totalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount);
            decimal totalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount);
            decimal balance = totalIncome - totalExpense;

            Console.WriteLine($"Всего заработано: {totalIncome} руб.");
            Console.WriteLine($"Всего потрачено:  {totalExpense} руб.");
            Console.WriteLine("-------------------------");
            if (balance >= 0)
                Console.ForegroundColor = ConsoleColor.Green;
            else
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($"Итоговый баланс:  {balance} руб.");
            Console.ResetColor();

            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
        static void Exit()
        {
            Console.Clear();
            isRunning = false;
            Console.WriteLine("До свидания!");
            Console.ReadKey();
        }
        static int GetRightType(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if(int.TryParse(input, out int result))
                {
                    if(result == 1 || result == 2)
                    {
                        return result;
                    }
                } Console.WriteLine("Введите правильно значение: либо 1, либо 2!");
            }
        }
        static decimal GetDecimal(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if(decimal.TryParse(input, out decimal result))
                {
                    return result;
                }
                Console.WriteLine("Пожалуйста, введите корректное значение");
            }
        }
        static int GetIncomeType(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if(int.TryParse(input, out int result))
                {
                    if (result >0 && result < 4)
                    {
                        return result;
                    }
                }
                Console.WriteLine("Введите корректное значение");
            }
        }
        static int GetExpenseType(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if(int.TryParse(input, out int result))
                {
                    if(result>0 && result < 8)
                    {
                        return result;
                    }
                    Console.WriteLine("Введите корректное число");
                }
            }

        }
        static void SaveData()
        {
            var option = new JsonSerializerOptions{ WriteIndented = true}; //делает jsonn структурированным
            string jsonString = JsonSerializer.Serialize(transactions, option);
            File.WriteAllText(filePath, jsonString);
        }
        static void LoadData()
        {
            if (File.Exists(filePath)) {
                string jsonString = File.ReadAllText(filePath);
                transactions = JsonSerializer.Deserialize<List<Transaction>>(jsonString);
            }

        }
        static void DeleteTransaction()
        {
            Console.Clear();
            Console.WriteLine("=== Удаление транзакции ===");
            if(transactions.Count == 0)
            {
                Console.WriteLine("Пусто, нету данных для удаления!");
                Console.WriteLine("\nНажмите любую клавишу для возврата...");
                Console.ReadKey();
                return;
            }
            try {
                ShowNumberedList();
                int index = GetInteger("Введите номер транзакции для удаления");
                transactions.RemoveAt(index-1);
                SaveData();
                Console.WriteLine("Транзакция успешно удалена!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Ошибка: неверный номер транзакции.");
            }
            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey();
        }
        static void EditTransaction()
        {
            Console.Clear();
            Console.WriteLine("=== Редактирование транзакции ===");
            if(transactions.Count == 0)
            {
                Console.WriteLine("Список пуст, нету данных для редактирования!");
                Console.WriteLine("\nНажмите любую клавишу для возврата...");
                Console.ReadKey();
                return;
            }
            ShowNumberedList();
            int index = GetInteger("Введите номер транзакции для редактирования");
            var transactionToEdit = transactions[index-1];
            Console.Write($"Новая категория (оставьте пустым для сохранения ' {transactionToEdit.Type} | {transactionToEdit.Category} '): ");
            var (type, finalCategory) = GetTransactionTypeCategory();
            decimal amount = GetDecimal("Новая сумма");
            transactionToEdit.Type = type;
            transactionToEdit.Category = finalCategory;
            transactionToEdit.Amount = amount;
            //transactions[index - 1] = transactionToEdit;
            Console.WriteLine("\n Данные успешно обновлены!");
            SaveData();
            Console.WriteLine("\nНажмите любую клавишу для возврата...");
            Console.ReadKey();

        }
        static void ShowNumberedList()
        {
            for (int i = 0; i < transactions.Count; i++)
            {
                var transaction = transactions[i];
                string sign = transaction.Type == TransactionType.Income ? "+" : "-";
                Console.WriteLine($"{i + 1}. [{transaction.Date.ToString("dd.MM.yyyy")}] {transaction.Category} : {sign} {transaction.Amount} руб.");
            }
        }
        static int GetInteger(string message)
        {
            while (true)
            {
                Console.WriteLine(message);
                string input = Console.ReadLine();
                if(int.TryParse(input, out int result) && result<=transactions.Count)
                {
                    return result;
                }
                Console.WriteLine($"Неверное чило! выберите от {1} до {transactions.Count} !");
            }
        }
    }
}