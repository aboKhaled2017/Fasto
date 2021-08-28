
using Fastdo.Core.Repositories;
using Fastdo.Core.Repository.IRpository;

namespace Fastdo.Core
{
    public interface IUnitOfWork
    {
        IAdminRepository AdminRepository { get; }
        IBaseDrugRepository BaseDrugRepository { get; }
        IAreaRepository AreaRepository { get; }
        IComplainsRepository ComplainsRepository { get; }
        ILzDrgRequestsRepository LzDrgRequestsRepository { get; }
        ILzDrugRepository LzDrugRepository { get; }
        ILzDrg_Search_Repository LzDrg_Search_Repository { get; }
        IPharmacyInStkRepository PharmacyInStkRepository { get; }
        IPharmacyRepository PharmacyRepository { get; }
        IStkDrgInPackagesReqsRepository StkDrgInPackagesReqsRepository { get; }
        IStkDrugPackgesReqsRepository StkDrugPackgesReqsRepository { get; }
        IStkDrugsRepository StkDrugsRepository { get; }
        IStockRepository StockRepository { get; }
        IStockWithClassRepository StockWithClassRepository { get; }
        ITechSupportQRepository TechSupportQRepository { get; }
        bool Save();
    }
}
