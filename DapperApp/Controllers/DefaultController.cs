using Dapper;
using DapperApp.DapperContext;
using DapperApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;

namespace DapperApp.Controllers
{
    public class DefaultController : Controller
    {
        private readonly Context _context;

        public DefaultController(Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string query = "Select * From Project";
            var connection = _context.CreateConnection();
            var values = await connection.QueryAsync<ResultProjectDto>(query);
            return View(values.ToList());
        }
        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProject(CreateProjectDto createProjectDto)
        {
            string query = "insert into Project (Title,Description,ProjectCategory,CompleteDay,Price) Values(@title,@description,@projectCategory,@completeDay,@price)";
            var parameters = new DynamicParameters();
            parameters.Add("@title", createProjectDto.Title);
            parameters.Add("@description", createProjectDto.Description);
            parameters.Add("@price", createProjectDto.Price);
            parameters.Add("@projectCategory", createProjectDto.ProjectCategory);
            parameters.Add("@completeDay", createProjectDto.CompleteDay);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> DeleteProject(int id)
        {
            string query = "Delete From Project where ProjectID=@ProjectID";
            var parameters = new DynamicParameters();
            parameters.Add("@ProjectID", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> UpdateProject(int id)
        {
            string query = "Select * From Project Where ProjectID=@ProjectID";
            var parameters = new DynamicParameters();
            parameters.Add("@ProjectID", id);
            var connection = _context.CreateConnection();
            var values = await connection.QueryFirstOrDefaultAsync<UpdateProjectDto>(query,parameters);
            return View(values);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProject(UpdateProjectDto updateProjectDto)
        {
            string query = "Update Project Set Title=@title, Description=@description, Price=@price, ProjectCategory=@projectCategory, CompleteDay=@completeDay where ProjectID=@ProjectID";
            var parameters = new DynamicParameters();
            parameters.Add("@title", updateProjectDto.Title);
            parameters.Add("@description", updateProjectDto.Description);
            parameters.Add("@price", updateProjectDto.Price);
            parameters.Add("@projectCategory", updateProjectDto.ProjectCategory);
            parameters.Add("@projectID", updateProjectDto.ProjectCategory);
            parameters.Add("@completeDay", updateProjectDto.CompleteDay);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
            return RedirectToAction("Index");
        }
    }
}
