using System.Linq; // Обов'язково для .Split().Select()
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.DataTables;
using Nop.Web.Framework.Models.Extensions;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class PriceListController : BaseAdminController
    {
        private readonly IPriceListModelFactory _priceListModelFactory;
        private readonly IPriceListService _priceListService;
        private readonly IPriceListItemModelFactory _priceListItemModelFactory;
        private readonly IPriceListItemService _priceListItemService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IProductService _productService;

        public PriceListController(
            IPriceListModelFactory priceListModelFactory,
            IPriceListService priceListService,
            IPriceListItemModelFactory priceListItemModelFactory,
            IPriceListItemService priceListItemService,
            IProductModelFactory productModelFactory,
            IProductService productService)
        {
            _priceListModelFactory = priceListModelFactory;
            _priceListService = priceListService;
            _priceListItemModelFactory = priceListItemModelFactory;
            _priceListItemService = priceListItemService;
            _productModelFactory = productModelFactory;
            _productService = productService;
        }

        public async Task<IActionResult> List(PriceListSearchModel searchModel)
        {
            var model = await _priceListModelFactory.PreparePriceListSearchModelAsync(searchModel);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GetPriceListTable(PriceListSearchModel searchModel)
        {
            var list = await _priceListModelFactory.PreparePriceListListModelAsync(searchModel);
            return Json(list);
        }

        public async Task<IActionResult> Create()
        {
            PriceListModel priceListModel = new PriceListModel();
            return View(priceListModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PriceListModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var priceList = new PriceList
            {
                Name = model.Name,
                CurrencyId = model.CurrencyId
            };

            await _priceListService.InsertPriceListAsync(priceList);

            return RedirectToAction("List");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("List");
            }

            var priceList = await _priceListService.GetPriceListByIdAsync(id);
            if (priceList == null)
            {
                return RedirectToAction("List");
            }

            PriceListModel priceListModel = new PriceListModel
            {
                Id = priceList.Id,
                Name = priceList.Name,
                CurrencyId = priceList.CurrencyId,
                PriceListItemSearchModel = await _priceListItemModelFactory
                .PreparePriceListItemSearchModelAsync(new PriceListItemSearchModel(), priceList)
            };

            return View(priceListModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PriceListModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var priceList = await _priceListService.GetPriceListByIdAsync(model.Id);
            if (priceList == null)
            {
                return View(model);
            }

            priceList.Name = model.Name;
            priceList.CurrencyId = model.CurrencyId;

            await _priceListService.UpdatePriceListAsync(priceList);

            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var priceList = await _priceListService.GetPriceListByIdAsync(id);
            if (priceList == null)
                return RedirectToAction("List");

            await _priceListService.DeletePriceListAsync(priceList);

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> PriceListItemsList(PriceListItemSearchModel searchModel)
        {
            // Якщо ID прайс-листа не передався, повертаємо правильну порожню структуру
            if (searchModel.PriceListId == 0)
            {
                return Json(new
                {
                    Data = new List<PriceListItemModel>(),
                    RecordsTotal = 0,
                    RecordsFiltered = 0
                });
            }

            var items = await _priceListItemService.GetPriceListItemsByPriceListIdAsync(searchModel.PriceListId, searchModel.Page - 1,
        searchModel.PageSize);

            var gridModel = new List<PriceListItemModel>();
            foreach (var item in items)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                gridModel.Add(new PriceListItemModel
                {
                    Id = item.Id, // КРИТИЧНО ВАЖЛИВО ДЛЯ DT_RowId!
                    PriceListId = item.PriceListId,
                    ProductId = item.ProductId,
                    ProductName = product?.Name ?? "Товар",
                    Price = item.Price
                });
            }

            // ПРАВИЛЬНИЙ ПІДХІД: Повертаємо анонімний об'єкт, який ідеально збігається з вимогами DataTables
            return Json(new
            {
                Data = gridModel,
                RecordsTotal = gridModel.Count,
                RecordsFiltered = gridModel.Count
            });
        }

        // 1. МЕТОД ДЛЯ ОНОВЛЕННЯ ЦІНИ ПРЯМО В ТАБЛИЦІ (Inline Editing)
        [HttpPost]
        public async Task<IActionResult> PriceListItemUpdate(PriceListItemModel model)
        {
            var priceListItem = await _priceListItemService.GetPriceListItemByIdAsync(model.Id);
            if (priceListItem == null)
                return Content("Елемент не знайдено");

            // Оновлюємо тільки ціну, яку ввів користувач
            priceListItem.Price = model.Price;

            await _priceListItemService.UpdatePriceListItemAsync(priceListItem);

            // Стандартна відповідь nopCommerce для успішного inline-оновлення в DataTables
            return new NullJsonResult();
        }

        // 2. МЕТОД ДЛЯ ВИДАЛЕННЯ ТОВАРУ З ПРАЙС-ЛИСТА
        [HttpPost]
        public async Task<IActionResult> PriceListItemDelete(int id)
        {
            var priceListItem = await _priceListItemService.GetPriceListItemByIdAsync(id);
            if (priceListItem == null)
                return Json(new { success = false, message = "Елемент не знайдено" });

            await _priceListItemService.DeletePriceListItemAsync(priceListItem);

            return Json(new { success = true });
        }

        // 3. ВІДКРИТТЯ ПОПУПУ ЗІ СПИСКОМ ТОВАРІВ САЙТУ
        public async Task<IActionResult> ProductAddPopup(int priceListId, string btnId)
        {
            var searchModel = await _productModelFactory.PrepareProductSearchModelAsync(new ProductSearchModel());

            ViewBag.PriceListId = priceListId;
            ViewBag.BtnId = btnId;

            return View(searchModel);
        }

        // 4. ЗАВАНТАЖЕННЯ ДАНИХ У ТАБЛИЦЮ ПОПУПУ
        [HttpPost]
        public async Task<IActionResult> ProductAddPopupList(ProductSearchModel searchModel)
        {
            var model = await _productModelFactory.PrepareProductListModelAsync(searchModel);
            return Json(model);
        }

        // 5. ОБРОБКА ЗБЕРЕЖЕННЯ ОБРАНИХ ТОВАРІВ (УЗГОДЖЕНО З JS)
        [HttpPost]
        public async Task<IActionResult> SelectedProductAdd(int priceListId, string selectedIds)
        {
            // Якщо нічого не прийшло — повертаємо false
            if (string.IsNullOrEmpty(selectedIds))
                return Json(new { success = false, message = "Не вибрано жодного товару" });

            var productIds = selectedIds.Split(',').Select(int.Parse).ToList();

            foreach (var productId in productIds)
            {
                var priceListItem = new PriceListItem
                {
                    PriceListId = priceListId,
                    ProductId = productId,
                    Price = 0 // Початкова ціна 0, менеджер змінить її в гриді
                };

                await _priceListItemService.InsertPriceListItemAsync(priceListItem);
            }

            return Json(new { success = true });
        }
    }
}
