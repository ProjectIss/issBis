using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bis.Models
{
    public class shift
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Time)]
        public DateTime StartingTime { get; set; }
        [DataType(DataType.Time)]
        public DateTime EndingTime { get; set; }
        public bool OverTime { get; set; }

        internal static void Add(SelectListItem selectListItem)
        {
            throw new NotImplementedException();
        }
    }
}