using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculationWorkerRole
{
    class Bottle
    {
        public string userId { get; set; }
        public string message { get; set; }
        public override string ToString()
        {
                return "userId=" + userId + "&message=" + message; 
        }
    }
}
