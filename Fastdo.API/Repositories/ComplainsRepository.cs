﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fastdo.Core.Models;
using System.Threading.Tasks;
using Fastdo.Core.Repositories;
using AutoMapper;

namespace Fastdo.API.Repositories
{
    public class ComplainsRepository : Repository<Complain>, IComplainsRepository
    {
        public ComplainsRepository(SysDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<bool> ComplainExists(Guid id)
        {
            return await GetAll().AnyAsync(e => e.Id == id);
        }

        public async Task<Complain> Delete(Guid id)
        {
            var complain = await GetByIdAsync(id);
            if (complain!=null)
            {
                Remove(complain);
                await SaveAsync();
            }
            return complain;
        }

        public async Task Update(Complain complain)
        {
            _context.Entry(complain).State = EntityState.Modified;
             await SaveAsync();
        }

       
    }
}
