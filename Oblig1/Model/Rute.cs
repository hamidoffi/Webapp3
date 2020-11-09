using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Oblig1.Model
{
    [ExcludeFromCodeCoverage]
    public class Rute
    {
        public string BussNavn { get; set; }
        public int Pris { get; set; }
        public string Avganger { get; set; }
        public string Tider { get; set; }

    }
}
