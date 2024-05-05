using CompaniesWebApp.Models;
using CompaniesWebApp.Models.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Serialization;

namespace CompaniesWebApp.Controllers
{
    public class HomeController(ApplicationDbContext db, ILogger<HomeController> logger) : Controller
    {
        private readonly ApplicationDbContext _db = db;
        private readonly ILogger<HomeController> _logger = logger;

        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var companies = await _db.Companies
                .Include(c => c.Departments)
                .ThenInclude(d => d.Divisions)
                .ToListAsync();

            return View(companies);
        }

        [HttpPost]
        [Route("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany([FromForm] Guid id) => await RemoveEntityFromDB<Company>(id);

        [HttpPost]
        [Route("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment([FromForm] Guid id) => await RemoveEntityFromDB<Department>(id);

        [HttpPost]
        [Route("DeleteDivision")]
        public async Task<IActionResult> DeleteDivision([FromForm] Guid id) => await RemoveEntityFromDB<Division>(id);

        private async Task<IActionResult> RemoveEntityFromDB<T>(Guid id)
        {
            var entity = await _db.FindAsync(typeof(T), id);
            if (entity == null)
                return NotFound();

            try
            {
                _db.Remove(entity);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }          
        }

        [HttpGet]
        [Route("ExportCompaniesXML")]
        public async Task<FileContentResult> ExportCompaniesXML()
        {
            var companies = await _db.Companies
                .Include(c => c.Departments)
                .ThenInclude(d => d.Divisions)
                .ToListAsync();

            var serializer = new XmlSerializer(typeof(List<Company>), new XmlRootAttribute("Companies"));

            using var ms = new MemoryStream();
            serializer.Serialize(ms, companies);
            return File(ms.ToArray(), "application/xml", "companies.xml");
        }

        [HttpPost]
        [Route("ImportCompaniesXML")]
        public async Task<IActionResult> ImportCompaniesXML(IFormFile companiesFile)
        {
            if (companiesFile?.ContentType != "text/xml" && companiesFile?.ContentType != "application/xml")
                return BadRequest();

            var serializer = new XmlSerializer(typeof(List<Company>), new XmlRootAttribute("Companies"));

            using var companiesData = companiesFile.OpenReadStream();
            try
            {
                var companies = (List<Company>)serializer.Deserialize(companiesData);
                await _db.Companies.AddRangeAsync(companies);
                await _db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
