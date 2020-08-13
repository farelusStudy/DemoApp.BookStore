using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models
{
    class ScienceBook : Book
    {
        /// <summary>
        /// Тип научной литературы (диссертация, энциклопедия, словарь, справочник и т.д.)
        /// </summary>
        public string Type { get; set; }
    }
}
