﻿using DAL.Context;

namespace DAL.UnitOfWork
{
    internal class UnitOfWork(ApplicationContext context) : IUnitOfWork
    {
        private readonly ApplicationContext _context = context;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
