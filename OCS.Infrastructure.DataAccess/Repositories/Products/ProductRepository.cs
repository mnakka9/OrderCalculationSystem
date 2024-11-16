using OCS.Domain.Entities;

namespace OCS.Infrastructure.DataAccess.Repositories.Products;
public class ProductRepository(TaxSystemContext context) : Repository<Product>(context), IProductRepository;
