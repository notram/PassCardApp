using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PassCardApp.Data;
using PassCardApp.Models;

namespace PassCardApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CheckinsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckinsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checkins
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Checkin.Include(c => c.CheckedInBy).Include(c => c.CheckedOutBy).Include(c => c.Ticket);
            return View(await applicationDbContext.ToListAsync());
        }

        

        // GET: Checkins/Details/5
        /*public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkin = await _context.Checkin
                .Include(c => c.CheckedInBy)
                .Include(c => c.CheckedOutBy)
                .Include(c => c.Ticket)
                .FirstOrDefaultAsync(m => m.CheckinId == id);
            if (checkin == null)
            {
                return NotFound();
            }

            return View(checkin);
        }*/

        // GET: Checkins/Create
        /*public IActionResult Create()
        {
            ViewData["CheckedInById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CheckedOutById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId");
            return View();
        }

        // POST: Checkins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CheckinId,TicketId,CheckedInById,ChecinDate,CheckedOutById,CheckOutDate")] Checkin checkin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checkin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CheckedInById"] = new SelectList(_context.Users, "Id", "Id", checkin.CheckedInById);
            ViewData["CheckedOutById"] = new SelectList(_context.Users, "Id", "Id", checkin.CheckedOutById);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", checkin.TicketId);
            return View(checkin);
        }

        // GET: Checkins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkin = await _context.Checkin.FindAsync(id);
            if (checkin == null)
            {
                return NotFound();
            }
            ViewData["CheckedInById"] = new SelectList(_context.Users, "Id", "Id", checkin.CheckedInById);
            ViewData["CheckedOutById"] = new SelectList(_context.Users, "Id", "Id", checkin.CheckedOutById);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", checkin.TicketId);
            return View(checkin);
        }

        // POST: Checkins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CheckinId,TicketId,CheckedInById,ChecinDate,CheckedOutById,CheckOutDate")] Checkin checkin)
        {
            if (id != checkin.CheckinId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(checkin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CheckinExists(checkin.CheckinId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CheckedInById"] = new SelectList(_context.Users, "Id", "Id", checkin.CheckedInById);
            ViewData["CheckedOutById"] = new SelectList(_context.Users, "Id", "Id", checkin.CheckedOutById);
            ViewData["TicketId"] = new SelectList(_context.Tickets, "TicketId", "TicketId", checkin.TicketId);
            return View(checkin);
        }

        // GET: Checkins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checkin = await _context.Checkin
                .Include(c => c.CheckedInBy)
                .Include(c => c.CheckedOutBy)
                .Include(c => c.Ticket)
                .FirstOrDefaultAsync(m => m.CheckinId == id);
            if (checkin == null)
            {
                return NotFound();
            }

            return View(checkin);
        }

        // POST: Checkins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var checkin = await _context.Checkin.FindAsync(id);
            _context.Checkin.Remove(checkin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CheckinExists(int id)
        {
            return _context.Checkin.Any(e => e.CheckinId == id);
        }*/
    }
}
