using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        // GET: CoverTypeController
        public ActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        }

        // GET: CoverTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CoverTypeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoverType obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.CoverType.Add(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "Cover Type created successfully";
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

        // GET: CoverTypeController/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var objCoverTypeType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (objCoverTypeType == null)
            {
                return NotFound();
            }
            return View(objCoverTypeType);
        }

        // POST: CoverTypeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CoverType obj)
        {
            try
            {
                //update item if not null and return to index page
                if (ModelState.IsValid)
                {

                    _unitOfWork.CoverType.Update(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "CoverType edited successfully";
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

        // GET: CoverTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            var coverTypeObj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypeObj == null)
            {
                return NotFound();
            }
            return View(coverTypeObj);
        }

        // POST: CoverTypeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? id)
        {
            try
            {
                var coverTypeObj = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
                if (coverTypeObj == null)
                {
                    return NotFound();
                }
                else
                {
                    _unitOfWork.CoverType.Remove(coverTypeObj);
                    _unitOfWork.Save();
                    TempData["success"] = "CoverType deleted successfully";
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
