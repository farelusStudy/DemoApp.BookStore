using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDemoApp.Models
{
    class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BookBoxofficeId { get; set; }
        public string Discriminator { get; }

        public ICollection<AuthorBook> AuthorBooks { get; set; }
        public BookBoxoffice Boxoffice { get; set; }
    }
}
