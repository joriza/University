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
using University.BL.Models;
using University.BL.Repositories;
using University.BL.Repositories.Implements;
using University.BL.Services.Implements;

namespace University.API.Controllers
{
    public class CoursesController : ApiController
    {
        private IMapper mapper;
        private static UniversityContext contex_     = UniversityContext.Create();
        private static ICourseRepository repo_       = new CourseRepository(contex_);
        private readonly CourseService courseService = new CourseService(repo_);
        //private readonly CourseService courseService = new CourseService(new CourseRepository(UniversityContext.Create())); //Todo en una linea, arriba está por separado.

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

        [HttpPost]
        public async Task<IHttpActionResult> Post(CourseDTO courseDTO) // Si el método comienza con el verbo http no es necesario colocar el decorador. No resuelve por el nombre del metodo sino por el tipo de método. Tampoco importa el nombre el método, no entiendo como matchea.
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var course = mapper.Map<Course>(courseDTO); //Comno el servicio debe recibir un modelo, con Mapper se envía courseDTO para recibir el modelo Course
                course = await courseService.Insert(course); //aunque la tabla curso no tiene id auto incremental, si lo fuera, por eso se guarda en una variable lo que devuelve el metodo Insert.
                return Ok(course); // Retorna 200 ok + el objeto que se ha insertado.
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
    }
}
//8.5