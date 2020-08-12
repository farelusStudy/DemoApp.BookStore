using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFCoreDemoApp.Models
{
    class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SurnameName { get; set; }
        public string Discriminator { get; set; }

        public string FullName { get => FirstName + " " + SurnameName; }
    }
}
