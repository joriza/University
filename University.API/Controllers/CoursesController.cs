using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using University.BL.Data;
using University.BL.DTOs;
using University.BL.Repositories.Implements;
using University.BL.Services.Implements;

namespace University.API.Controllers
{
    public class CoursesController : ApiController
    {
        private IMapper mapper;
        private readonly CourseService courseService = new CourseService(new CourseRepository(UniversityContext.Create()));

        public CoursesController()
        {
            this.mapper = WebApiApplication.MapperConfiguration.CreateMapper();
        }
 
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var courses = await courseService.GetAll();
            var coursesDTO = courses.Select(x => mapper.Map<CourseDTO>(x));
            
            return Ok(coursesDTO);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById(int id)
        {
            var course = await courseService.GetById(id);

            if (course == null)
                return NotFound();

            var courseDTO = mapper.Map<CourseDTO>(course);
            
            return Ok(courseDTO);
        }
    }
}
//8.5