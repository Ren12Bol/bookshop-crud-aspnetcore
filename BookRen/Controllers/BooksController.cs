using BookRen.Data;
using BookRen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookRen.Controllers
{
    public class BooksController : Controller 
    {
        private readonly BookRenContext _context;

        public BooksController(BookRenContext context)
        {
            _context = context;
        }

        //GET: Books
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Book == null)
            {
                return Problem("Entity set BookRenContext.Book is null!");
            }

            var books = from b in _context.Book select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(
                    x => x.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(await books.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(b => b.Id == id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        //GET: Book/Create
        public IActionResult Create()
        {
            return View();
        }

        //POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Title,Description,Author,Publisher,ReleaseDate,Genre")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new {id = book.Id});
            }

            return View(book);
        }

        //GET: Book/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        //POST: Book/Edit/id
        [HttpPost]
        public async Task<IActionResult> Edit(int id, 
            [Bind("Id,Title,Description,Author,Publisher,ReleaseDate,Genre")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(id))
                        return NotFound();
                    else 
                        throw;
                }
            }
            return RedirectToAction(nameof(Details), new { id = book.Id });
        }

        //GET: Book/Delete/id
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);

            if (book == null)
            {
                return (NotFound());
            }

            return View(book);
        }

        //POST: Book/Delete/id
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteDone(int id)
        {
            var book = await _context.Book.FindAsync(id);

            if (book != null)
            {
                _context.Book.Remove(book);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
