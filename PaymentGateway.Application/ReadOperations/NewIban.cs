using PaymentGateway.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.ReadOperations
{
   public class NewIban
    {
        public static string GetNewIban()
        {
            var database = Database.GetInstance();
            var accounts = database.BankAccounts;//.ForEach(acc => acc.Iban);
            //List<String> ibans = accounts.ForEach(e =>  e.Iban);
            
            List<String> ibans=new List<String>();
            foreach(var acc in accounts)
            {
                ibans.Add(acc.Iban);
            }
            if (ibans.ToArray().Count() == 0)
                return "1";
            
            return  (Int64.Parse( ibans.Last())+1).ToString();
            
        }


    }
}
