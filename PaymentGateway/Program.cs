using Abstractions;
using PaymentGateway.Application.WriteOperations;
using PaymentGateway.Data;
using PaymentGateway.ExternalService;
using PaymentGateway.Models;
using PaymentGateway.PublishedLanguage.WriteSide;
using System;
using PaymentGateway.Application.ReadOperations;
using PaymentGateway.PublishedLanguage.Events;
using System.Collections.Generic;

namespace PaymentGateway
{
    class Program
    {
        static void Main(string[] args)
        {
            var account = new BankAccount();
            account.Balance = 100;
            Console.WriteLine(account.Balance);

            var enrollCustomer = new EnrollCustomer();
            enrollCustomer.ClientType = "Company";
            enrollCustomer.AccountType = "Debit";
            enrollCustomer.Name = "Gigi Popa";
            enrollCustomer.Valuta = "Eur";
            enrollCustomer.UniqueIdentifier = "23";
            var enrollCustomerOperation = new EnrollCustomerOperation(new EventSender());
            enrollCustomerOperation.PerformOperation(enrollCustomer);

            var makeAccountDetails = new MakeNewAccount();
            makeAccountDetails.UniqueIdentifier = "23";
            makeAccountDetails.AccountType = "Debit";
            makeAccountDetails.Valuta = "Eur";
            var makeAccountOperation = new CreateAccount(new EventSender());
            makeAccountOperation.PerformOperation(makeAccountDetails);


            var database = Database.GetInstance();
            //foreach (var item in database.BankAccounts)
            //{
            //    Console.WriteLine(item.Iban); 
            //}





            var depositDetails = new MakeNewDeposit();
            depositDetails.Iban =(Int64.Parse( NewIban.GetNewIban())-1).ToString();
            depositDetails.Cnp = "23";
            depositDetails.Currency = "Eur";
            depositDetails.Amount = 750;

            var makeDeposit = new DepositMoney(new EventSender());
            makeDeposit.PerformOperation(depositDetails);



            var withdrawDetails = new MakeWithdraw();
            withdrawDetails.Amount = 150;
            withdrawDetails.Cnp = "23";
            withdrawDetails.Iban= (Int64.Parse(NewIban.GetNewIban()) - 1).ToString();

            var makeWithdraw = new WithdrawMoney(new EventSender());
            makeWithdraw.PerformOperation(withdrawDetails);

            var produs = new Product();
            produs.ID = 1;
            produs.Limit = 10;
            produs.Name = "Pantofi";
            produs.Currency = "Eur";
            produs.Value = 10;

            var produs1 = new Product();
            produs1.ID = 2;
            produs1.Limit = 5;
            produs1.Name = "pantaloni";
            produs1.Currency = "Eur";
            produs1.Value = 5;

            var produs2 = new Product();
            produs2.ID = 3;
            produs2.Limit = 3;
            produs2.Name = "Camasa";
            produs2.Currency = "Eur";
            produs2.Value = 3;

            database.Products.Add(produs);
            database.Products.Add(produs1);
            database.Products.Add(produs2);

            var listaProduse = new List<CommandDetails>();

            var prodCmd1 = new CommandDetails();
            prodCmd1.idProd = 1;
            prodCmd1.Quantity = 2;
            listaProduse.Add(prodCmd1);

            var prodCmd2 = new CommandDetails();
            prodCmd2.idProd = 2;
            prodCmd2.Quantity = 3;
            listaProduse.Add(prodCmd2);

            var comanda = new Command();
            comanda.Details = listaProduse;
            comanda.Iban = (Int64.Parse(NewIban.GetNewIban()) - 1).ToString();



        }
    }
}

/*
 */