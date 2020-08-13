using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreDemoApp.Models
{
    class Author : User
    {
        public ICollection<AuthorBook> AuthorBooks { get; set; }

        public Author()
        {
            AuthorBooks = new List<AuthorBook>();
        }
    }
}
