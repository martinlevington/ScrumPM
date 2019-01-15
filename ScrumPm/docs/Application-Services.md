# Application Services

Application services are used to implement the **use cases** of an application. They are used to **expose domain logic to the presentation layer**.

An Application Service is called from the presentation layer (optionally) with a **DTO (Data Transfer Object)** as the parameter. It uses domain objects to **perform some specific business logic** and (optionally) returns a DTO back to the presentation layer. Thus, the presentation layer is completely **isolated** from domain layer.

## Example

### OrderOrderOrder Entity

Assume that you have a `Order` entity (actually, an aggregate root) defined as shown below:

````csharp
public class Order : AggregateRoot<OrderIdentity>
{
    public const int MaxNameLength = 128;

    public  string OrderReference { get; protected set; }

    public  Customer Customer { get; set; }

    public static Order Create()
    {
        return new Order(OrderIdentity id, string orderReference, Customer customer);
    }

    private Order(OrderIdentity id, string orderReference, Customer customer)
    {
        Id = id;
        OrderReference = CheckOrderReference(orderReference);
        Type = type;
        Price = price;
    }

    public virtual void ChangeOrderReference(string name)
    {
        OrderReference = CheckOrderReference(name);
    }

    private  string CheckOrderReference(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"name can not be empty or white space!");
        }

        if (name.Length > MaxNameLength)
        {
            throw new ArgumentException($"name can not be longer than {MaxNameLength} chars!");
        }

        return name;
    }
}
````

* `Order` entity has a `MaxNameLength` that defines the maximum length of the `OrderReference` property. 
* `Order` constructor and `ChangeOrderReference` method to ensure that the `OrderReference` is always a valid value. Notice that `OrderReference`'s  setter is not `public`.



### IOrderAppService Interface

An application service should implement the `IApplicationService` interface. It's good to create an interface for each application service:

````csharp
public interface IOrderAppService : IApplicationService
{
    Task CreateAsync(CreateOrderDto input);
}
````

A Create method will be implemented as the example. `CreateOrderDto` is defined like that:

````csharp
public class CreateOrderDto
{
    [Required]
    [StringLength(Order.MaxNameLength)]
    public string OrderReference { get; set; }

    public Customer Customer { get; set; }

    public List<LineItemDto> LineItems { get; set; } = new List<LineItemDto>();
}
````

> See [data transfer objects document](Data-Transfer-Objects.md) for more about DTOs.

### OrderAppService (Implementation)

````csharp
public class OrderAppService :  IOrderAppService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderRepositoIGuidGeneratorry _guidGenerator;

    public OrderAppService(IOrderRepository orderRepository, IGuidGenerator guidGenerator)
    {
        _orderRepository = orderRepository;
        _guidGenerator = guidGenerator;
    }

    public async Task CreateAsync(CreateOrderDto input)
    {
        var order = new Order(
            guidGenerator.Create(),
            input.OrderReference,
            input.Customer,
        );

        await _orderRepository.InsertAsync(order);
    }
}
````


* `OrderAppService` implements the `IOrderAppService` as expected.


## Data Transfer Objects

Application services should get and return DTO's instead of entities.  Exposing entities to presentation layer (or to remote clients) have significant problems and not suggested, as this expose the internal implementation of the application to the outside world.

See the [DTO documentation](Data-Transfer-Objects.md) for more.

## Validation

Inputs of application service methods should be  validated (like ASP.NET Core controller actions). You can use the standard data annotation attributes or custom validation method to perform the validation.

See the [validation document](Validation.md) for more.

## Authorization

It's possible to use declarative and imperative authorization for application service methods.

See the [authorization document](Authorization.md) for more.


## Lifetime

Lifetime of application services are [transient](Dependency-Injection.md).
