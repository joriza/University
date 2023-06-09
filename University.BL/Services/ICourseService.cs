﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;


namespace University.BL.Services
{
    public interface ICourseService : IGenericService<Course>
    {
        Task<bool> DeleteCheckRelatedEntity(int id);
        Course GetByCredit(int id); //
        Task<Course> GetByCreditAsync(int id); //
        //Task<Course> PostByCreditAsync(int id); //
        //Course PostByCredit(int id); //
    }

}
