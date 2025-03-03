using System;
using System.Collections.Generic;

namespace Convenience.org.Repositories.Interfaces
{
    public interface IProductRepository
    {
        List<Product> GetProducts(List<Guid> WebPageGuids);
        Product GetProduct(Guid WebPageGuid);
    }
}
