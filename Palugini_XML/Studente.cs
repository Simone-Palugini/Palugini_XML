using System;
using System.Collections.Generic;
using System.Text;

namespace Palugini_XML
{
    public class Studente
    {
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public int Presenze { get; set; }

        public override string ToString()
        {
            return $"{Nome} {Cognome}, presenze: {Presenze}";
        }
    }
}
