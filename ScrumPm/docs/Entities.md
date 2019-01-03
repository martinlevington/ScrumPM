## Entities

Entities are one of the core concepts of DDD (Domain Driven Design). Eric Evans describe it as "*An object that is not fundamentally defined by its attributes, but rather by a thread of continuity and identity*".


### Entity Class

Entities are derived from  `IEntity` interfaces as shown below:

```C#
public class Person : Entity
{
    
    Identity Id {get;set;}
    
    public string Name { get; set; }

    public DateTime Created { get; set; }

    private Person(Identity id)
    {
        Id = id;
        Created = DateTime.Now;
    }

    public static Create (Identity id)
    {
        return new Person()
        {
               Id = id;
        Created = DateTime.Now;
        }
    }

      protected override IEnumerable<object> GetIdentityComponents()
    {
         return new List<object>()
               {
                   Id
               }.AsEnumerable() ;
    }
}
```

> Note Entities all need an identity. The abstract class called 'Identity' can be used as a base for this.

There are a number of starting class options to use depending upon the requirements.

e.g.

`Entity`   
`EntityWithCompositeId`

Entity class also overrides the **equality** operator (==) to easily check if two entities are equal (they are equals if they are same entity type and their Ids are equals).


#### Entities with Composite Keys

Some entities may need to have **composite keys**. In that case, you can derive your entity from the non-generic `EntityWithCompositeId` class. Example:

````C#
public class UserRole : EntityWithCompositeId
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
    
    public DateTime Created { get; set; }

    public UserRole()
    {
            
    }

    protected override IEnumerable<object> GetIdentityComponents()
    {
         return new List<object>()
               {
                   UserId,
                   RoleId
               }.AsEnumerable() ;
    }
}
````

For the example above, the composite key is composed of `UserId` and `RoleId`. 

Entities with composite keys should implement the `GetIdentityComponents()` method as shown above.


> Composite primary keys has a restriction with repositories. Since it has not known Id property, you can not use `IRepository<TEntity, TKey>` for these entities. However, you can always use `IRepository<TEntity>`. See repository documentation (TODO: link) for more.