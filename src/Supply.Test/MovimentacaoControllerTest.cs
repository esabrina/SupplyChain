using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using Supply.Controllers;
using Supply.Models;
using Supply.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Supply.Test
{
    public class MovimentacaoControllerTest : IDisposable
    {
        MovimentacaoController _controller;
        SupplyDBContext _contextTest;

        public MovimentacaoControllerTest()
        {
            var options = new DbContextOptionsBuilder<SupplyDBContext>()
                .UseInMemoryDatabase(databaseName: "TestDB").Options;

            var context = new SupplyDBContext(options);
            if (!context.Mercadorias.Any())
            {
                var data = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
                // Mercadorias
                var m1 = context.Mercadorias.Add(
                    new Mercadoria { Nome = "Merc1", Descricao = "descricao xxx1", Fabricante = "Fabricante xxx", Tipo = "xxx" }).Entity;
                var m2 = context.Mercadorias.Add(
                     new Mercadoria { Nome = "Merc2", Descricao = "descricao xxx2", Fabricante = "Fabricante xxx", Tipo = "xxx" }).Entity;
                var m3 = context.Mercadorias.Add(
                     new Mercadoria { Nome = "Merc3", Descricao = "descricao xxx3", Fabricante = "Fabricante xxx", Tipo = "xxx" }).Entity;
                var m4 = context.Mercadorias.Add(
                    new Mercadoria { Nome = "Merc4", Descricao = "descricao xxx4", Fabricante = "Fabricante xxx", Tipo = "xxx" }).Entity;
                // Movimentacoes
                var mov1 = context.Movimentacoes.Add(
                    new Movimentacao { DataHora = data, Local = "local 1a", MovimentacaoEntrada = true, Quantidade = 100, IdMercadoria = m1.Id }).Entity;
                var mov2 = context.Movimentacoes.Add(
                    new Movimentacao { DataHora = data, Local = "local 1b", MovimentacaoEntrada = true, Quantidade = 10, IdMercadoria = m1.Id }).Entity;
                var mov3 = context.Movimentacoes.Add(
                    new Movimentacao { DataHora = data.AddMonths(1), Local = "local 2", MovimentacaoEntrada = true, Quantidade = 50, IdMercadoria = m1.Id }).Entity;
                var mov43 = context.Movimentacoes.Add(
                    new Movimentacao { DataHora = data, Local = "local 2", MovimentacaoEntrada = false, Quantidade = 200, IdMercadoria = m2.Id }).Entity;
                var mov5 = context.Movimentacoes.Add(
                    new Movimentacao { DataHora = data, Local = "local 1", MovimentacaoEntrada = false, Quantidade = 80, IdMercadoria = m3.Id }).Entity;
                context.SaveChanges();
            }

            _contextTest = context;
            var _repo = new MovimentacaoRepository(context);
            var _repoMercadoria = new MercadoriaRepository(context);
            _controller = new MovimentacaoController(_repo, _repoMercadoria);
        }
        public void Dispose()
        {
            _contextTest.Database.EnsureDeleted();
            _contextTest.Dispose();
        }

        [Fact]
        public void IndexTest()
        {
            var response = _controller.Index() as ViewResult;
            var model = response.Model as List<Movimentacao>;
            Assert.NotNull(model);
        }
        [Fact]
        public void GetCreateViewTest()
        {
            var response = _controller.Create() as ViewResult;
            Assert.NotNull(response);
        }
        [Fact]
        public void InsertOKTest()
        {
            var data = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            var _data = new Movimentacao()
            {
                DataHora = data,
                Local = "local 1aaaa insert",
                MovimentacaoEntrada = true,
                Quantidade = 20,
                IdMercadoria = 1
            };

            var response = _controller.Create(_data);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(response);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        [Fact]
        public void InsertErrorTest()
        {
            var data = Convert.ToDateTime(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            var _data = new Movimentacao()
            {
                DataHora = data,
                MovimentacaoEntrada = true,
                Quantidade = 20
            };
            _controller.ModelState.AddModelError("SessionName", "Required");
            var response = _controller.Create(_data) as ViewResult;
            var mov = (Movimentacao)response.ViewData.Model;
            Assert.Equal(20, mov.Quantidade);
        }
        [Fact]
        public void GetEditViewOKTest()
        {
            var response = _controller.Edit(1) as ViewResult;
            var model = response.Model as Movimentacao;
            Assert.Equal(1, model.Id);
        }
        [Fact]
        public void GetEditViewErrorTest()
        {
            var response = _controller.Edit(100);
            var notFoundObjectResult = Assert.IsType<NotFoundResult>(response);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }
        [Fact]
        public void EditOKTest()
        {
            int id = 2;
            var response = _controller.Details(id) as ViewResult;
            Movimentacao _obj = (Movimentacao)response.ViewData.Model;
            _obj.Local = "local alterado";
            _obj.Quantidade = 1;

            var response_update = _controller.Edit(id, _obj);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(response_update);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        [Fact]
        public void EditErrorIdTest()
        {
            int id = 100;
            var response = _controller.Details(id) as ViewResult;
            Assert.Null(response);
        }
        [Fact]
        public void EditErrorRequiredTest()
        {
            int id = 2;
            var response = _controller.Details(id) as ViewResult;
            Movimentacao _obj = (Movimentacao)response.ViewData.Model;
            _obj.Local = "local alterado";
            _obj.Quantidade = 1;

            _controller.ModelState.AddModelError("SessionName", "Required");
            var response_update = _controller.Edit(id, _obj) as ViewResult; 
            var model = response_update.Model as Movimentacao;
            Assert.NotNull(model);
        }
        [Fact]
        public void GetDeleteViewOKTest()
        {
            var response = _controller.Delete(1) as ViewResult;
            var model = response.Model as Movimentacao;
            Assert.Equal(1, model.Id);
        }
        [Fact]
        public void GetDeleteViewErrorTest()
        {
            var response = _controller.Delete(100);
            var notFoundObjectResult = Assert.IsType<NotFoundResult>(response);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }
        [Fact]
        public void DeleteOKTest()
        {
            var response = _controller.DeleteConfirmed(4);
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(response);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        [Fact]
        public void DeleteErrorTest()
        {
            var response = _controller.DeleteConfirmed(100);
            var notFoundObjectResult = Assert.IsType<NotFoundResult>(response);
            Assert.Equal(404, notFoundObjectResult.StatusCode);
        }
        [Fact]
        public void GraficoTest()
        {
            bool validOK = true;
            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _controller.TempData = tempData;
            var response = _controller.Grafico() as ViewResult;
            if (string.IsNullOrEmpty(response.TempData["grafico_ano"].ToString()) ||
                string.IsNullOrEmpty(response.TempData["grafico_series_entrada"].ToString()) ||
                string.IsNullOrEmpty(response.TempData["grafico_series_saida"].ToString()))
            {
                validOK = false;
            }
            Assert.True(validOK);
        }
        [Fact]
        public void EstoqueTest()
        {
            var response = _controller.Estoque() as ViewResult;
            var model = response.Model as List<Movimentacao>;
            Assert.NotNull(model);
        }
    }
}
