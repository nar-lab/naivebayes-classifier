using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NBayes.Models
{
    public class Person
    {
        public int PersonId { get; set; }

        public string Education { get; set; }

        public string Age { get; set; }

        public string Sex{ get; set; }

        public string Acceptence { get; set; }

    }
   
}