using Fastdo.Core;
using Fastdo.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Utilities
{
    public class UserRepoUtility
    {
        public static bool IsValidUserId(IUnitOfWork unit,EUserType type,string userId)
        {
            switch (type)
            {
                case EUserType.Pharmacy:
                    return unit.PharmacyRepository.Any(e => e.Id == userId);
                case EUserType.Stock:
                    return unit.StockRepository.Any(e => e.Id == userId);
                case EUserType.Admin:
                    return unit.AdminRepository.Any(e => e.Id == userId);
                default:
                    return false;
            }
        }
    }
}
