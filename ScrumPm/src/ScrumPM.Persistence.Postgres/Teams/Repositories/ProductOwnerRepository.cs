using System;
using System.Collections.Generic;
using System.Linq;
using FluentResults;
using ScrumPm.Domain.Teams;
using ScrumPm.Domain.Tenants;
using ScrumPm.Persistence.Models.Teams.Adapters;
using ScrumPm.Persistence.Models.Teams.PersistenceModels;

namespace ScrumPM.Persistence.Postgres.Teams.Repositories
{
    public class ProductOwnerRepository : EfCoreRepository<ScrumPmContext, ProductOwnerEf,Guid > , IProductOwnerRepository
    {
        private readonly IPersistenceAdapter<TenantId, ProductOwnerEf, ProductOwner> _persistenceAdapter;

        public ProductOwnerRepository(IDbContextProvider<ScrumPmContext> dbContextProvider, IPersistenceAdapter<TenantId, ProductOwnerEf, ProductOwner> persistenceAdapter ) : base(dbContextProvider)
        {
            _persistenceAdapter = persistenceAdapter;
        }

        public ICollection<ProductOwner> GetAllProductOwners(TenantId tenantId)
        {
            throw new NotImplementedException();
        }

        public Result<ProductOwner> Get(TenantId tenantId, string userName)
        {

            var productOwner = DbContext.ProductOwners.FirstOrDefault(x => x.TenantId == tenantId.Id && x.UserName == userName);

            if (productOwner != null)
            {
                return Results.Ok<ProductOwner>(_persistenceAdapter.ToDomain(tenantId, productOwner));
            }

            return Results.Fail<ProductOwner>("Unable To find Object");

        }

        public void Remove(ProductOwner owner)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(IEnumerable<ProductOwner> owners)
        {
            throw new NotImplementedException();
        }

      

        public Result<ProductOwner> Save(TenantId tenantId, ProductOwner owner)
        {

            var ownerEf = new ProductOwnerEf()
            {
               Id = owner.ProductOwnerId,
               TenantId = owner.TenantId.Id,
               UserName = owner.UserName,
               Created = DateTime.Now,
               EmailAddress = owner.EmailAddress,
               FirstName = owner.FirstName,
               LastName = owner.LastName,
               Modified = DateTime.Now,
        
            };

            DbContext.ProductOwners.Add(ownerEf);

             return Results.Ok<ProductOwner>(_persistenceAdapter.ToDomain(tenantId, ownerEf));



        }

        public Result SaveAll(IEnumerable<ProductOwner> owners)
        {
            throw new NotImplementedException();
        }

     

      
    }
}
