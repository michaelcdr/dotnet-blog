﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers.Admin;

[Route("admin")]
public class AdminPostController : Controller
{
    [Route("post")]
    public ActionResult Index()
    {
        return View();
    }

    [Route("post/detalhes/{id}")]
    public ActionResult Details(int id)
    {
        return View();
    }

    [Route("post/create")]
    public ActionResult Create()
    {
        return View();
    }

    // POST: PostController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: PostController/Edit/5
    public ActionResult Edit(int id)
    {
        return View();
    }

    // POST: PostController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }

    // GET: PostController/Delete/5
    public ActionResult Delete(int id)
    {
        return View();
    }

    // POST: PostController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(int id, IFormCollection collection)
    {
        try
        {
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return View();
        }
    }
}