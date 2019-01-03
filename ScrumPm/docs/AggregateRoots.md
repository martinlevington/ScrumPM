## AggregateRoots

"*Aggregate is a pattern in Domain-Driven Design. A DDD aggregate is a cluster of domain objects that can be treated as a single unit. An example may be an order and its line-items, these will be separate objects, but it's useful to treat the order (together with its line items) as a single aggregate.*" (see the [full description](http://martinfowler.com/bliki/DDD_Aggregate.html))

`IAggregateRoot` interface is used to mark an `Entity` class so that an aggregate  also has `Identity`.


* An aggregate root is responsible to preserve it's own integrity. This is also true for all entities, but aggregate root has responsibility for it's sub entities too. So, the aggregate root must always be in a valid state.
* An aggregate root can be referenced by it's Id. Do not reference it by it's navigation property.
* An aggregate root is treated as a single unit. It's retrieved and updated as a single unit. It's generally considered as a transaction boundary.
* Work with sub-entities over the aggregate root- do not modify them independently.

#### Aggregate Example

This is a full sample of an aggregate root with a related sub-entity collection:

````C#
public class Order : AggregateRoot<Identity>
{
    public  string ReferenceNo { get; protected set; }

    public  int TotalItemCount { get; protected set; }

    public  DateTime Created { get; protected set; }

    public  List<OrderLine> OrderLines { get; protected set; }

 

    public static Create (OrderIdentity id, string referenceNo)
    {
        return new Order(OrderIdentity id, string referenceNo)
        {
            Check.NotNull(referenceNo, nameof(referenceNo));
        
            Id = id;
            ReferenceNo = referenceNo;
            OrderLines = new List<OrderLine>();
        }
    }

    private Order(OrderIdentity id, string referenceNo)
    {
        Check.NotNull(referenceNo, nameof(referenceNo));
        
        Id = id;
        ReferenceNo = referenceNo;
        
        OrderLines = new List<OrderLine>();
    }

    public void AddProduct(ProductIdentity productId, int count)
    {
        if (count <= 0)
        {
            throw new ArgumentException(
                "You can not add zero or negative count of products!",
                nameof(count)
            );
        }

        var existingLine = OrderLines.FirstOrDefault(ol => ol.ProductId == productId);

        if (existingLine == null)
        {
            OrderLines.Add(new OrderLine(this.Id, productId, count));
        }
        else
        {
            existingLine.ChangeCount(existingLine.Count + count);
        }

        TotalItemCount += count;
    }
}

public class OrderLine : Entity
{
    public  OrderIdentity OrderId { get; protected set; }

    public  ProductIdentity ProductId { get; protected set; }

    public  int Count { get; protected set; }

    protected OrderLine()
    {

    }

    internal OrderLine(OrderIdentity orderId, ProductIdentity productId, int count)
    {
        OrderId = orderId;
        ProductId = productId;
        Count = count;
    }

    internal void ChangeCount(int newCount)
    {
        Count = newCount;
    }
}
````


`Order` is an **aggregate root** with an identity property. It has a collection of `OrderLine` entities. `OrderLine` is another entity.

While this example the has the following best practices of an aggregate root:

* `Order` has a private constructor to stop this class being injected into other services. A Create method that takes **minimal requirements** to construct an `Order` instance. So, it's not possible to create an order without an id and reference number. So the class has the responsibilities to maintain the its state and ensure it state is always valid.
* `OrderLine` constructor is internal, so it is only allowed to be created by the domain layer. It's used inside of the `Order.AddProduct` method.
* `Order.AddProduct` implements the business rule to add a product to an order.
* All properties have `protected` setters. This is to prevent the entity from arbitrary changes from outside of the entity. For example, it would be dangerous to set `TotalItemCount` without adding a new product to the order. It's value is maintained by the `AddProduct` method.


#### Aggregate Roots with Composite Keys

While it's not common (and not suggested) for aggregate roots, it is in fact possible to define composite keys in the same way as defined for the mentioned entities above. Use non-generic `AggregateRoot` base class in that case.