using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDemoApp.Models
{
    /// <summary>
    /// Успешность книги
    /// </summary>
    class BookBoxoffice
    {
        public int Id { get; set; }

        /// <summary>
        /// Рейтинг
        /// </summary>
        public decimal Rating { get; set; }

        /// <summary>
        /// Продажи
        /// </summary>
        public long Sales { get; set; }
        public int BookId { get; set; }

        public Book Book { get; set; }

    }
}
