﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CUnit.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethod : Attribute
    {
    }
}
