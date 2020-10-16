using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhereWeBoutToEatApp.Shared
{
    public class AspNetUserSearch
    {
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string IdUser { get; set; }

        public string Search { get; set; }

        public int IdSearchType { get; set; }
    }
}
