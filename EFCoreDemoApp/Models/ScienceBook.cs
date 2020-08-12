using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDemoApp.Models
{
    class ScienceBook : Book
    {
        /// <summary>
        /// Жанры научной литературы (диссертация, энциклопедия, словарь, справочник и т.д.)
        /// </summary>
        public string Genre { get; set; }
    }
}
