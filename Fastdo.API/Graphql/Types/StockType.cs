using Fastdo.API.Repositories;
using Fastdo.Core.Enums;
using Fastdo.Core.Models;
using Fastdo.Core.Repositories;
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

    public class StockType : ObjectGraphType<Stock>
    {
        public StockType(IStkDrugsRepository stkDrugsRepository, IDataLoaderContextAccessor dataLoader)
        {
            Field(x => x.CustomerId,type:typeof(IdGraphType)).Description("Stock id");
            Field(x => x.Customer.Name).Description("Stock name.");
            Field(x => x.Customer.Address, nullable: true);
            Field(x => x.Customer.AreaId,type:typeof(IdGraphType));
            Field(x => x.OwnerName);
            Field(x => x.MgrName);
            Field(x => x.CommercialRegImgSrc);
            Field(x => x.LicenseImgSrc);
            Field(x => x.Customer.LandlinePhone);
            Field(x => x.Customer.PersPhone);
            Field<StockRequestStatusType>("Status");

            Field<ListGraphType<StkDrugType>>(
                "drugs",
                resolve: context =>
                {
                    var loader = dataLoader.Context.GetOrAddCollectionBatchLoader<string, StkDrug>("load", async (keys) =>
                    {
                        var drugs = await stkDrugsRepository
                        .Where(s => keys.Contains(s.StockId)).ToListAsync();
                        return drugs.ToLookup(s => s.StockId);
                    });
                    return loader.LoadAsync(context.Source.CustomerId);
                }
            ) ;
        }
    }
}
