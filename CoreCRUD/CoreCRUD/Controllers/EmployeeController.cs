using CoreCRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUD.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly EmployeeContext _context;

        public EmployeeController(EmployeeContext context)
        {
            _context = context;
        }
        // GET: EmployeeController
        public async Task<IActionResult> Index()
        {

            var employee = await _context.Employees.ToListAsync();

            return View(employee);
        }


   

      

        // GET: EmployeeController/Details/5

        public async Task<IActionResult> AddOrEdit(int? employeeId)
        {
            ViewBag.PageName = employeeId == null ? "Create Employee" : "Edit Employee";
            ViewBag.IsEdit = employeeId == null ? false : true;
            if (employeeId == null)
            {
                return View();
            }
            else
            {
                var employee = await _context.Employees.FindAsync(employeeId);

                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
        }
        //AddOrEdit Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int employeeId, [Bind("EmployeeId,FullName,EmpCode,Position,OfficeLocation")]
Employee employeeData)
        {
            bool IsEmployeeExist = false;

            Employee employee = await _context.Employees.FindAsync(employeeId);

            if (employee != null)
            {
                IsEmployeeExist = true;
            }
            else
            {
                employee = new Employee();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    employee.FullName = employeeData.FullName;
                    employee.EmpCode = employeeData.EmpCode;
                    employee.Position = employeeData.Position;
                    employee.OfficeLocation = employeeData.OfficeLocation;
               

                    if (IsEmployeeExist)
                    {
                        _context.Update(employee);
                    }
                    else
                    {
                        _context.Add(employee);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employeeData);
        }

        public async Task<IActionResult> Details(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == employeeId);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }


        // GET: Employees/Delete/1
        public async Task<IActionResult> Delete(int? employeeId)
        {
            if (employeeId == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FirstOrDefaultAsync(m => m.EmployeeId == employeeId);

            return View(employee);
        }

        // POST: Employees/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int employeeId)
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }







    }
}
