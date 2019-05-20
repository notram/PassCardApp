using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            if (searchterm.Length < 1)
            {
                return View("Index", null);
            }
            var clients = from c in _context.Clients
                          where c.Name.Contains(searchterm) || c.PasscardNumber.Contains(searchterm) || c.Email.Contains(searchterm)
                          select c;
            var result = clients.ToList();

            return View("Index",result);
        }


        public  IActionResult OverviewClient(int? ClientId)
        {
            if (ClientId == null)
            {
                return View("Index", null);
            }
            var tickets = _context.Tickets.Select(t => t).Where(t => t.ClientId == ClientId);

            var ttt = tickets.Include(x=>x.Client).Include(x=>x.SoldByUser).Include(x=>x.TicketType).ToList();

            var ClietnName = _context.Clients.Where(c => c.ClientId == ClientId).Select(c => c.Name).FirstOrDefault();

            ViewBag.ClientName = ClietnName;
            ViewBag.ClientId = ClientId;
            return View(ttt);
        }

        public async Task<IActionResult> ChooseTicket(int? ClientId)
        {
            if (ClientId == null)
            {
                return View("Index");
            }
            var tt = await _context.TicketTypes.ToListAsync();

            var ClietnName = _context.Clients.Where(c => c.ClientId == ClientId).Select(c => c.Name).FirstOrDefault();

            ViewBag.ClientName = ClietnName;
            ViewBag.ClientId = ClientId;
            return View(tt);
        }

        // POST: TicketTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ChooseTicket(int ClientId, int TicketTypeId, DateTime? activeFrom)
        {
            /*ticketType.InsertedAt = DateTime.Now;
            ticketType.InsertedBy = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            User.FindFirstValue(ClaimTypes.Name);
            if (ModelState.IsValid)
            {
                _context.Add(ticketType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }*/
            Ticket t = new Ticket();
            t.ClientId = ClientId;
            t.Client = _context.Clients.Where(c => c.ClientId == ClientId).Select(c => c).FirstOrDefault();
            t.TicketType = _context.TicketTypes.Where(c => c.TicketTypeId == TicketTypeId).Select(c => c).FirstOrDefault();
            t.TicketTypeId = TicketTypeId;
            t.SoldById = User.FindFirstValue(ClaimTypes.NameIdentifier);
            t.SoldByUser = (from u in _context.Users where u.Id == t.SoldById select u).FirstOrDefault();
            t.ActiveFrom = activeFrom ?? DateTime.Today;
            return View("ConfigureTicket", t);
        }



        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmTicket(int ClientId, int TicketTypeId, DateTime activeFrom)
        {
            /*ticketType.InsertedAt = DateTime.Now;
            ticketType.InsertedBy = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            User.FindFirstValue(ClaimTypes.Name);
            if (ModelState.IsValid)
            {
                _context.Add(ticketType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }*/
            Ticket t = new Ticket();


            t.ClientId = ClientId;
            t.Client = _context.Clients.Where(c => c.ClientId == ClientId).Select(c => c).FirstOrDefault();
            t.TicketType = _context.TicketTypes.Where(c => c.TicketTypeId == TicketTypeId).Select(c => c).FirstOrDefault();
            t.TicketTypeId = TicketTypeId;
            t.SoldById = User.FindFirstValue(ClaimTypes.NameIdentifier);
         
            t.SoldByUser = (from u in _context.Users where u.Id == t.SoldById select u).FirstOrDefault();
            t.ActiveFrom = activeFrom;


            if (ModelState.IsValid)
            {
                t.SoldAt = DateTime.Now;
                _context.Add(t);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OverviewClient), new { ClientId = t.ClientId });
            }


            return View("ConfigureTicket", t);
        }

    }
}