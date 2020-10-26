using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
   public class Mesaj
    {
        public string Gonderen { get; set; }
        public string Mesaji { get; set; }
        public DateTime Gonderim { get; set; }
        public override string ToString()
        {
            return Gonderen + ": (" + Gonderim.ToString() + ") ->" + Mesaji;
        }
    }
}
