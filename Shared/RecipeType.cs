﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WhereWeBoutToEatApp.Shared
{
    public class RecipeType
    {
        public int ID { get; set; }
     
        public int EnumCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string BaseURL { get; set; }

    }
}
