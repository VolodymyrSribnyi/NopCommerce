using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
    public class PriceListItemService:IPriceListItemService
    {
        private readonly IRepository<PriceListItem> _listItemRepository;
        public PriceListItemService(IRepository<PriceListItem> listItemRepository)
        {
            _listItemRepository=listItemRepository;
        }
        public async Task<IPagedList<PriceListItem>> GetPriceListItemsByPriceListIdAsync(int priceListId, int pageIndex, int pageSize)
        {
            var priceListItems = await _listItemRepository.GetAllPagedAsync(query => query.Where(p => p.PriceListId==priceListId), pageIndex, pageSize);
            return priceListItems;
        }
        public async Task<PriceListItem> GetPriceListItemByIdAsync(int priceListItemId)
        {
            if (priceListItemId == 0)
                return null;

            return await _listItemRepository.GetByIdAsync(priceListItemId);
        }
        public async Task InsertPriceListItemAsync(PriceListItem priceListItem)
        {
            if (priceListItem == null)
                throw new ArgumentNullException(nameof(priceListItem));
            await _listItemRepository.InsertAsync(priceListItem);
        }
        public async Task UpdatePriceListItemAsync(PriceListItem priceListItem)
        {
            if (priceListItem == null)
                throw new ArgumentNullException(nameof(priceListItem));

            await _listItemRepository.UpdateAsync(priceListItem);
        }

        public async Task DeletePriceListItemAsync(PriceListItem priceListItem)
        {
            if (priceListItem == null)
                throw new ArgumentNullException(nameof(priceListItem));

            await _listItemRepository.DeleteAsync(priceListItem);
        }
    }
}
