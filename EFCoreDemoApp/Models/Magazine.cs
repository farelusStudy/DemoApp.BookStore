using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models
{
    class Magazine : Book
    {
        /// <summary>
        /// Читателький адрес 
        /// (журнал для определенных специалистов (дизайнеров, поваров и т.д.), для массовго читателя)
        /// </summary>
        public string ReaderType { get; set; }
    }
}
