using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PassCardApp.Data;
using PassCardApp.Models;

namespace PassCardApp.Controllers
{
    public class SalesController : Controller
    {

        private readonly ApplicationDbContext _context;

        public SalesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string searchterm)
        {
            var v = searchterm;
            if (searchterm == null)
            {
                return View("Index", null);
            }
            var clients = from c in _context.Clients
                          where c.Name.Contains(searchterm) || c.PasscardNumber.Contains(searchterm) || c.Email.Contains(searchterm)
                          select c;
            var result = clients.ToList();

            return View("Index",result);
        }
    }
}