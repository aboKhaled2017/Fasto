using AutoMapper;
using AutoMapper.QueryableExtensions;
using EFCore.BulkExtensions;
using Fastdo.API.Services;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
using Fastdo.Core.Utilities;
using Fastdo.Core.ViewModels;
using Fastdo.Core.ViewModels.StockClasseModels;
using Fastdo.Core.ViewModels.Stocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Repositories
{
    public class StockWithClassRepository : Repository<StockWithPharmaClass>, IStockWithClassRepository
    {
        public StockWithClassRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {        
        }
        public StockWithPharmaClass AddNewClass(string newClass)
        {
            if (Any(s => s.StockId == UserId && s.ClassName == newClass))
                throw new Exception("هذا التصنيف موجود بالفعل");
            var _class = new StockWithPharmaClass
            {
                StockId = UserId,
                ClassName = newClass
            };
            Add(_class);
            return _class;
        }
        public void RemoveClass(DeleteStockClassForPharmaModel model, Action<object> SendError = null)
        {
            //if the deleted class isn't exists
            if (!Any(s => s.Id == model.getDeletedClassId))
            {
                SendError?.Invoke(BasicUtility.MakeError(nameof(model.DeletedClassId), "هذا التصنيف غير موجود"));
                return;
            }

            var _deletedClass = GetAll()
                .SingleOrDefault(s => s.Id == model.getDeletedClassId);

            var stkDrugs = new List<StkDrug>();

            //this class has joined pharmacies ,so they will be assigned another existed class
            if (Any(s => s.Id == model.getDeletedClassId && s.PharmaciesOfThatClass.Count > 0))

            {//if class has joined pharmacies
                //get subscibed pharmacies list
                var joinedPharmasOFThisClass = _unitOfWork.PharmacyInStkClassRepository
                    .Where(s => s.StockClassId == _deletedClass.Id)
                    .ToList();
                //get the replaced existed class
                var replacedEntityClass = GetAll()
                    .SingleOrDefault(s => s.StockId == UserId && s.Id == model.getReplaceClassId);


                if (replacedEntityClass == null)
                {
                    SendError?.Invoke(BasicUtility.MakeError(nameof(model.ReplaceClassId), "هذا التصنيف غير موجود"));
                    return;
                }

                joinedPharmasOFThisClass.ForEach(el =>
                {
                    el.StockClassId = replacedEntityClass.Id;
                });
                _unitOfWork.PharmacyInStkClassRepository.UpdateRange(joinedPharmasOFThisClass);
                Save();
                //get all drugs for this stock
                stkDrugs = _unitOfWork.StkDrugsRepository.Where(s => s.StockId == UserId).ToList();

                //performe edit for discount of class
                stkDrugs.ForEach(drug =>
                {
                    drug.Discount = DiscountClassifier<Guid>
                    .ReplaceClassForDiscount(drug.Discount, model.getDeletedClassId, model.getReplaceClassId);
                });

            }
            else
            {
                //get all drugs for this stock
                stkDrugs = _unitOfWork.StkDrugsRepository
                    .Where(s => s.StockId == UserId).ToList();

                //performe edit for discount of class
                stkDrugs.ForEach(drug =>
                {
                    drug.Discount = DiscountClassifier<Guid>
                    .RemoveClassForDiscount(drug.Discount, model.getDeletedClassId);
                });
            }

            var removedStkDrugs = stkDrugs.Where(s => s.Discount == null).ToList();
            var updatedStkDrugs = stkDrugs.Where(s => s.Discount != null).ToList();
            if (removedStkDrugs.Count > 0)
            {
                _unitOfWork.StkDrugsRepository.RemoveRange(removedStkDrugs);
                Save();
            }
            if (updatedStkDrugs.Count > 0)
            _unitOfWork.StkDrugsRepository.UpdateRange(updatedStkDrugs);
             Save();
            //alwys remove this class
            Remove(_deletedClass);
            Save();
        }
        public async Task UpdateClass(UpdateStockClassForPharmaModel model)
        {
            var stocksWithPhClasses = _unitOfWork.StockWithClassRepository.GetAll();
            var entity = await stocksWithPhClasses
                 .SingleOrDefaultAsync(s => s.StockId == UserId && s.ClassName == model.OldClass);
            if (entity == null)
                throw new Exception("هذا التصنيف غير موجود");
            if (stocksWithPhClasses
                .Any(s => s.StockId == UserId && s.ClassName == model.NewClass))
                throw new Exception("هذا التصنيف موجود بالفعل");

            entity.ClassName = model.NewClass;
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync();
        }
        public async Task<List<StockClassWithPharmaCountsModel>> GetStockClassesOfJoinedPharmas(string stockId)
        {
            return await _unitOfWork.StockWithClassRepository
                .Where(s => s.StockId == stockId)
                .Select(s => new StockClassWithPharmaCountsModel
                {
                    Id = s.Id,
                    Name = s.ClassName,
                    Count = s.PharmaciesOfThatClass.Count
                }).ToListAsync();

        }

        public bool HasClassName(string className)
        {
            return Any(e => e.StockId == UserId && e.ClassName == className);
        }

        public async Task<IReadOnlyList<GetStockClassViewModel>> GetStockClasses()
        {
            return await Where(e => e.StockId == UserId)
                .ProjectTo<GetStockClassViewModel>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public int ClassesCount()
        {
            return GetAll().Count(e => e.StockId == UserId);
        }

        public void UpdateClassDiscount(UpdateStockClassDiscountModel model)
        {
            var _class = GetById(model.ClassId);
            if (_class is null) throw new Exception("this class id is not found");
            _class.Discount = model.Discount;
            UpdateFields(_class, e => e.Discount);
        }
    }
}
