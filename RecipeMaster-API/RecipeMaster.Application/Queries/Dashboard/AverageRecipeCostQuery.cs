﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeMaster.Application.Queries.Dashboard
{
    public class AverageRecipeCostQuery : IRequest<decimal>
    {
    }
}
