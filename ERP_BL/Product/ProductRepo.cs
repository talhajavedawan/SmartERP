using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP_BL.ChartofAccounts;
using ERP_BL.Databases;
using ERP_BL.Procurements.Inventories;

namespace ERP_BL.Databases
{
    public class ProductRepo :DBContextERP 
    {
        DBContextERP context = new DBContextERP();


        /// <summary>
        /// Get list of all Products in DB
        /// </summary>
        /// <returns>List of Products Objects</returns>
        public List<Product> getAll()
        {
            return context.Products.ToList();

        }
        /// <summary>
        /// Get list of all Products in DB for a current user
        /// </summary>
        /// <returns>List of Products Objects in db for current user</returns>
        public List<Product> getAllUserProducts(int userId)
        {
            var user = context.Users.FirstOrDefault(x => x.id == userId );
            //List<int> deptIds = new List<int>();
            List<Product> products = new List<Product>();
            foreach (var dpt in user.employee.departments)
                foreach (var pro in dpt.Products)
                {
                    if(pro.isActive==true)
                    products.Add(pro);
                }
            return products.Distinct().ToList();//context.Products.Where(x => x.departments.Where(y=> deptIds.Contains(y.Id))!=null).ToList();

        }
        public List<Product> getAllDepartmentProducts(int deptIdId)
        {
           var department= context.Departments.FirstOrDefault(x => x.Id == deptIdId);
            
            return department.Products.Where(x=>x.isActive==true).Distinct().ToList();//context.Products.Where(x => x.departments.Where(y=> deptIds.Contains(y.Id))!=null).ToList();

        }
        public List<CostSheetField> getAllUserCostSheetFields()
        {
            var costSheetFields = context.costSheetFields.ToList();
            return costSheetFields;
        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetDepartments()
        {
            return context.Departments.ToList();
        }
        /// <summary>
        /// Get All Active products
        /// </summary>
        /// <returns></returns>
        public List<Product> getActiveProducts(List<int> compIds, List<int> deptIds)
        {
            List<Product> finalProducts = new List<Product>();
            var companyProducts= context.Products.Where(x => x.isActive == true && x.company_id==null && x.productType==Enums.ProductType.Inventory|| compIds.Contains((int)x.company_id) && x.isActive==true ).ToList();
            foreach(var product in companyProducts)
            {
                foreach(var dept in product.departments)
                if(deptIds.Contains(dept.Id))
                    {
                        finalProducts.Add(product);
                        break;
                    }
            }
            return finalProducts;

        }
        public List<Product> getActiveInventoryProducts(List<int> compIds, List<int> deptIds)
        {
            List<Product> finalProducts = new List<Product>();
            var companyProducts = context.Products.Where(x => x.isActive == true && x.company_id == null && x.productType == Enums.ProductType.Inventory || compIds.Contains((int)x.company_id) && x.isActive == true && x.productType == Enums.ProductType.Inventory).ToList();
            foreach (var product in companyProducts)
            {
                foreach (var dept in product.departments)
                    if (deptIds.Contains(dept.Id))
                    {
                        finalProducts.Add(product);
                        break;
                    }
            }
            return finalProducts;

        }
        public List<Product> getActiveProductsForInventory(List<int> compIds, List<int> deptIds)
        {
            List<Product> finalProducts = new List<Product>();
            var companyProducts = context.Products.Where(x => x.isActive == true && x.company_id == null && x.productType == Enums.ProductType.Inventory || compIds.Contains((int)x.company_id) && x.isActive == true && x.productType==Enums.ProductType.Inventory).ToList();
            foreach (var product in companyProducts)
            {
                foreach (var dept in product.departments)
                    if (deptIds.Contains(dept.Id))
                    {
                        finalProducts.Add(product);
                        break;
                    }
            }
            return finalProducts;
        }
        public List<Product> getActiveProductsForInventory(List<int> compIds)
        {
            List<Product> finalProducts = new List<Product>();
            var companyProducts = context.Products.Where(x => x.isActive == true && x.company_id == null && x.productType == Enums.ProductType.Inventory || compIds.Contains((int)x.company_id) && x.isActive == true && x.productType == Enums.ProductType.Inventory).ToList();
            return companyProducts;
        }
        public List<Product> getActiveProductForInventory(List<int> compIds,int prodId, List<int> deptIds)
        {
            List<Product> finalProducts = new List<Product>();
            var companyProducts = context.Products.Where(x =>  compIds.Contains((int)x.company_id) && x.isActive == true && x.productType == Enums.ProductType.Inventory && x.Id == prodId).ToList();

            foreach (var product in companyProducts)
            {
                foreach (var dept in product.departments)
                    if (deptIds.Contains(dept.Id))
                    {
                        finalProducts.Add(product);
                        break;
                    }
            }
            return finalProducts;
        }
        public List<Product> getInActiveProducts(List<int> compIds)
        {
            return context.Products.Where(x => x.isActive != true && x.company_id == null || compIds.Contains((int)x.company_id) && x.isActive != true).ToList();

        }
        /// <summary>
        /// Get All inActive products
        /// </summary>
        /// <returns></returns>
        public List<Product> getinActiveProducts()
        {
            return context.Products.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="product">product  Object</param>
        public void Add(Product product)
        {

            List<Department> departments = new List<Department>();
            foreach (var _dept in product.departments)
            {
                departments.Add(context.Departments.FirstOrDefault(x => x.Id == _dept.Id));
            }
            product.departments.Clear();
            product.departments = new List<Department>();
            product.departments = departments;

            context.Products.Add(product);
            context.SaveChanges();
        }
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product">product  Object</param>
        public void Update(Product product)
        {
            
            Product prod= context.Products.FirstOrDefault(x => x.Id == product.Id);

            List<Department> departments = new List<Department>();
            foreach(var _dept in product.departments)
            {
                departments.Add(context.Departments.FirstOrDefault(x=>x.Id == _dept.Id));
            }
            prod = product;
            prod.departments.Clear();
            prod.departments = new List<Department>();
            prod.departments = departments;
            context.SaveChanges();
        }
        
        /// <summary>
        /// get Product matching to ID
        /// </summary>
        /// <param name="Productid">Product ID</param>
        /// <returns></returns>
        public Product get(int Productid)
        {
            return context.Products
                .FirstOrDefault(x => x.Id == Productid);
        }
        /// <summary>
        /// get Product matching to Code
        /// </summary>
        /// <param name="ProductCode">Product Code</param>
        /// <returns></returns>
        public Product get(String ProductCode)
        {
            return context.Products
                .FirstOrDefault(x => x.code == ProductCode);
        }
        /// <summary>
        /// Get list of all Product natures in DB
        /// </summary>
        /// <returns>List of ProductNatures Objects</returns>
        public List<ProductNature> getallnature()
        {
            return context.productNatures.ToList();

        }
        /// <summary>
        /// Get All Active product natures
        /// </summary>
        /// <returns></returns>
        public List<ProductNature> getActiveProductNatures()
        {
            return context.productNatures.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive products Natures
        /// </summary>
        /// <returns></returns>
        public List<ProductNature> getinActiveProductNatures()
        {
            return context.productNatures.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new ProductNature
        /// </summary>
        /// <param name="productnature">Product nature Object</param>
        public void Addnature(ProductNature productnature)
        {
            context.productNatures.Add(productnature);
            context.SaveChanges();
        }
        /// <summary>
        /// get Product nature by ID
        /// </summary>
        /// <param name="Natureid">Product nature ID</param>
        /// <returns></returns>
        public ProductNature getnature(int natureId)
        {
            return context.productNatures
                .FirstOrDefault(x => x.Id == natureId);
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="product">ProductNature  Object</param>
        public void Updatenature(ProductNature productnature)
        {
            ProductNature prod = context.productNatures.FirstOrDefault(x => x.Id == productnature.Id);
            prod = productnature;
            context.SaveChanges();
        }
        /// <summary>
        /// Add new ProcurementProduct
        /// </summary>
        /// <param name="proproduct">ProcurementProduct  Object</param>
        public void Add(ProcurementProduct product)
        {
            context.procurementProducts.Add(product);
            context.SaveChanges();
        }
        /// <summary>
        /// Update Procurment product
        /// </summary>
        /// <param name="product">ProcurementProduct  Object</param>
        public void Update(ProcurementProduct product)
        {
            ProcurementProduct prod = context.procurementProducts.FirstOrDefault(x => x.Id == product.Id);
            prod = product;
            context.SaveChanges();
        }
        /// <summary>
        /// get procurment Product matching to ID
        /// </summary>
        /// <param name="Productid">Product ID</param>
        /// <returns></returns>
        public ProcurementProduct getprocproduct(int productid)
        {
            return context.procurementProducts
                .FirstOrDefault(x => x.Id == productid);
        }
        /// <summary>
        /// Get all inquiry products
        /// </summary>
        /// <returns></returns>
        public List<InquiryProduct> getAllInquiryProducts()
        {
            return context.inquiryProducts.ToList();

        }
        
        /// <summary>
        /// Add new Inquiry product
        /// </summary>
        /// <param name="product">inquiryproduct  Object</param>
        public void Add(InquiryProduct product)
        {
            context.inquiryProducts.Add(product);
            context.SaveChanges();
        }
        /// <summary>
        /// Update InquiryProduct
        /// </summary>
        /// <param name="product">InquiryProduct  Object</param>
        public void Update(InquiryProduct product)
        {
            InquiryProduct prod = context.inquiryProducts.FirstOrDefault(x => x.Id == product.Id);
            prod = product;
            context.SaveChanges();
        }
        /// <summary>
        /// Get inquiry  product  by Id
        /// </summary>
        /// <param name="productid"></param>
        /// <returns></returns>
        public InquiryProduct getinquiryproduct(int productid)
        {
            return context.inquiryProducts
                .FirstOrDefault(x => x.Id == productid);
        }

        //unit of measure Functions 


        /// <summary>
        /// Get list of all UnitOfMeasures in DB
        /// </summary>
        /// <returns>List of UnitOfMeasures Objects</returns>
        public List<UnitOfMeasure> getallUnitOfMeasure()
        {
            return context.unitOfMeasures.ToList();

        }
        /// <summary>
        /// Get All Active UnitOfMeasure
        /// </summary>
        /// <returns></returns>
        public List<UnitOfMeasure> getActiveUnitOfMeasures()
        {
            return context.unitOfMeasures.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive UnitOfMeasures
        /// </summary>
        /// <returns></returns>
        public List<UnitOfMeasure> getinActiveUnitOfMeasures()
        {
            return context.unitOfMeasures.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new UnitOfMeasure
        /// </summary>
        /// <param name="unitOfMeasure">UnitOfMeasure Object</param>
        public void AddUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        {
            context.unitOfMeasures.Add(unitOfMeasure);
            context.SaveChanges();
        }
        /// <summary>
        /// get UnitOfMeasure by ID
        /// </summary>
        /// <param name="unitOfMeasureid">UnitOfMeasure ID</param>
        /// <returns></returns>
        public UnitOfMeasure getUnitOfMeasure(int unitOfMeasureId)
        {
            return context.unitOfMeasures
                .FirstOrDefault(x => x.Id == unitOfMeasureId);
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="UnitOfMeasure">UnitOfMeasure  Object</param>
        public void UpdateUnitOfMeasure(UnitOfMeasure unitOfMeasure)
        {
            UnitOfMeasure prod = context.unitOfMeasures.FirstOrDefault(x => x.Id == unitOfMeasure.Id);
            prod = unitOfMeasure;
            context.SaveChanges();
        }
        /// <summary>
        /// Get all Departments
        /// </summary>
        /// <returns>List of Deparment Objects</returns>
        public List<ERP_BL.Databases.Department> GetUserDepartments(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.departments;
        }
        /// <summary>
        /// Get all Companies
        /// </summary>
        /// <returns>List of Companies Objects</returns>
        public List<ERP_BL.Databases.Company> GetUserCompanies(int id)
        {
            var user = context.Users
                .FirstOrDefault(x => x.id == id);
            return user.employee.Companies;
        }

        //Product Category Functions


        /// <summary>
        /// Get list of all ProductCategories in DB
        /// </summary>
        /// <returns>List of ProductCategories Objects</returns>
        public List<ProductCategory> getallProductCategory()
        {
            return context.productCategories
                .ToList();

        }
        /// <summary>
        /// Get All Active ProductCategory
        /// </summary>
        /// <returns></returns>
        public List<ProductCategory> getActiveProductCategories()
        {
            return context.productCategories.Where(x => x.isActive == true).ToList();

        }
        /// <summary>
        /// Get All inActive ProductCategories
        /// </summary>
        /// <returns></returns>
        public List<ProductCategory> getinActiveProductCategories()
        {
            return context.productCategories.Where(x => x.isActive == false).ToList();

        }
        /// <summary>
        /// Add new ProductCategory
        /// </summary>
        /// <param name="productCategory">ProductCategory Object</param>
        public void AddProductCategory(ProductCategory productCategory)
        {
            context.productCategories.Add(productCategory);
            context.SaveChanges();
        }
        /// <summary>
        /// get ProductCategory by ID
        /// </summary>
        /// <param name="productCategoryid">ProductCategory ID</param>
        /// <returns></returns>
        public ProductCategory getProductCategory(int productCategoryId)
        {
            return context.productCategories
                .FirstOrDefault(x => x.Id == productCategoryId);
        }
        /// <summarProductNature
        /// </summary>
        /// <param name="ProductCategory">ProductCategory  Object</param>
        public void UpdateProductCategory(ProductCategory productCategory)
        {
            ProductCategory prod = context.productCategories.FirstOrDefault(x => x.Id == productCategory.Id);
            prod = productCategory;
            context.SaveChanges();
        }
        public List<Product> getAllParentProducts()
        {
            return context.Products.Where(x => x.isActive == true).ToList();

        }
        public double getProductAvgAmount(int prodId)
        {
            
            var prodInventories= context.inventories.Where(x => x.prodId == prodId && x.TransactionsType==Enums.InventoryTransactionsType.PurchaseInvoice).ToList();

            return prodInventories.Sum(x => x.UnitRate / prodInventories.Count());
        }
        public double getProductAvgAmountOC(int prodId)
        {
            var prodInventories = context.inventories.Where(x => x.prodId == prodId && x.TransactionsType == Enums.InventoryTransactionsType.PurchaseInvoice).ToList();
            return prodInventories.Sum(x => x.UnitRate / prodInventories.Count());
        }
        public List<Inventory> getPurchases(int prodId)
        {
            return   context.inventories.Where(x => x.prodId == prodId && x.TransactionsType == Enums.InventoryTransactionsType.PurchaseInvoice).ToList();
        }
        public List<Inventory> getSales(int prodId)
        {
            return context.inventories.Where(x => x.prodId == prodId && x.TransactionsType == Enums.InventoryTransactionsType.SaleInvoice).ToList();
        }
        public Product getByCode(string code)
        {
            return context.Products.FirstOrDefault(x => x.code == code);
        }
        public Product getProductByCode(string code)
        {
            return context.Products.FirstOrDefault(x => x.code == code);
        }

    }
}
