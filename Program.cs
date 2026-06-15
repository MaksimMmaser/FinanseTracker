using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Transactions;

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
    struct Transaction
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
                Console.WriteLine("--3-- Вывести текущий баланс");
                Console.WriteLine("--4-- Выход");
                Console.WriteLine("--Выберите действие--");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1": AddTransaction(); break;
                    case "2": ShowHistory(); break;
                    case "3": ShowBalance(); break;
                    case "4": Exit(); break;
                    default: break;
                }
            }
        }
        static void AddTransaction()
        {
            Console.Clear();
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
                int incomType  =  GetIncomeType("Выберите способ заработка!") ;
                switch (incomType)
                {
                    case 1 : finalCategory = IncomeCategory.Salary.ToString(); break;
                    case 2:  finalCategory  = IncomeCategory.ScholarShip.ToString(); break;
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
            Console.Clear();
            decimal amount = GetDecimal("Введите сумму!");
            Transaction newTransaction = new Transaction(amount, finalCategory, type, DateTime.Now);
            transactions.Add(newTransaction);
            SaveData();
            Console.Clear();
            Console.WriteLine("Запись успешно добавлена!");
            Console.ReadKey();
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
    }
}