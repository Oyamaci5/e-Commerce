
# e-Commerce

# Overview 
### Service Classes: 

    ProductRepository (IProductRepository): This service class handles CRUD operations for products in the database. 
    It includes methods to retrieve all products, get a specific product by ID, add a new product, 
    remove a product, and update a product's information. 

    PurchaseRepository (IPurchaseService): The PurchaseRepository manages purchase-related operations. 
    It includes methods to add products to a cart, 
    calculate cart prices with discounts, and complete the purchase process. 

    RoleRepository (IRoleRepository): The UserRoleRepository service interacts with user roles in the system. 
    It provides methods to retrieve roles based on user IDs and perform role-related operations. 

    CacheService(ICachService): Caching plays a crucial role in improving application performance, scalability, 
    and responsiveness. 

    All  services add scoped in Program.cs to integrate with its own interface to satisfy Dependency Injection. 

### Model Classes: 

    Product Model: Represents a product with properties such as ID, name, description, price, and stock quantity. 

    Purchase Model: Defines a purchase entity with attributes like purchase ID, user ID, product ID, quantity, total price, and purchase date. 

    User Model: Represents a user in the system and contains properties like ID, username, password. 

    Role Model: Defines roles available in the system, such as Admin, Employee, Customer. 

    UserWithRoleDTO Model: It keeps username and roles. Like UserRole model. Instead of keeping id it has names. 

### JWT:  

JWTs are used for authorization and security. 

Explain the benefits of using asynchronous programming: 

initiate multiple operations and handle the results as they complete instead of waiting so it becomes, 

- Concurrency Control 

- Increased Performance 

- Improved Scalability 

### Describe how caching works and why it's beneficial: 

- Using in-memory-cache with CacheService it has 3 methods which get, set and remove 

- It is benefical because no need to get data every time from database. It creates performance issues. When you cache data you and set key with data, you can use cached data where cache service defined. 

#### In short: 

- Better User Experience because of faster response 

- Scalability: Caching helps improve system scalability by reducing the load on backend data stores or service 

- Improved Performance 

## HOW TO USE:

- First of all, Clone the project

```bash
  git clone https://github.com/Oyamaci5/e-Commerce.git
```
After cloning open project and use Package Manager Console and type this commands:
```bash
  Add-Migration First
  Update-database
```
After doing these, just open the project and follow the steps below:

- In the auth api, the admin role can perform all operations but customers and employees only perform login and register. 

- Register api assigns the role to customer by default, When we register or login, a jwt token is returned to us. 

- We write the jwt in the Authorize section at the top right and give it as "Bearer jwt_token" and thus we are authoized. 

- Then we click on Get api/Products to access the book list in the Products api and click on try it out and then click on execute. In the response section, we get a list of products. 

- In the same way, we can access the properties of a single product with its id to find out how much is left in stock or to find out the price. 

- Then we add the products we want to buy with the help of productId with the addToCart method in the Purchase API. See the list above for product ids. 

- We can add more than one of the same product. In the same api, we can see our cart with showCart and remove products from the cart with removeFromCart. 

- To see the total price, discounted price and discount rate of our products, we can use the getCartPrice api and see the values. 

- When we say purchase product, a message is returned that your purchase is successful and both the transaction we have made in the purchase table is recorded and the stock amount of the product decreases in line with the amount we purchase. 

- Finally, we can see our purchase history with endpoint. It has product list, paid amount , total price with our id. 

## Notes: 

- For Auth api: We can change the user's role as admin with addRole and RemoveRole endpoints. Admin can see the entire user list and can add or remove roles to the user with the id of the user whose role he wants to change. If the role is not already in the table, it is also added to the Role table. 

- There are 3 fields in the Products api that only the admin can use, these are update, add and delete methods. Because only admin can add, update or delete a product. 

- In addition, roles can also be changed from the role endpoints and only admin can do these operations. 
