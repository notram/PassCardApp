using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PassCardApp.Data;
using PassCardApp.Models;

namespace PassCardApp.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
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

            if (result.Count == 1)
            {
                return RedirectToAction(nameof(OverviewClient), new { ClientId = result.FirstOrDefault().ClientId });
            }

            return View("Index",result);
        }


        public  IActionResult OverviewClient(int? ClientId)
        {
            if (ClientId == null)
            {
                return View("Index", null);
            }
            var tickets = _context.Tickets.Select(t => t).Where(t => t.ClientId == ClientId);
            //
            var ttt = tickets.Include(x=>x.Client).Include(x=>x.SoldByUser).Include(x => x.TicketType).Include(x => x.Checkins).ToList();

            var ClietnName = _context.Clients.Where(c => c.ClientId == ClientId).Select(c => c.Name).FirstOrDefault();

            ttt.Sort( Comparer<Ticket>.Create((i1, i2) => i1.Status.CompareTo(i2.Status)));

            ViewBag.IsAlreadYcheckedIn = false;
            foreach (var item in ttt)
            {
                if (item.CanCheckOut)
                {
                    ViewBag.IsAlreadYcheckedIn = true;
                }
            }
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
            t.SoldAt = DateTime.Now;


            if (ModelState.IsValid)
            {
                _context.Add(t);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(OverviewClient), new { ClientId = t.ClientId });
            }


            return View("ConfigureTicket", t);
        }

        [HttpPost]
        public async Task<IActionResult> CheckIn(int clientId, int ticketId)
        {
            var ticket = _context.Tickets.Where(t => t.TicketId == ticketId).Select(t => t).Include(x => x.TicketType).Include(x => x.Checkins).FirstOrDefault();
            if (!ticket.CanCheckIn)
            {
                //TODO: Return an error
                return RedirectToAction(nameof(OverviewClient), new { ClientId = clientId });
            }

            Checkin c = new Checkin();
            c.CheckedInById = User.FindFirstValue(ClaimTypes.NameIdentifier);
            c.ChecinDate = DateTime.Now;
            c.TicketId = ticket.TicketId;
            _context.Add(c);
            await _context.SaveChangesAsync();
            //TODO: return to success or fail or statisc page
            return RedirectToAction(nameof(OverviewClient), new { ClientId = clientId });
            //var ttt = tickets.Include(x => x.Client).Include(x => x.SoldByUser).Include(x => x.TicketType).ToList();

        }
        public async Task<IActionResult> CheckOut(int clientId, int ticketId)
        {
            var ticket = _context.Tickets.Where(t => t.TicketId == ticketId).Select(t => t).Include(x => x.TicketType).Include(x => x.Checkins).FirstOrDefault();
            if (!ticket.CanCheckOut)
            {
                //TODO: Return an error
                return RedirectToAction(nameof(OverviewClient), new { ClientId = clientId });
            }
            var c = ticket.Checkins.Where(t => t.CheckOutDate == null).FirstOrDefault();
            c.CheckOutDate = DateTime.Now;
            c.CheckedOutById = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.Update(c);

            await _context.SaveChangesAsync();
            //TODO: return to success or fail or statisc page
            return RedirectToAction(nameof(OverviewClient), new { ClientId = clientId });
            //var ttt = tickets.Include(x => x.Client).Include(x => x.SoldByUser).Include(x => x.TicketType).ToList();

        }

    }
}