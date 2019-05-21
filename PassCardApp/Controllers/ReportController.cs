using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using PassCardApp.Data;

namespace PassCardApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GenerateUsersSpreadsheet()
        {

            IWorkbook workbook;

            workbook = new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("Sheet 1");

            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("Username");
            row.CreateCell(1).SetCellValue("Email");
            row.CreateCell(2).SetCellValue("Pass Card Number");
            row.CreateCell(3).SetCellValue("Registered At");
            row.CreateCell(4).SetCellValue("Registerd By");
            rowIndex++;
            foreach (var client in _context.Clients.Include(x=>x.InsertedByUser).ToList())
            {
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(client.Name);
                row.CreateCell(1).SetCellValue(client.Email);
                row.CreateCell(2).SetCellValue(client.PasscardNumber);
                row.CreateCell(3).SetCellValue(client.InsertedAt);
                row.CreateCell(4).SetCellValue(client.InsertedByUser.UserName);
                rowIndex++;
            }

            var exportData = new MemoryStream();
            workbook.Write(exportData);
            var a = exportData.ToArray();
            return File(
                fileContents: a,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "test.xlsx"
            );
        }

        [Authorize(Roles = "Cashier,Admin")]
        public IActionResult GenerateSalesSpreadSheet(DateTime?from, DateTime?until, bool? onlyMySales)
        {

            IWorkbook workbook;

            workbook = new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet("Sheet 1");

            var rowIndex = 0;
            var row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("Ticket Type Name");
            row.CreateCell(1).SetCellValue("Seller");
            row.CreateCell(2).SetCellValue("Buyer");
            row.CreateCell(3).SetCellValue("Time");
            row.CreateCell(4).SetCellValue("Price");
            rowIndex++;
            var totalprice = 0.0;
            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Models.Ticket, Microsoft.AspNetCore.Identity.IdentityUser> tickets;
            if (onlyMySales!=null && onlyMySales == false)
            {
                tickets =_context.Tickets
                .Where(t => t.SoldAt > from && t.SoldAt < until )
                .Include(x => x.Client)
                .Include(x => x.TicketType)
                .Include(t => t.SoldByUser);
            }
            else
            {

                tickets = _context.Tickets
                .Where(t => t.SoldAt > from && t.SoldAt < until && t.SoldById == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Include(x => x.Client)
                .Include(x => x.TicketType)
                .Include(t => t.SoldByUser);
            }
            foreach (var ticket in tickets.ToList())                
            {
                row = sheet.CreateRow(rowIndex);
                row.CreateCell(0).SetCellValue(ticket.TicketType.TicketTypeName);
                row.CreateCell(1).SetCellValue(ticket.SoldByUser.UserName);
                row.CreateCell(2).SetCellValue(ticket.Client.Name);
                row.CreateCell(3).SetCellValue(ticket.SoldAt.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                row.CreateCell(4).SetCellValue(ticket.TicketType.Price);
                rowIndex++;
                totalprice += ticket.TicketType.Price;
            }

            row = sheet.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue("Total");
            row.CreateCell(4).SetCellValue(totalprice);

            var exportData = new MemoryStream();
            workbook.Write(exportData);
            var a = exportData.ToArray();
            return File(
                fileContents: a,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "test.xlsx"
            );
        }
    }
}