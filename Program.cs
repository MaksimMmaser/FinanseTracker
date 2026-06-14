using System;
using System.Collections.Generic;

namespace ExpenseTracker
{
    enum TransactionType
    {
        Income, // доход
        Expense // расход
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
        static void Main()
        {
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
            Console.ReadKey();
        }
        static void ShowHistory()
        {
            Console.Clear();
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
    }
}