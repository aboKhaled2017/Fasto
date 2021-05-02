﻿using Fastdo.API.Repositories;
using Fastdo.Core;
using Fastdo.Core.Models;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.API.Graphql
{

    public class FastdoQuery : ObjectGraphType
    {
        private IUnitOfWork _unitOfWork { get; }
        
    

        public FastdoQuery(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            Add_Stocks_Field();
            Add_StkDrugs_Field();
            Add_StkDrugPackageReq_Field();
        }
    

        private void Add_StkDrugPackageReq_Field()
        {
            var arguments = new QueryArguments(
                new QueryArgument<IdGraphType> {Name="pharmacyId",Description="Id of pharmacy that made the packages"},
                new QueryArgument<IdGraphType> { Name="id",Description="package id"});
            Field<ListGraphType<StkDrgPackageReqType>>(
                "StkDrugspackagesReqs",
                arguments: arguments,
                resolve: ctx =>
                {
                    if (ctx.HasArgument("id"))
                    {
                        Guid id;
                        if (!Guid.TryParse(ctx.GetArgument<string>("id"), out id))
                        {
                            ctx.Errors.Add(new ExecutionError("wrong value for package id"));
                            return null;
                        }
                        return _unitOfWork.StkDrugPackgesReqsRepository.Where(r => r.Id == id);
                    }
                    else if (ctx.HasArgument("pharmacyId"))
                    {
                        var id = ctx.GetArgument<string>("pharmacyId");
                        return _unitOfWork.StkDrugPackgesReqsRepository.Where(r => r.PharmacyId == id);
                    }
                    else
                    {
                        return _unitOfWork.StkDrugPackgesReqsRepository.GetAll();
                    }
                });
        }

        private void Add_Stocks_Field()
        {
            Field<ListGraphType<StockType>>(
                "stocks",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name = "id", Description = "Stock id" }
                ),
                resolve: context =>
                {

                    if (context.HasArgument("id"))
                    {
                        var id = context.GetArgument<string>("id");
                        return _unitOfWork.StockRepository.Where(s => s.CustomerId == id);
                    }
                    else
                    {
                        return _unitOfWork.StockRepository.GetAll();
                    }
                }
            );
        }
        private void Add_StkDrugs_Field()
        {
            Field<StkDrugType>(
               "stkDrug",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id", Description = "StkDrug id" }
               ),
               resolve: context => _unitOfWork.StockRepository.GetById(context.GetArgument<Guid>("id"))
           );
        }
    }

}
