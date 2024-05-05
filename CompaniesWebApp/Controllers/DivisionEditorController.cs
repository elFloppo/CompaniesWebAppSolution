using CompaniesWebApp.Models;
using CompaniesWebApp.Models.DatabaseModels;
using CompaniesWebApp.Models.ViewModels.EditorModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CompaniesWebApp.Controllers
{
    public class DivisionEditorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<DivisionEditorController> _logger;

        public DivisionEditorController(ApplicationDbContext db, ILogger<DivisionEditorController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        [Route("DivisionEditor/Create")]
        public async Task<IActionResult> Create(Guid departmentId)
        {
            ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name");

            return View(new DivisionCreationModel { DepartmentId = departmentId });
        }

        [HttpPost]
        [Route("DivisionEditor/Create")]
        public async Task<IActionResult> Create([FromForm] DivisionCreationModel divisionModel)
        {
            if (_db.Departments.Find(divisionModel?.DepartmentId) == null)
                return NotFound();

            try
            {
                var division = new Division(divisionModel.Name, divisionModel.DepartmentId.Value);
                _db.Divisions.Add(division);
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
        [Route("DivisionEditor/Edit")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var division = await _db.Divisions.FindAsync(id);
            if (division == null)
                return NotFound();

            ViewBag.Departments = new SelectList(await _db.Departments.ToListAsync(), "Id", "Name");

            return View(division);
        }

        [HttpPost]
        [Route("DivisionEditor/Edit")]
        public async Task<IActionResult> Edit([FromForm] Division division)
        {
            try
            {
                _db.Divisions.Update(division);
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
