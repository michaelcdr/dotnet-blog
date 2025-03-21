﻿using CodingBlog.Services;
using CodingBlog.Models;
using Microsoft.AspNetCore.Mvc;

namespace CodingBlog.Controllers;

public class CategoriaController : Controller
{
    private readonly IBlogApiService _client;

    public CategoriaController(IBlogApiService client)
    {
        _client = client;
    }

    public async Task<IActionResult> Index()
    { 
        var categorias = await _client.ObterCategorias(); 
        return View(categorias);
    }

    public IActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Cadastrar(CategoriaCadastroModel categoria)
    {
        if (!ModelState.IsValid) return View(categoria);

        _client.CriarCategoria(categoria);
        return RedirectToAction("Index");        
    }
}
