
using AutoMapper;
using Fastdo.Core;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using Fastdo.Core.Repository.IRpository;
using Fastdo.Core.Services;

namespace Fastdo.API.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SysDbContext context;
        private readonly IMapper mapper;
        private readonly IpropertyMappingService _propertMappingService;

        public UnitOfWork(IpropertyMappingService propertMappingService, SysDbContext context, IMapper _mapper)
        {
            _propertMappingService = propertMappingService;
            this.context = context;
            mapper = _mapper;
        }

        private AdminRepository _AdminRepository;
        private IAreaRepository _AreaRepository;
        private IComplainsRepository _ComplainsRepository;
        private ILzDrgRequestsRepository _LzDrgRequestsRepository;
        private ILzDrugRepository _LzDrugRepository;
        private ILzDrg_Search_Repository _LzDrg_Search_Repository;
        private IPharmacyInStkRepository _PharmacyInStkRepository;
        private IPharmacyRepository _PharmacyRepository;
        private IStkDrgInPackagesReqsRepository _StkDrgInPackagesReqsRepository;
        private IStkDrugPackgesReqsRepository _StkDrugPackgesReqsRepository;
        private IStkDrugsRepository _StkDrugsRepository;
        private IStockRepository _StockRepository;
        private IStockWithClassRepository _StockWithClassRepository;
        private ITechSupportQRepository _TechSupportQRepository;
        private IBaseDrugRepository _baseDrugRepository;
        private ILzDrgRequestExchangeRepository _lzDrgRequestExchangeRepository;

        public IAdminRepository AdminRepository
        {
            get
            {
                if (_AdminRepository is null)
                {
                    _AdminRepository = new AdminRepository(context, mapper);
                    _AdminRepository.SetUnitOfWork(this);
                }
                return _AdminRepository;
            }
        }

        public IAreaRepository AreaRepository
        {
            get
            {
                return _AreaRepository ?? (_AreaRepository = new AreaRepository(context, mapper));
            }
        }

        public IComplainsRepository ComplainsRepository
        {
            get
            {
                return _ComplainsRepository ?? (_ComplainsRepository = new ComplainsRepository(context, mapper));
            }
        }

        public ILzDrgRequestsRepository LzDrgRequestsRepository
        {
            get
            {
                if(_LzDrgRequestsRepository is null)
                {
                    _LzDrgRequestsRepository = new LzDrgRequestsRepository(context, mapper);
                    _LzDrgRequestsRepository.SetUnitOfWork(this);
                }
                return _LzDrgRequestsRepository;
            }
        }

        public ILzDrugRepository LzDrugRepository
        {
            get
            {
                return _LzDrugRepository ?? (_LzDrugRepository = new LzDrugRepository(context, mapper));
            }
        }

        public ILzDrg_Search_Repository LzDrg_Search_Repository
        {
            get
            {
                return _LzDrg_Search_Repository ?? (_LzDrg_Search_Repository = new LzDrg_Search_Repository(context, _propertMappingService, mapper));
            }
        }

       
        public IPharmacyInStkRepository PharmacyInStkRepository
        {
            get
            {
                return _PharmacyInStkRepository ?? (_PharmacyInStkRepository = new PharmacyInStkRepository(context, mapper));
            }
        }

        public IPharmacyRepository PharmacyRepository
        {
            get
            {
                return _PharmacyRepository ?? (_PharmacyRepository=new PharmacyRepository(context, mapper));
            }
        }

        public IStkDrgInPackagesReqsRepository StkDrgInPackagesReqsRepository
        {
            get
            {
                return _StkDrgInPackagesReqsRepository ?? (_StkDrgInPackagesReqsRepository=new StkDrgInPackagesReqs(context, mapper));
            }
        }

        public IStkDrugPackgesReqsRepository StkDrugPackgesReqsRepository
        {
            get
            {
                return _StkDrugPackgesReqsRepository ?? (_StkDrugPackgesReqsRepository=new StkDrugPackagesReqsRepository(context, mapper));
            }
        }

        public IStkDrugsRepository StkDrugsRepository
        {
            get
            {
                if (_StkDrugsRepository is null)
                {
                    _StkDrugsRepository = new StkDrugsRepository(context, mapper);
                    _StkDrugsRepository.SetUnitOfWork(this);
                }
                return _StkDrugsRepository;
            }
        }

        public IStockRepository StockRepository
        {
            get
            {
                if (_StockRepository is null)
                {
                    _StockRepository = new StockRepository(context, mapper);
                    _StockRepository.SetUnitOfWork(this);
                }
                return _StockRepository;
            }
        }

        public IStockWithClassRepository StockWithClassRepository
        {
            get
            {
                if(_StockWithClassRepository is null)
                {
                    _StockWithClassRepository = new StockWithClassRepository(context, mapper);
                    _StockWithClassRepository.SetUnitOfWork(this);
                }
                return _StockWithClassRepository;
            }
        }

        public ITechSupportQRepository TechSupportQRepository
        {
            get
            {
                if(_TechSupportQRepository is null)
                {
                    _TechSupportQRepository = new TechSupportQRepository(context, mapper);
                    _TechSupportQRepository.SetUnitOfWork(this);
                }
                return _TechSupportQRepository;
            }
        }

        public IBaseDrugRepository BaseDrugRepository
        {
            get
            {
                if (_baseDrugRepository is null)
                {
                    _baseDrugRepository = new BaseDrugRepository(context, mapper);
                    _baseDrugRepository.SetUnitOfWork(this);
                }
                return _baseDrugRepository;
            }
        }

        public ILzDrgRequestExchangeRepository lzDrgRequestExchangeRepository
        {
            get
            {
                if (_lzDrgRequestExchangeRepository is null)
                {
                    _lzDrgRequestExchangeRepository = new LzDrgRequestExchangeRepository(context, mapper);
                    _lzDrgRequestExchangeRepository.SetUnitOfWork(this);
                }
                return _lzDrgRequestExchangeRepository;
            }
        }

        public bool Save()
        {
            return context.SaveChanges() > 0;
        }
    }
}
