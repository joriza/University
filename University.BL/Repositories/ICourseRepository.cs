﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;

namespace University.BL.Repositories
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<bool> DeleteCheckRelatedEntity(int id);
        Course GetByCredit(int credit);
        Task<Course> GetByCreditAsync(int credit);
        //Task<Course> PostByCreditAsync(int credit);

    }
}
