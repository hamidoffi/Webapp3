using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Oblig1.Model
{
    [ExcludeFromCodeCoverage]
    public class Reise
    {
        public int BussRuteId { get; set; }
        public int RuteId { get; set; }
        public int StasjonId { get; set; }
        public int Pris { get; set; }
        public string BussNavn { get; set; }
       
    }
}
