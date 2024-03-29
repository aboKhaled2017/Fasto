﻿using Fastdo.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fastdo.Core;

namespace Fastdo.Core.Repositories
{
    public interface IPharmacyInStkRepository:IRepository<PharmacyInStock>
    {
        void PatchUpdateRequest(PharmacyInStock request);
    }
}
