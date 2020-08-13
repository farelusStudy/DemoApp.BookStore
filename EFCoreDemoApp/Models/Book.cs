using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Models
{
    class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Discriminator { get; }

        public ICollection<AuthorBook> AuthorBooks { get; set; }
    }
}
