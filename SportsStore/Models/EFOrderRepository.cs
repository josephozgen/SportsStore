using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SportsStore.Models
{
    // This class implements the IOrderRepository interface using Entity Framework Core,
    // allowing the set of Order objects that have been stored to be retrieved and
    // allowing for orders to be created or changed.
    public class EFOrderRepository : IOrderRepository
    {
        private StoreDbContext context;

        public EFOrderRepository(StoreDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Order> Orders => context.Orders
            // These methods are used to specify that when an Order object is read
            // from the database, the collection associated with the Lines property
            // should also be loaded along with each Product object associated
            // with each collection object.
            .Include(o => o.Lines)
            .ThenInclude(l => l.Product);

        public void SaveOrder(Order order)
        {
            // This ensures that Entity Framework Core won't try to write the
            // de-serialized Product objects that are associated with the Order
            context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.OrderID == 0)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();
        }
    }
}
