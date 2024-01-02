using Dapper;
using DapperApp.DapperContext;
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
            var connection=_context.CreateConnection();
            var values= await connection.QueryAsync<>(query);
            return View(values.ToList());
        }
    }
}
