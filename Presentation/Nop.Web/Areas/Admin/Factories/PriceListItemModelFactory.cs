using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class PriceListItemModelFactory:IPriceListItemModelFactory
    {
        private readonly IPriceListItemService _priceListItemService;
        private readonly IProductService _productService;
        public PriceListItemModelFactory(IPriceListItemService priceListItemService, IProductService productService)
        {
            _priceListItemService=priceListItemService;
            _productService=productService;
        }

        public virtual Task<PriceListItemSearchModel> PreparePriceListItemSearchModelAsync(PriceListItemSearchModel searchModel, PriceList priceList)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            if (priceList == null)
                throw new ArgumentNullException(nameof(priceList));

            searchModel.PriceListId = priceList.Id;
            searchModel.SetGridPageSize(); // Стандартний розмір сторінки для грідів nopCommerce

            return Task.FromResult(searchModel);
        }

        public virtual async Task<PriceListItemListModel> PreparePriceListItemListModelAsync(PriceListItemSearchModel searchModel)
        {
            // 1. Отримуємо список елементів з бази даних (через твій оновлений метод!)
            // Пам'ятай: DataTables передає сторінки починаючи з 1, а база (і nopCommerce) рахує з 0, тому Page - 1
            var priceListItems = await _priceListItemService.GetPriceListItemsByPriceListIdAsync(
                searchModel.PriceListId,
                searchModel.Page - 1,
                searchModel.PageSize);

            // 2. Створюємо порожній список для моделей
            var models = new List<PriceListItemModel>();

            // 3. Проходимося по кожному елементу і формуємо модель
            foreach (var item in priceListItems)
            {
                // Шукаємо сам товар у базі nopCommerce
                var product = await _productService.GetProductByIdAsync(item.ProductId);

                models.Add(new PriceListItemModel
                {
                    Id = item.Id,
                    PriceListId = item.PriceListId,
                    ProductId = item.ProductId,

                    // Якщо товар знайдено, беремо його ім'я. Якщо раптом його видалили - пишемо заглушку.
                    ProductName = product != null ? product.Name : "Товар видалено",

                    Price = item.Price
                });
            }

            // 4. Повертаємо готову модель для DataTables
            return new PriceListItemListModel().PrepareToGrid(searchModel, priceListItems, () => models);
        }
    }
}
