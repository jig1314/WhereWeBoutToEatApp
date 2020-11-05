using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WhereWeBoutToEatApp.Shared
{
    public class AspNetUserRecipeTag
    {
        public long Id { get; set; }

        [Column(TypeName = "nvarchar(450)")]
        public string IdUser { get; set; }

        public int IdRecipeTag { get; set; }
    }
}
