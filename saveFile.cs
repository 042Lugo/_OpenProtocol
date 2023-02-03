using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IHM_1v0
{
    public class saveFile
    {
        public static async Task toFile(string IP, string port, string name, string usingDB, string dbAddress, string user, string password)
        {
            string telegram = $"IP=>" + IP +
                                 $"\nPORT=>" + port +
                                 $"\nNAME=>" + name +
                                 $"\nUSING_DB=>" + usingDB +
                                 $"\nDB_ADDRESS=>" + dbAddress +
                                 $"\nUSER=>" + user +
                                 $"\nPASSWORD=>" + password;
            using StreamWriter file = new("setup.op");           
                
            await file.WriteLineAsync(telegram);
                
            
        }
    }
}
