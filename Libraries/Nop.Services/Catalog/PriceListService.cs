using Nop.Core.Domain.Catalog;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Services.Catalog
{
    public class PriceListService:IPriceListService
    {
        private readonly IRepository<PriceList> _repository;
        private readonly IRepository<PriceListItem> _listItemRepository;
        public PriceListService(IRepository<PriceList> repository, IRepository<PriceListItem> listItemRepository)
        {
            _repository = repository;
            _listItemRepository = listItemRepository;
        }
        public async Task InsertPriceListAsync(PriceList priceList)
        {
            await _repository.InsertAsync(priceList);
        }
        public async Task UpdatePriceListAsync(PriceList priceList)
        {
            await _repository.UpdateAsync(priceList);
            
        }
        public async Task<PriceList> GetPriceListByIdAsync(int priceListId)
        {
            var priceList=await _repository.GetByIdAsync(priceListId);
            return priceList;
        }
        public async Task<IList<PriceListItem>>GetPriceListItemsByPriceListIdAsync(int priceListId)
        {
            var priceListItems = await _listItemRepository.GetAllAsync(query=>query.Where(p=> p.PriceListId==priceListId));
            return priceListItems;
        }
        public async Task<decimal?> GetPriceByProductAndPriceListAsync(int productId, int priceListId)
        {
            var priceListItems = await _listItemRepository.GetAllAsync(query =>
                query.Where(p => p.ProductId == productId && p.PriceListId == priceListId));

            var item = priceListItems.FirstOrDefault();

            return item?.Price;
        }
        public virtual async Task<IList<PriceList>> GetAllPriceListsAsync()
        {
            return await _repository.GetAllAsync(query => query);
        }
    }
}
