using IPCViewer.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPCViewer.Api.Data
{
    public class CameraRepository : GenericRepository<Camera>, ICameraRepository
    {
        private readonly DataContext context;

        public CameraRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return this.context.Cameras.Include(p => p.User).OrderBy(p => p.Name);

        }
    }
}
