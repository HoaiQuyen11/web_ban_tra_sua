﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopManager.Areas.Admin.DAL;
using ShopManager.Areas.Admin.Models;

namespace ShopManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]

    public class CategoryAdminController : Controller
    {
        CategoryAdminDAL categoryAdminDAL = new CategoryAdminDAL();
        // GET: CategoryAdminController
        public ActionResult Index()
        {
            return View(categoryAdminDAL.getAll());
        }

        // GET: CategoryAdminController/Details/5
        public ActionResult Details(int id)
        {
            CategoryAdmin category = new CategoryAdmin();
            category = categoryAdminDAL.getCategoryById(id);
            return View(category);
        }

        // GET: CategoryAdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoryAdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CategoryAdmin categoryNew)
        {
            try
            {
                bool IsInserted = false;
                if(ModelState.IsValid)
                {
                    categoryNew.CreateAt = DateTime.Now;  
                    categoryNew.UpdateAt = DateTime.Now;
                    IsInserted = categoryAdminDAL.AddNew(categoryNew);
                    if(IsInserted) 
                    {
                        TempData["SuccessMessage"] = "Insert Success";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Insert Fail";
                    }    
                }    
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: CategoryAdminController/Edit/5
        public ActionResult Edit(int id)
        {
            CategoryAdmin category = new CategoryAdmin();
            category = categoryAdminDAL.getCategoryById(id);
            return View(category);
        }

        // POST: CategoryAdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryAdmin categoryNew)
        {
            try
            {
                bool IsInserted = false;
                if(ModelState.IsValid)
                {
                    Console.WriteLine("Update Category Id = ", id);
                    categoryNew.CreateAt = DateTime.Now;
                    categoryNew.UpdateAt = DateTime.Now;
                    IsInserted = categoryAdminDAL.updateCategoryById(id, categoryNew);
                    if (IsInserted)
                    {
                        Console.WriteLine("Update Success");
                        TempData["SuccessMessage"] = "Insert Success";
                    }
                    else
                    {
                        Console.WriteLine("Update Fail");
                        TempData["ErrorMessage"] = "Insert Fail";
                    }    
                }    
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Update Error: " + ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: CategoryAdminController/Delete/5
        public ActionResult Delete(int id)
        {
            CategoryAdmin category = new CategoryAdmin();
            category = categoryAdminDAL.getCategoryById(id);
            return View(category);
        }

        // POST: CategoryAdminController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                bool IsInserted = false;
                if (ModelState.IsValid)
                {
                    Console.WriteLine("Update Category Id = ", id);
                    IsInserted = categoryAdminDAL.deleteCategoryById(id);
                    if (IsInserted)
                    {
                        Console.WriteLine("Delete Success");
                        TempData["SuccessMessage"] = "Delete Success";
                    }
                    else
                    {
                        Console.WriteLine("Delete Fail");
                        TempData["ErrorMessage"] = "Delete Fail";
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete error " + ex.Message);
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
