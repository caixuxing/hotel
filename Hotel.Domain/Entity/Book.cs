using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hotel.Domain.Entity
{
    public class Book
    {
        public ObjectId Id { get; set; }

        public long No { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }
}
