using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.db;

namespace Shop.GenerateDB
{
    class Program
    {
        static void Main(string[] args)
        {

            DB.GeterateDBScript();
            Console.ReadKey();
        }
    }
}
