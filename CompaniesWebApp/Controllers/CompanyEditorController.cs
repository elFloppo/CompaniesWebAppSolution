using CompaniesWebApp.Models;
using CompaniesWebApp.Models.DatabaseModels;
using CompaniesWebApp.Models.ViewModels.EditorModels;
using Microsoft.AspNetCore.Mvc;

namespace CompaniesWebApp.Controllers
{
    public class CompanyEditorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<CompanyEditorController> _logger;

        public CompanyEditorController(ApplicationDbContext db, ILogger<CompanyEditorController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpGet]
        [Route("CompanyEditor/Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("CompanyEditor/Create")]
        public async Task<IActionResult> Create([FromForm] CompanyCreationModel companyModel)
        {
            try
            {
                var company = new Company(companyModel.Name);
                _db.Companies.Add(company);
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
        [Route("CompanyEditor/Edit")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var company = await _db.Companies.FindAsync(Id);

            return View(company);
        }

        [HttpPost]
        [Route("CompanyEditor/Edit")]
        public async Task<IActionResult> Edit([FromForm] Company company)
        {
            try
            {
                _db.Companies.Update(company);
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
