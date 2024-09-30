using AutoMapper;
using DAL.Concrete;
using DALEF.Concrete;
using DTO;
using Microsoft.Data.SqlClient;
using DALEF.Mapping;
using DALEF.MappingProfile;
using DALEF.Context;


string connStr = "Data Source=DESKTOP-UANF194;Initial Catalog=MyDatabase;Integrated Security=True;Encrypt=False;";
var config = new MapperConfiguration(c =>
{
    c.AddMaps(typeof(UserProfile).Assembly);
    c.AddMaps(typeof(ProductProfile).Assembly);
    c.AddMaps(typeof(CategoryProfile).Assembly);
});

IMapper mapper = config.CreateMapper();

MapperConfiguration conf = new MapperConfiguration(a => a.AddMaps(typeof(CategoryProfile).Assembly));


User loggedInUser = null;

do
{
    
    if (loggedInUser == null)
    {
        loggedInUser = Login(); 
        if (loggedInUser == null)
        {
            Console.WriteLine("Invalid credentials, please try again or press 'l' to leave.");

            
            string retryInput = Console.ReadLine()?.Trim().ToLower();

            
            if (retryInput == "l")
            {
                break; 
            }

            
            continue;
        }

    }

    
    Console.WriteLine("Welcome to Trading Company!\n");
    char option;

    Console.WriteLine("Please select an option:");
    Console.WriteLine("1. Add new Categorie");
    Console.WriteLine("2. List all Categories");
    Console.WriteLine("3. Delete Categorie");
    Console.WriteLine("4. Add Product");
    Console.WriteLine("5. List all Products");
    Console.WriteLine("6. Delete Product");
    Console.WriteLine("7. Add User");
    Console.WriteLine("8. List all Users");
    Console.WriteLine("9. Delete User");
    Console.WriteLine("10. Add new Categorie with EF");
    Console.WriteLine("11. List all Categories with EF");
    Console.WriteLine("12. Delete Categorie with EF");
    Console.WriteLine("13. Add Product with EF");
    Console.WriteLine("14. List all Products with EF");
    Console.WriteLine("15. Delete Product with EF");
    Console.WriteLine("16. Add User with EF");
    Console.WriteLine("17. List all Users with EF");
    Console.WriteLine("18. Delete User with EF");
    Console.WriteLine("l. Leave");

    string selectedOption = Console.ReadLine()?.Trim().ToLower();

    // Перевірка на коректність вводу
    if (int.TryParse(selectedOption, out int optionNumber) && optionNumber >= 1 && optionNumber <= 18)
    {
        switch (optionNumber)
        {
            case 1:
                AddCategorie();
                break;
            case 2:
                ListAllCategories();
                break;
            case 3:
                DeleteCategory();
                break;
            case 4:
                AddProduct();
                break;
            case 5:
                ListAllProducts();
                break;
            case 6:
                DeleteProduct();
                break;
            case 7:
                AddUser();
                break;
            case 8:
                ListAllUsers();
                break;
            case 9:
                DeleteUser();
                break;
            case 10:
                AddCategorieWithEF();
                break;
            case 11:
                ListAllCategoriesWithEF();
                break;
            case 12:
                DeleteCategorieWithEF();
                break;
            case 13:
                AddProductWithEF();
                break;
            case 14:
                ListAllProductsWithEF();
                break;
            case 15:
                DeleteProductWithEF();
                break;
            case 16:
                AddUserWithEF();
                break;
            case 17:
                ListAllUsersWithEF();
                break;
            case 18:
                DeleteUserWithEF();
                break;
            default:
                Console.WriteLine("Incorrect option selected!");
                break;
        }
    }
    else if (selectedOption == "l")
    {
        break; // Вихід з програми
    }
    else
    {
        Console.WriteLine("Incorrect option selected!");
    }

} while (true);

// Метод для логування користувача
User Login()
{
    Console.WriteLine("Please enter your username:");
    string userName = Console.ReadLine();

    Console.WriteLine("Please enter your password:");
    string userPassword = Console.ReadLine();

    var userDal = new UserDal(connStr);
    User user = userDal.GetByCredentials(userName, userPassword); // Метод, який потрібно реалізувати в DAL

    return user;
}


// Функції CRUD для Categorie
void AddCategorie()
{
    Console.WriteLine("Please enter Categorie Name:");
    string name = Console.ReadLine();

    Console.WriteLine("Please enter Categorie Description:");
    string description = Console.ReadLine();

    var categorieDal = new CategoryDAL(connStr); // Використовуємо твій клас CategorieDal
    var category = new Category { CategoryName = name, CategoryDescription = description };
    categorieDal.Create(category);

    Console.WriteLine($"Categorie '{name}' has been added.");
}


void ListAllCategories()
{
    var categorieDal = new CategoryDAL(connStr);
    List<Category> categories = categorieDal.GetAll();

    foreach (var category in categories)
    {
        Console.WriteLine($"{category.CategoryId}.\t{category.CategoryName}\t{category.CategoryDescription}");
    }
}

void DeleteCategory()
{
    Console.WriteLine("Please enter Categorie ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    var categorieDal = new CategoryDAL(connStr);
    Category deletedCategory = categorieDal.Delete(id);

    if (deletedCategory != null)
        Console.WriteLine($"Categorie '{deletedCategory.CategoryName}' has been deleted.");
    else
        Console.WriteLine("Categorie not found.");
}


// Функції CRUD для Product
void AddProduct()
{
    Console.WriteLine("Please enter Product Name:");
    string productName = Console.ReadLine();

    Console.WriteLine("Please enter Product Price:");
    decimal price = Convert.ToDecimal(Console.ReadLine());

    Console.WriteLine("Please enter Product Quantity:");
    int quantity = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter Category ID:");
    int categoryId = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter User ID:");
    int userId = Convert.ToInt32(Console.ReadLine());

    var productDal = new ProductDal(connStr);
    var product = new Product
    {
        ProductName = productName,
        Price = price,
        Quantity = quantity,
        CategoryId = categoryId,
        UserId = userId
    };
    productDal.Create(product);

    Console.WriteLine($"Product '{productName}' has been added.");
}


void ListAllProducts()
{
    var productDal = new ProductDal(connStr);
    List<Product> products = productDal.GetAll();
    foreach (var product in products)
    {
        Console.WriteLine($"{product.ProductId}.\t{product.ProductName}\t{product.Price}\t{product.Quantity}");
    }
}


void DeleteProduct()
{
    Console.WriteLine("Please enter Product ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    var productDal = new ProductDal(connStr);
    Product deletedProduct = productDal.Delete(id);

    if (deletedProduct != null)
        Console.WriteLine($"Product '{deletedProduct.ProductName}' has been deleted.");
    else
        Console.WriteLine("Product not found.");
}


// Функції CRUD для User
void AddUser()
{
    Console.WriteLine("Please enter User Name:");
    string userName = Console.ReadLine();

    Console.WriteLine("Please enter User Password:");
    string userPassword = Console.ReadLine();

    Console.WriteLine("Please enter User Role:");
    string userRole = Console.ReadLine();

    var userDal = new UserDal(connStr);
    var user = new User
    {
        UserName = userName,
        UserPassword = userPassword,
        Role = userRole
    };
    userDal.Create(user);

    Console.WriteLine($"User '{userName}' has been added.");
}


void ListAllUsers()
{
    var userDal = new UserDal(connStr);
    List<User> users = userDal.GetAll();

    if (users.Count == 0)
    {
        Console.WriteLine("No users found.");
        return;
    }

    foreach (var user in users)
    {
        Console.WriteLine($"{user.UserId}.\t{user.UserName}\t{user.Role}");
    }
}


void DeleteUser()
{
    Console.WriteLine("Please enter User ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    var userDal = new UserDal(connStr);
    User deletedUser = userDal.Delete(id);

    if (deletedUser != null)
        Console.WriteLine($"User '{deletedUser.UserName}' has been deleted.");
    else
        Console.WriteLine("User not found.");
}

// Функції CRUD для Categorie з використанням EF
void AddCategorieWithEF()
{
    string name = "Empty";
    Console.WriteLine("Please enter Categorie Name:");
    name = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Category name cannot be empty.");
        return; // Вихід з функції, якщо ім'я не вказано
    }

    Console.WriteLine("Please enter Categorie Description:");
    string description = Console.ReadLine();

    using (var context = new DataBase(connStr))
    {
        var categoryDal = new CategoryDALEF(context, conf.CreateMapper());
        var category = new Category { CategoryName = name, CategoryDescription = description };

        // Логування даних
        Console.WriteLine($"Adding category with Name: '{name}' and Description: '{description}'");

        
            categoryDal.Create(category);
            Console.WriteLine($"Categorie '{name}' has been added.");
        
            
        
    }
}



void ListAllCategoriesWithEF()
{
    using (var context = new DataBase(connStr))
    {
        var categoryDal = new CategoryDALEF(context, mapper); // Використовуємо DAL з EF
        List<Category> categories = categoryDal.GetAll();

        if (categories.Count == 0)
        {
            Console.WriteLine("No categories found.");
            return;
        }

        foreach (var category in categories)
        {
            Console.WriteLine($"{category.CategoryId}.\t{category.CategoryName}\t{category.CategoryDescription}");
        }
    }
}

void DeleteCategorieWithEF()
{
    Console.WriteLine("Please enter Categorie ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var categoryDal = new CategoryDALEF(context, mapper); // Використовуємо DAL з EF
        Category deletedCategory = categoryDal.Delete(id);

        if (deletedCategory != null)
            Console.WriteLine($"Categorie '{deletedCategory.CategoryName}' has been deleted.");
        else
            Console.WriteLine("Categorie not found.");
    }
}


void AddUserWithEF()
{
    Console.WriteLine("Please enter User Name:");
    string userName = Console.ReadLine();

    Console.WriteLine("Please enter User Password:");
    string userPassword = Console.ReadLine();

    Console.WriteLine("Please enter User Role:");
    string userRole = Console.ReadLine();

    using (var context = new DataBase(connStr))
    {
        var userDal = new UserDALEF(context, mapper); 
        var user = new User
        {
            UserName = userName,
            UserPassword = userPassword,
            Role = userRole
        };
        userDal.Create(user);

        Console.WriteLine($"User '{userName}' has been added.");
    }
}

void ListAllUsersWithEF()
{
    using (var context = new DataBase(connStr))
    {
        var userDal = new UserDALEF(context, mapper);
        List<User> users = userDal.GetAll();

        if (users.Count == 0)
        {
            Console.WriteLine("No users found.");
            return;
        }

        foreach (var user in users)
        {
            Console.WriteLine($"{user.UserId}.\t{user.UserName}\t{user.Role}");
        }
    }
}

void DeleteUserWithEF()
{
    Console.WriteLine("Please enter User ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var userDal = new UserDALEF(context, mapper); 
        User deletedUser = userDal.Delete(id);

        if (deletedUser != null)
            Console.WriteLine($"User '{deletedUser.UserName}' has been deleted.");
        else
            Console.WriteLine("User not found.");
    }
}

void AddProductWithEF()
{
    Console.WriteLine("Please enter Product Name:");
    string productName = Console.ReadLine();

    Console.WriteLine("Please enter Product Price:");
    decimal price = Convert.ToDecimal(Console.ReadLine());

    Console.WriteLine("Please enter Product Quantity:");
    int quantity = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter Category ID:");
    int categoryId = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Please enter User ID:");
    int userId = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var productDal = new ProductDALEF(context, mapper); 
        var product = new Product
        {
            ProductName = productName,
            Price = price,
            Quantity = quantity,
            CategoryId = categoryId,
            UserId = userId
        };
        productDal.Create(product);

        Console.WriteLine($"Product '{productName}' has been added.");
    }
}

void ListAllProductsWithEF()
{
    using (var context = new DataBase(connStr))
    {
        var productDal = new ProductDALEF(context, mapper); 
        List<Product> products = productDal.GetAll();

        if (products.Count == 0)
        {
            Console.WriteLine("No products found.");
            return;
        }

        foreach (var product in products)
        {
            Console.WriteLine($"{product.ProductId}.\t{product.ProductName}\t{product.Price}\t{product.Quantity}");
        }
    }
}

void DeleteProductWithEF()
{
    Console.WriteLine("Please enter Product ID to delete:");
    int id = Convert.ToInt32(Console.ReadLine());

    using (var context = new DataBase(connStr))
    {
        var productDal = new ProductDALEF(context, mapper); 
        Product deletedProduct = productDal.Delete(id);

        if (deletedProduct != null)
            Console.WriteLine($"Product '{deletedProduct.ProductName}' has been deleted.");
        else
            Console.WriteLine("Product not found.");
    }
}







