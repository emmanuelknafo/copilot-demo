using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Data;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodoController> _logger;

        public TodoController(TodoContext context, ILogger<TodoController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Todo
        public async Task<IActionResult> Index(string? filter = null, Priority? priorityFilter = null)
        {
            try
            {
                var query = _context.TodoItems.AsQueryable();

                // Apply filters
                if (!string.IsNullOrEmpty(filter))
                {
                    switch (filter.ToLower())
                    {
                        case "completed":
                            query = query.Where(t => t.IsCompleted);
                            break;
                        case "pending":
                            query = query.Where(t => !t.IsCompleted);
                            break;
                    }
                }

                if (priorityFilter.HasValue)
                {
                    query = query.Where(t => t.Priority == priorityFilter);
                }

                // Order by priority and creation date
                var todoItems = await query
                    .OrderByDescending(t => t.Priority)
                    .ThenByDescending(t => t.CreatedAt)
                    .ToListAsync();

                ViewBag.Filter = filter;
                ViewBag.PriorityFilter = priorityFilter;
                return View(todoItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving todo items");
                return View("Error");
            }
        }

        // GET: Todo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var todoItem = await _context.TodoItems
                    .FirstOrDefaultAsync(m => m.Id == id);
                
                if (todoItem == null)
                {
                    return NotFound();
                }

                return View(todoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving todo item with id {Id}", id);
                return View("Error");
            }
        }

        // GET: Todo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Priority")] TodoItem todoItem)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    todoItem.CreatedAt = DateTime.UtcNow;
                    _context.Add(todoItem);
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Created new todo item with id {Id}", todoItem.Id);
                    TempData["SuccessMessage"] = "Todo item created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating todo item");
                    ModelState.AddModelError("", "An error occurred while creating the todo item.");
                }
            }
            return View(todoItem);
        }

        // GET: Todo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return NotFound();
                }
                return View(todoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving todo item for edit with id {Id}", id);
                return View("Error");
            }
        }

        // POST: Todo/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,IsCompleted,Priority,CreatedAt,CompletedAt")] TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update completion status and timestamp
                    if (todoItem.IsCompleted && todoItem.CompletedAt == null)
                    {
                        todoItem.CompletedAt = DateTime.UtcNow;
                    }
                    else if (!todoItem.IsCompleted)
                    {
                        todoItem.CompletedAt = null;
                    }

                    _context.Update(todoItem);
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Updated todo item with id {Id}", todoItem.Id);
                    TempData["SuccessMessage"] = "Todo item updated successfully!";
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TodoItemExists(todoItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "Concurrency error updating todo item with id {Id}", id);
                        ModelState.AddModelError("", "The item was modified by another user. Please refresh and try again.");
                        return View(todoItem);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating todo item with id {Id}", id);
                    ModelState.AddModelError("", "An error occurred while updating the todo item.");
                    return View(todoItem);
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todoItem);
        }

        // GET: Todo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var todoItem = await _context.TodoItems
                    .FirstOrDefaultAsync(m => m.Id == id);
                
                if (todoItem == null)
                {
                    return NotFound();
                }

                return View(todoItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving todo item for delete with id {Id}", id);
                return View("Error");
            }
        }

        // POST: Todo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem != null)
                {
                    _context.TodoItems.Remove(todoItem);
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Deleted todo item with id {Id}", id);
                    TempData["SuccessMessage"] = "Todo item deleted successfully!";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo item with id {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while deleting the todo item.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Todo/ToggleComplete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            try
            {
                var todoItem = await _context.TodoItems.FindAsync(id);
                if (todoItem == null)
                {
                    return NotFound();
                }

                todoItem.IsCompleted = !todoItem.IsCompleted;
                todoItem.CompletedAt = todoItem.IsCompleted ? DateTime.UtcNow : null;

                _context.Update(todoItem);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Toggled completion status for todo item with id {Id}", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error toggling completion for todo item with id {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while updating the todo item.";
                return RedirectToAction(nameof(Index));
            }
        }

        private bool TodoItemExists(int id)
        {
            return _context.TodoItems.Any(e => e.Id == id);
        }
    }
}
