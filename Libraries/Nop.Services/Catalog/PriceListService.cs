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
    public class PriceListService:IPriceListService
    {
        private readonly IRepository<PriceList> _repository;
        
        public PriceListService(IRepository<PriceList> repository)
        {
            _repository = repository; 
        }
        public async Task InsertPriceListAsync(PriceList priceList)
        {
            if (priceList == null)
                throw new ArgumentNullException(nameof(priceList));

            await _repository.InsertAsync(priceList);
        }
        public async Task UpdatePriceListAsync(PriceList priceList)
        {
            if (priceList == null)
                throw new ArgumentNullException(nameof(priceList));

            await _repository.UpdateAsync(priceList);
            
        }
        public async Task<PriceList> GetPriceListByIdAsync(int priceListId)
        {
            var priceList=await _repository.GetByIdAsync(priceListId);
            return priceList;
        }
        public async Task<IPagedList<PriceList>> GetAllPriceListAsync(string name, int pageIndex, int pageSize)
        {
            var priceListList = await _repository.GetAllPagedAsync(query=> query.Where(p=> string.IsNullOrEmpty(name) || p.Name.Contains(name)), pageIndex, pageSize);
            return priceListList;
        }

        public async Task DeletePriceListAsync(PriceList priceList)
        {
            if (priceList == null)
                throw new ArgumentNullException(nameof(priceList));

            await _repository.DeleteAsync(priceList);
        }
    }
}
