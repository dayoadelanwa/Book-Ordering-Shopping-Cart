using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        // GET: CoverTypeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CoverTypeController/Edit/5
        public ActionResult Upsert(int? id)
        {
            ProductVM productVM = new() 
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })

            };
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList = CategoryList;
                //ViewBag.CoverTypeList = CoverTypeList;
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                //update product
                
            }
            return View(productVM);
        }

        // POST: CoverTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upsert(ProductVM obj, IFormFile file)
        {
            try
            {
                //update item if not null and return to index page
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if(file != null) 
                    {
                        string filename = Guid.NewGuid().ToString();
                        var uploads = Path.Combine(wwwRootPath, @"images\products");
                        var extension = Path.GetExtension(file.FileName);

                        if(obj.Product.ImageUrl!= null) 
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                            if(System.IO.File.Exists(oldImagePath)) 
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStreams = new FileStream(Path.Combine(uploads, filename+extension), FileMode.Create))
                        {
                            file.CopyTo(fileStreams);
                        }
                        obj.Product.ImageUrl = @"\images\products\" + filename + extension;
                    }
                    if(obj.Product.Id == 0)
                    {
                        _unitOfWork.Product.Add(obj.Product);
                    }
                    else
                    {
                        _unitOfWork.Product.Update(obj.Product);
                    }
                    //_unitOfWork.Product.Add(obj.Product);
                    _unitOfWork.Save();
                    TempData["success"] = "Product added successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(obj);
                }
            }
            catch
            {
                return View();
            }
        }

        //// GET: CoverTypeController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    var objProduct = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
        //    if (objProduct == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(objProduct);
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }

        // POST: CoverTypeController/Delete/5
        [HttpDelete]
        public ActionResult Delete(int? id)
        {
            try
            {
                var objProduct = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                if (objProduct == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objProduct.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Product.Remove(objProduct);
                _unitOfWork.Save();
                return Json(new { success = true, message = "Delete successful" });
            }
            catch
            {
                return View();
            }
        }


        #endregion
    }
}
