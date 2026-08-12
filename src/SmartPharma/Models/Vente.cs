using System;
using System.Collections.Generic;

namespace SmartPharma.Models
{
    public class Vente
    {
        public int Id { get; set; }

        public DateTime DateVente { get; set; } = DateTime.Now;

        public decimal MontantTotal { get; set; }

        public List<LigneVente> Lignes { get; set; } = new();
    }
}