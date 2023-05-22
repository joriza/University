using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.BL.Models;
using University.BL.DTOs;

namespace University.BL.DTOs
{
    public class MapperConfig
    {
        public static MapperConfiguration MapperConfiguration()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Course, CourseDTO>(); // De un modelo a un DTO - GET
                cfg.CreateMap<CourseDTO, Course>(); // De un DTO a un modelo - PUT

                cfg.CreateMap<Student, StudentDTO>(); // De un modelo a un DTO - GET
                cfg.CreateMap<StudentDTO, Student>(); // De un DTO a un modelo - PUT

                cfg.CreateMap<Enrollment, EnrollmentDTO>(); // De un modelo a un DTO - GET
                cfg.CreateMap<EnrollmentDTO, Enrollment>(); // De un DTO a un modelo - PUT

            });
        }
    }
}
