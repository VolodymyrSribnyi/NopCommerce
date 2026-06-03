using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Catalog;
using System.Threading.Tasks;

namespace Nop.Web.Areas.Admin.Controllers
{
    public class PriceListController : BaseAdminController
    {
        private readonly IPriceListModelFactory _priceListModelFactory;
        private readonly IPriceListService _priceListService;
        private readonly IPriceListItemModelFactory _priceListItemModelFactory;
        private readonly IPriceListItemService _priceListItemService;
        private readonly IProductModelFactory _productModelFactory;

        public PriceListController(
            IPriceListModelFactory priceListModelFactory,
            IPriceListService priceListService,
            IPriceListItemModelFactory priceListItemModelFactory, 
            IPriceListItemService priceListItemService,         
            IProductModelFactory productModelFactory)
        {
            _priceListModelFactory = priceListModelFactory;
            _priceListService = priceListService;
            _priceListItemModelFactory = priceListItemModelFactory;
            _priceListItemService = priceListItemService;
            _productModelFactory = productModelFactory;
        }

        public async Task<IActionResult> List(PriceListSearchModel searchModel)
        {
            var model = await _priceListModelFactory.PreparePriceListSearchModelAsync(searchModel);
            return View(model);
        }

        // Залишаємо просто [HttpPost], без тексту в дужках
        [HttpPost]
        public async Task<IActionResult> GetPriceListTable(PriceListSearchModel searchModel)
        {
            var list = await _priceListModelFactory.PreparePriceListListModelAsync(searchModel);
            return Json(list);
        }

        // Для звичайних сторінок [HttpGet] можна взагалі не писати, це поведінка за замовчуванням
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

        // Система автоматично зловить {id} з URL завдяки стандартній маршрутизації
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
                CurrencyId = priceList.CurrencyId
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

            // Видаляємо сам прайс-лист (і бажано, щоб у сервісі або на рівні БД 
            // каскадно видалялися й пов'язані PriceListItems, щоб не лишалося сміття)
            await _priceListService.DeletePriceListAsync(priceList);

            return RedirectToAction("List");
        }
        [HttpPost]
        public async Task<IActionResult> PriceListItemsList(PriceListItemSearchModel searchModel)
        {
            // Перевіряємо, чи є доступ до прайс-листа
            var priceList = await _priceListService.GetPriceListByIdAsync(searchModel.PriceListId);
            if (priceList == null)
                return Json(new { Data = new List<PriceListItemModel>(), Total = 0 }); // Порожня таблиця

            // Отримуємо дані через фабрику
            var model = await _priceListItemModelFactory.PreparePriceListItemListModelAsync(searchModel);

            return Json(model);
        }
        // 1. МЕТОД ДЛЯ ВИДАЛЕННЯ ТОВАРУ З ПРАЙС-ЛИСТА
        [HttpPost]
        public async Task<IActionResult> PriceListItemDelete(int id)
        {
            // Шукаємо елемент у базі
            var priceListItem = await _priceListItemService.GetPriceListItemByIdAsync(id);
            if (priceListItem == null)
                return Json(new { success = false, message = "Елемент не знайдено" });

            // Видаляємо (переконайся, що такий метод є в твоєму сервісі)
            await _priceListItemService.DeletePriceListItemAsync(priceListItem);

            return Json(new { success = true });
        }

        // 2. ВІДКРИТТЯ ПОПУПУ ЗІ СПИСКОМ ТОВАРІВ САЙТУ
        public async Task<IActionResult> ProductAddPopup(int priceListId, string btnId)
        {
            // Використовуємо стандартну фабрику nopCommerce для підготовки пошукової моделі товарів
            // Нам знадобиться ін'єкція IProductModelFactory в конструктор контролера
            var searchModel = await _productModelFactory.PrepareProductSearchModelAsync(new ProductSearchModel());

            // Передаємо потрібні ID через ViewBag у вікно
            ViewBag.PriceListId = priceListId;
            ViewBag.BtnId = btnId;

            return View(searchModel);
        }

        // 3. ОБРОБКА ДАНИХ, КОЛИ МЕНЕДЖЕР ОБРАВ ТОВАРИ І НАТИСНУВ "ЗБЕРЕГТИ"
        [HttpPost]
        public async Task<IActionResult> ProductAddPopupList(ProductSearchModel searchModel)
        {
            // Отримуємо список усіх товарів сайту для вибору
            var model = await _productModelFactory.PrepareProductListModelAsync(searchModel);
            return Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> SelectedProductAdd(int priceListId, string selectedProductIds)
        {
            if (string.IsNullOrEmpty(selectedProductIds))
                return Json(new { success = false });

            // Перетворюємо рядок з ID (наприклад "1,5,12") у масив чисел
            var productIds = selectedProductIds.Split(',').Select(int.Parse).ToList();

            foreach (var productId in productIds)
            {
                // Створюємо новий запис для нашого прайс-листа із ціною за замовчуванням 0
                var priceListItem = new PriceListItem
                {
                    PriceListId = priceListId,
                    ProductId = productId,
                    Price = 0 // Менеджер потім відредагує ціну через кнопку "Редагувати"
                };

                await _priceListItemService.InsertPriceListItemAsync(priceListItem);
            }

            // Повертаємо JavaScript код, який закриє вікно попупу і оновить головну таблицю
            return View("RefreshDataTable");
        }
    }
}