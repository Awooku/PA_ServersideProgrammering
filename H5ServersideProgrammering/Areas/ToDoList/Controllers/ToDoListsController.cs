using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using H5ServersideProgrammering.Areas.TodoList.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using H5ServersideProgrammering.Codes;

namespace H5ServersideProgrammering.Areas.ToDoList.Controllers
{
    [Area("ToDoList")]
    [Route("ToDoList/[controller]/[action]")]
    [Authorize("RequireAuthenticatedUser")]
    public class ToDoListsController : Controller
    {
        private readonly TestContext _context;
        private readonly IDataProtector _dataProtector;
        private readonly CrytpExample _crytpExample;

        public ToDoListsController(TestContext context, IDataProtectionProvider dataProtector, CrytpExample crytpExample)
        {
            _context = context;
            _crytpExample = crytpExample;
            _dataProtector = dataProtector.CreateProtector("H5ServersideProgrammering.MyToDoListsController.NielsOlesen");
        }

        // GET: ToDoList/ToDoLists
        public async Task<IActionResult> Index()
        {
            var userIdentityName = User.Identity.Name;

            var rows = await _context.ToDoLists.Where(s => s.User == userIdentityName).ToListAsync();
            bool matchFound = rows.Count > 0;

            if (matchFound)
            {
                foreach(TodoList.Models.ToDoList row in rows)
                {
                    string encryptedText = row.Description;
                    row.Description = _crytpExample.Decrypt(encryptedText, _dataProtector);
                }
                return View(rows);
            }
            else
            {
                return View(new List<TodoList.Models.ToDoList>());
            }
        }

        // GET: ToDoList/ToDoLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        // GET: ToDoList/ToDoLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToDoList/ToDoLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titel,Description,User")] TodoList.Models.ToDoList toDoList)
        {
            if (ModelState.IsValid)
            {
                string description = toDoList.Description;
                toDoList.Description = _crytpExample.Encrypt(description, _dataProtector);

                _context.Add(toDoList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toDoList);
        }

        // GET: ToDoList/ToDoLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoLists.FindAsync(id);
            if (toDoList == null)
            {
                return NotFound();
            }
            return View(toDoList);
        }

        // POST: ToDoList/ToDoLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titel,Description,User")] TodoList.Models.ToDoList toDoList)
        {
            if (id != toDoList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toDoList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToDoListExists(toDoList.Id))
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
            return View(toDoList);
        }

        // GET: ToDoList/ToDoLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toDoList = await _context.ToDoLists
                .FirstOrDefaultAsync(m => m.Id == id);
            if (toDoList == null)
            {
                return NotFound();
            }

            return View(toDoList);
        }

        // POST: ToDoList/ToDoLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toDoList = await _context.ToDoLists.FindAsync(id);
            _context.ToDoLists.Remove(toDoList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToDoListExists(int id)
        {
            return _context.ToDoLists.Any(e => e.Id == id);
        }
    }
}
