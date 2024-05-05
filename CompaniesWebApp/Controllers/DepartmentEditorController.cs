using CompaniesWebApp.Models;
using CompaniesWebApp.Models.DatabaseModels;
using CompaniesWebApp.Models.ViewModels.EditorModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompaniesWebApp.Controllers
{
    public class DepartmentEditorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DepartmentEditorController> _logger;

        public DepartmentEditorController(ApplicationDbContext db, ILogger<DepartmentEditorController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        [Route("DepartmentEditor/Create")]
        public async Task<IActionResult> Create(Guid companyId)
        {
            // Список компаний для выбора
            ViewBag.Companies = new SelectList(await _db.Companies.ToListAsync(), "Id", "Name");

            return View(new DepartmentCreationModel { CompanyId = companyId });
        }

        [HttpPost]
        [Route("DepartmentEditor/Create")]
        public async Task<IActionResult> Create([FromForm] DepartmentCreationModel departmentModel)
        {
            if (_db.Companies.Find(departmentModel?.CompanyId) == null)
                return NotFound();

            try
            {
                var department = new Department(departmentModel.Name, departmentModel.CompanyId.Value);
                _db.Departments.Add(department);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }            
        }

        [HttpGet]
        [Route("DepartmentEditor/Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var department = await _db.Departments.FindAsync(id);
            if (department == null)
                return NotFound();

            // Список компаний для выбора
            ViewBag.Companies = new SelectList(await _db.Companies.ToListAsync(), "Id", "Name");

            return View(department);
        }

        [HttpPost]
        [Route("DepartmentEditor/Edit")]
        public async Task<IActionResult> Edit([FromForm] Department department)
        {
            try 
            {
                _db.Departments.Update(department);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }        
        }
    }
}
