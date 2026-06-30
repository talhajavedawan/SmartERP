namespace ERP_BL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoredProcedureDisableLazyLoading : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Address_Insert",
                p => new
                    {
                        Line1 = p.String(),
                        Line2 = p.String(),
                        Zip = p.Int(),
                        State = p.String(),
                        Country = p.String(),
                        City = p.String(),
                        region = p.String(),
                        addressType = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tabAddress]([Line1], [Line2], [Zip], [State], [Country], [City], [region], [addressType])
                      VALUES (@Line1, @Line2, @Zip, @State, @Country, @City, @region, @addressType)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tabAddress]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tabAddress] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Address_Update",
                p => new
                    {
                        Id = p.Int(),
                        Line1 = p.String(),
                        Line2 = p.String(),
                        Zip = p.Int(),
                        State = p.String(),
                        Country = p.String(),
                        City = p.String(),
                        region = p.String(),
                        addressType = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tabAddress]
                      SET [Line1] = @Line1, [Line2] = @Line2, [Zip] = @Zip, [State] = @State, [Country] = @Country, [City] = @City, [region] = @region, [addressType] = @addressType
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Address_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tabAddress]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Contact_Insert",
                p => new
                    {
                        ContactNo = p.String(),
                        Fax = p.String(),
                        Email = p.String(),
                        Website = p.String(),
                        SMLink1 = p.String(),
                        SMLink2 = p.String(),
                        SMLink3 = p.String(),
                        contactType = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tabContact]([ContactNo], [Fax], [Email], [Website], [SMLink1], [SMLink2], [SMLink3], [contactType])
                      VALUES (@ContactNo, @Fax, @Email, @Website, @SMLink1, @SMLink2, @SMLink3, @contactType)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tabContact]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tabContact] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Contact_Update",
                p => new
                    {
                        Id = p.Int(),
                        ContactNo = p.String(),
                        Fax = p.String(),
                        Email = p.String(),
                        Website = p.String(),
                        SMLink1 = p.String(),
                        SMLink2 = p.String(),
                        SMLink3 = p.String(),
                        contactType = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tabContact]
                      SET [ContactNo] = @ContactNo, [Fax] = @Fax, [Email] = @Email, [Website] = @Website, [SMLink1] = @SMLink1, [SMLink2] = @SMLink2, [SMLink3] = @SMLink3, [contactType] = @contactType
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Contact_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tabContact]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Currency_Insert",
                p => new
                    {
                        CurrencyName = p.String(),
                        Symbol = p.String(),
                        Abbrivation = p.String(),
                        Country = p.String(),
                    },
                body:
                    @"INSERT [dbo].[Currencies]([CurrencyName], [Symbol], [Abbrivation], [Country])
                      VALUES (@CurrencyName, @Symbol, @Abbrivation, @Country)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Currencies]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Currencies] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Currency_Update",
                p => new
                    {
                        Id = p.Int(),
                        CurrencyName = p.String(),
                        Symbol = p.String(),
                        Abbrivation = p.String(),
                        Country = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Currencies]
                      SET [CurrencyName] = @CurrencyName, [Symbol] = @Symbol, [Abbrivation] = @Abbrivation, [Country] = @Country
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Currency_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Currencies]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Department_Insert",
                p => new
                    {
                        DeptName = p.String(),
                        Code = p.String(maxLength: 20),
                        Abbrivation = p.String(maxLength: 20),
                        Timestamp = p.DateTime(),
                        userID = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tabDepartment]([DeptName], [Code], [Abbrivation], [Timestamp], [userID], [IsSubsidary], [ParentID])
                      VALUES (@DeptName, @Code, @Abbrivation, @Timestamp, @userID, @IsSubsidary, @ParentID)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tabDepartment]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tabDepartment] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Department_Update",
                p => new
                    {
                        Id = p.Int(),
                        DeptName = p.String(),
                        Code = p.String(maxLength: 20),
                        Abbrivation = p.String(maxLength: 20),
                        Timestamp = p.DateTime(),
                        userID = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tabDepartment]
                      SET [DeptName] = @DeptName, [Code] = @Code, [Abbrivation] = @Abbrivation, [Timestamp] = @Timestamp, [userID] = @userID, [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Department_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tabDepartment]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerCompany_Insert",
                p => new
                    {
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        billingAddres_Id = p.Int(),
                        company_Id = p.Int(),
                        contact_Id = p.Int(),
                        contactPerson_Id = p.Int(),
                        shippingAddress_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[CustomerCompanies]([IsSubsidary], [ParentID], [billingAddres_Id], [company_Id], [contact_Id], [contactPerson_Id], [shippingAddress_Id])
                      VALUES (@IsSubsidary, @ParentID, @billingAddres_Id, @company_Id, @contact_Id, @contactPerson_Id, @shippingAddress_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[CustomerCompanies]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[CustomerCompanies] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerCompany_Update",
                p => new
                    {
                        Id = p.Int(),
                        IsSubsidary = p.Boolean(),
                        ParentID = p.Int(),
                        billingAddres_Id = p.Int(),
                        company_Id = p.Int(),
                        contact_Id = p.Int(),
                        contactPerson_Id = p.Int(),
                        shippingAddress_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[CustomerCompanies]
                      SET [IsSubsidary] = @IsSubsidary, [ParentID] = @ParentID, [billingAddres_Id] = @billingAddres_Id, [company_Id] = @company_Id, [contact_Id] = @contact_Id, [contactPerson_Id] = @contactPerson_Id, [shippingAddress_Id] = @shippingAddress_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.CustomerCompany_Delete",
                p => new
                    {
                        Id = p.Int(),
                        billingAddres_Id = p.Int(),
                        company_Id = p.Int(),
                        contact_Id = p.Int(),
                        contactPerson_Id = p.Int(),
                        shippingAddress_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[CustomerCompanies]
                      WHERE (((((([Id] = @Id) AND (([billingAddres_Id] = @billingAddres_Id) OR ([billingAddres_Id] IS NULL AND @billingAddres_Id IS NULL))) AND (([company_Id] = @company_Id) OR ([company_Id] IS NULL AND @company_Id IS NULL))) AND (([contact_Id] = @contact_Id) OR ([contact_Id] IS NULL AND @contact_Id IS NULL))) AND (([contactPerson_Id] = @contactPerson_Id) OR ([contactPerson_Id] IS NULL AND @contactPerson_Id IS NULL))) AND (([shippingAddress_Id] = @shippingAddress_Id) OR ([shippingAddress_Id] IS NULL AND @shippingAddress_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Insert",
                p => new
                    {
                        MaritalStatus = p.String(),
                        Disability = p.Boolean(),
                        DisDescription = p.String(),
                        isActive = p.Boolean(),
                        Status = p.Int(),
                        JoinDate = p.DateTime(),
                        BasicPay = p.Double(),
                        address_Id = p.Int(),
                        contact_Id = p.Int(),
                        Desig_DesigId = p.Int(),
                        person_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Employees]([MaritalStatus], [Disability], [DisDescription], [isActive], [Status], [JoinDate], [BasicPay], [address_Id], [contact_Id], [Desig_DesigId], [person_Id])
                      VALUES (@MaritalStatus, @Disability, @DisDescription, @isActive, @Status, @JoinDate, @BasicPay, @address_Id, @contact_Id, @Desig_DesigId, @person_Id)
                      
                      DECLARE @EmpId int
                      SELECT @EmpId = [EmpId]
                      FROM [dbo].[Employees]
                      WHERE @@ROWCOUNT > 0 AND [EmpId] = scope_identity()
                      
                      SELECT t0.[EmpId]
                      FROM [dbo].[Employees] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[EmpId] = @EmpId"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Update",
                p => new
                    {
                        EmpId = p.Int(),
                        MaritalStatus = p.String(),
                        Disability = p.Boolean(),
                        DisDescription = p.String(),
                        isActive = p.Boolean(),
                        Status = p.Int(),
                        JoinDate = p.DateTime(),
                        BasicPay = p.Double(),
                        address_Id = p.Int(),
                        contact_Id = p.Int(),
                        Desig_DesigId = p.Int(),
                        person_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Employees]
                      SET [MaritalStatus] = @MaritalStatus, [Disability] = @Disability, [DisDescription] = @DisDescription, [isActive] = @isActive, [Status] = @Status, [JoinDate] = @JoinDate, [BasicPay] = @BasicPay, [address_Id] = @address_Id, [contact_Id] = @contact_Id, [Desig_DesigId] = @Desig_DesigId, [person_Id] = @person_Id
                      WHERE ([EmpId] = @EmpId)"
            );
            
            CreateStoredProcedure(
                "dbo.Employee_Delete",
                p => new
                    {
                        EmpId = p.Int(),
                        address_Id = p.Int(),
                        contact_Id = p.Int(),
                        Desig_DesigId = p.Int(),
                        person_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Employees]
                      WHERE ((((([EmpId] = @EmpId) AND (([address_Id] = @address_Id) OR ([address_Id] IS NULL AND @address_Id IS NULL))) AND (([contact_Id] = @contact_Id) OR ([contact_Id] IS NULL AND @contact_Id IS NULL))) AND (([Desig_DesigId] = @Desig_DesigId) OR ([Desig_DesigId] IS NULL AND @Desig_DesigId IS NULL))) AND (([person_Id] = @person_Id) OR ([person_Id] IS NULL AND @person_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.User_Insert",
                p => new
                    {
                        employeeId = p.Int(),
                        userName = p.String(maxLength: 25, unicode: false),
                        password = p.String(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"INSERT [dbo].[Users]([employeeId], [userName], [password], [isActive])
                      VALUES (@employeeId, @userName, @password, @isActive)
                      
                      DECLARE @id int
                      SELECT @id = [id]
                      FROM [dbo].[Users]
                      WHERE @@ROWCOUNT > 0 AND [id] = scope_identity()
                      
                      SELECT t0.[id]
                      FROM [dbo].[Users] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[id] = @id"
            );
            
            CreateStoredProcedure(
                "dbo.User_Update",
                p => new
                    {
                        id = p.Int(),
                        employeeId = p.Int(),
                        userName = p.String(maxLength: 25, unicode: false),
                        password = p.String(),
                        isActive = p.Boolean(),
                    },
                body:
                    @"UPDATE [dbo].[Users]
                      SET [employeeId] = @employeeId, [userName] = @userName, [password] = @password, [isActive] = @isActive
                      WHERE ([id] = @id)"
            );
            
            CreateStoredProcedure(
                "dbo.User_Delete",
                p => new
                    {
                        id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Users]
                      WHERE ([id] = @id)"
            );
            
            CreateStoredProcedure(
                "dbo.Inquiry_Insert",
                p => new
                    {
                       referenceNo = p.String(),
                        fileNo = p.String(),
                        inquiryDate = p.DateTime(),
                        responseDate = p.DateTime(),
                        alertDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        customerCompany_Id = p.Int(),
                        dept_Id = p.Int(),
                        company_Id = p.Int(),
                        user_Id = p.Int(),
                        allocation_Id = p.Int(),
                        inquirytype = p.Int(),
                        inquiryStatus_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Inquiries]([referenceNo], [fileNo], [inquiryDate], [responseDate], [alertDate], [closingDate], [customerCompany_Id], [dept_Id], [company_Id], [user_Id], [allocation_Id], [inquirytype], [inquiryStatus_Id])
                      VALUES (@referenceNo, @fileNo, @inquiryDate, @responseDate, @alertDate, @closingDate, @customerCompany_Id, @dept_Id, @company_Id, @user_Id, @allocation_Id, @inquirytype, @inquiryStatus_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Inquiries]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Inquiries] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Inquiry_Update",
                p => new
                    {
                        Id = p.Int(),
                       referenceNo = p.String(),
                        fileNo = p.String(),
                        inquiryDate = p.DateTime(),
                        responseDate = p.DateTime(),
                        alertDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        customerCompany_Id = p.Int(),
                        dept_Id = p.Int(),
                        company_Id = p.Int(),
                        user_Id = p.Int(),
                        allocation_Id = p.Int(),
                        inquirytype = p.Int(),
                        inquiryStatus_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Inquiries]
                      SET [referenceNo] = @referenceNo, [fileNo] = @fileNo, [inquiryDate] = @inquiryDate, [responseDate] = @responseDate, [alertDate] = @alertDate, [closingDate] = @closingDate, [customerCompany_Id] = @customerCompany_Id, [dept_Id] = @dept_Id, [company_Id] = @company_Id, [user_Id] = @user_Id, [allocation_Id] = @allocation_Id, [inquirytype] = @inquirytype, [inquiryStatus_Id] = @inquiryStatus_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Inquiry_Delete",
                p => new
                    {
                        Id = p.Int(),
                        inquiryStatus_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Inquiries]
                      WHERE (([Id] = @Id) AND (([inquiryStatus_Id] = @inquiryStatus_Id) OR ([inquiryStatus_Id] IS NULL AND @inquiryStatus_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.InquiryStatus_Insert",
                p => new
                    {
                        Status = p.String(),
                        isApproved = p.Boolean(),
                        isActive = p.Boolean(),
                        backcolor = p.String(),
                        forecolor = p.String(),
                    },
                body:
                    @"INSERT [dbo].[InquiryStatus]([Status], [isApproved], [isActive], [backcolor], [forecolor])
                      VALUES (@Status, @isApproved, @isActive, @backcolor, @forecolor)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[InquiryStatus]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[InquiryStatus] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.InquiryStatus_Update",
                p => new
                    {
                        Id = p.Int(),
                        Status = p.String(),
                        isApproved = p.Boolean(),
                        isActive = p.Boolean(),
                        backcolor = p.String(),
                        forecolor = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[InquiryStatus]
                      SET [Status] = @Status, [isApproved] = @isApproved, [isActive] = @isActive, [backcolor] = @backcolor, [forecolor] = @forecolor
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InquiryStatus_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[InquiryStatus]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InquiryProduct_Insert",
                p => new
                    {
                        UOM = p.String(),
                        ownDiscription = p.String(),
                        quantity = p.Double(),
                        product_Id = p.Int(),
                        Inquiry_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[InquiryProducts]([UOM], [ownDiscription], [quantity], [product_Id], [Inquiry_Id])
                      VALUES (@UOM, @ownDiscription, @quantity, @product_Id, @Inquiry_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[InquiryProducts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[InquiryProducts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.InquiryProduct_Update",
                p => new
                    {
                        Id = p.Int(),
                        UOM = p.String(),
                        ownDiscription = p.String(),
                        quantity = p.Double(),
                        product_Id = p.Int(),
                        Inquiry_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[InquiryProducts]
                      SET [UOM] = @UOM, [ownDiscription] = @ownDiscription, [quantity] = @quantity, [product_Id] = @product_Id, [Inquiry_Id] = @Inquiry_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.InquiryProduct_Delete",
                p => new
                    {
                        Id = p.Int(),
                        Inquiry_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[InquiryProducts]
                      WHERE (([Id] = @Id) AND (([Inquiry_Id] = @Inquiry_Id) OR ([Inquiry_Id] IS NULL AND @Inquiry_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.Product_Insert",
                p => new
                    {
                        item = p.String(),
                        code = p.String(maxLength: 32, unicode: false),
                        nature_Id = p.Int(),
                        unitOfMeasureId = p.Int(),
                        categoryId = p.Int(),
                        itemDescription = p.String(),
                        ownDescription = p.String(),
                        isActive = p.Boolean(),
                        user_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Products]([item], [code], [nature_Id], [unitOfMeasureId], [categoryId], [itemDescription], [ownDescription], [isActive], [user_Id])
                      VALUES (@item, @code, @nature_Id, @unitOfMeasureId, @categoryId, @itemDescription, @ownDescription, @isActive, @user_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Products]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Products] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Product_Update",
                p => new
                    {
                        Id = p.Int(),
                        item = p.String(),
                        code = p.String(maxLength: 32, unicode: false),
                        nature_Id = p.Int(),
                        unitOfMeasureId = p.Int(),
                        categoryId = p.Int(),
                        itemDescription = p.String(),
                        ownDescription = p.String(),
                        isActive = p.Boolean(),
                        user_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Products]
                      SET [item] = @item, [code] = @code, [nature_Id] = @nature_Id, [unitOfMeasureId] = @unitOfMeasureId, [categoryId] = @categoryId, [itemDescription] = @itemDescription, [ownDescription] = @ownDescription, [isActive] = @isActive, [user_Id] = @user_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Product_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Products]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Offer_Insert",
                p => new
                    {
                       referenceNo = p.String(),
                        fileNo = p.String(),
                        ourreferenceNo = p.String(),
                        offerDate = p.DateTime(),
                        offerValidityDate = p.DateTime(),
                        deliveryDate = p.DateTime(),
                        maker = p.String(),
                        origin = p.String(),
                        responseDate = p.DateTime(),
                        exchngeRate = p.Single(),
                        bidOpenDate = p.DateTime(),
                        alertDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        commision = p.String(),
                        comments = p.String(),
                        deliveryTime = p.String(),
                        totalFOBValue = p.Double(),
                        totalCFRValue = p.Double(),
                        isPercentTax = p.Boolean(),
                        salesTax = p.Double(),
                        customerCompany_Id = p.Int(),
                        dept_Id = p.Int(),
                        user_Id = p.Int(),
                        allocation_Id = p.Int(),
                        vendor_Id = p.Int(),
                        company_Id = p.Int(),
                        offertype = p.Int(),
                        inquiry_Id = p.Int(),
                        bid_Id = p.Int(),
                        currency_Id = p.Int(),
                        paymentterm_Id = p.Int(),
                        incoterm_Id = p.Int(),
                        TitleValue1Id = p.Int(),
                        TitleValue2Id = p.Int(),
                        offerStatus_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Offers]([referenceNo], [fileNo], [ourreferenceNo], [offerDate], [offerValidityDate], [deliveryDate], [maker], [origin], [responseDate], [exchngeRate], [bidOpenDate], [alertDate], [closingDate], [commision], [comments], [deliveryTime], [totalFOBValue], [totalCFRValue], [isPercentTax], [salesTax], [customerCompany_Id], [dept_Id], [user_Id], [allocation_Id], [vendor_Id], [company_Id], [offertype], [inquiry_Id], [bid_Id], [currency_Id], [paymentterm_Id], [incoterm_Id], [TitleValue1Id], [TitleValue2Id], [offerStatus_Id])
                      VALUES (@referenceNo, @fileNo, @ourreferenceNo, @offerDate, @offerValidityDate, @deliveryDate, @maker, @origin, @responseDate, @exchngeRate, @bidOpenDate, @alertDate, @closingDate, @commision, @comments, @deliveryTime, @totalFOBValue, @totalCFRValue, @isPercentTax, @salesTax, @customerCompany_Id, @dept_Id, @user_Id, @allocation_Id, @vendor_Id, @company_Id, @offertype, @inquiry_Id, @bid_Id, @currency_Id, @paymentterm_Id, @incoterm_Id, @TitleValue1Id, @TitleValue2Id, @offerStatus_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[Offers]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[Offers] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Offer_Update",
                p => new
                    {
                        Id = p.Int(),
                       referenceNo = p.String(),
                        fileNo = p.String(),
                        ourreferenceNo = p.String(),
                        offerDate = p.DateTime(),
                        offerValidityDate = p.DateTime(),
                        deliveryDate = p.DateTime(),
                        maker = p.String(),
                        origin = p.String(),
                        responseDate = p.DateTime(),
                        exchngeRate = p.Single(),
                        bidOpenDate = p.DateTime(),
                        alertDate = p.DateTime(),
                        closingDate = p.DateTime(),
                        commision = p.String(),
                        comments = p.String(),
                        deliveryTime = p.String(),
                        totalFOBValue = p.Double(),
                        totalCFRValue = p.Double(),
                        isPercentTax = p.Boolean(),
                        salesTax = p.Double(),
                        customerCompany_Id = p.Int(),
                        dept_Id = p.Int(),
                        user_Id = p.Int(),
                        allocation_Id = p.Int(),
                        vendor_Id = p.Int(),
                        company_Id = p.Int(),
                        offertype = p.Int(),
                        inquiry_Id = p.Int(),
                        bid_Id = p.Int(),
                        currency_Id = p.Int(),
                        paymentterm_Id = p.Int(),
                        incoterm_Id = p.Int(),
                        TitleValue1Id = p.Int(),
                        TitleValue2Id = p.Int(),
                        offerStatus_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Offers]
                      SET [referenceNo] = @referenceNo, [fileNo] = @fileNo, [ourreferenceNo] = @ourreferenceNo, [offerDate] = @offerDate, [offerValidityDate] = @offerValidityDate, [deliveryDate] = @deliveryDate, [maker] = @maker, [origin] = @origin, [responseDate] = @responseDate, [exchngeRate] = @exchngeRate, [bidOpenDate] = @bidOpenDate, [alertDate] = @alertDate, [closingDate] = @closingDate, [commision] = @commision, [comments] = @comments, [deliveryTime] = @deliveryTime, [totalFOBValue] = @totalFOBValue, [totalCFRValue] = @totalCFRValue, [isPercentTax] = @isPercentTax, [salesTax] = @salesTax, [customerCompany_Id] = @customerCompany_Id, [dept_Id] = @dept_Id, [user_Id] = @user_Id, [allocation_Id] = @allocation_Id, [vendor_Id] = @vendor_Id, [company_Id] = @company_Id, [offertype] = @offertype, [inquiry_Id] = @inquiry_Id, [bid_Id] = @bid_Id, [currency_Id] = @currency_Id, [paymentterm_Id] = @paymentterm_Id, [incoterm_Id] = @incoterm_Id, [TitleValue1Id] = @TitleValue1Id, [TitleValue2Id] = @TitleValue2Id, [offerStatus_Id] = @offerStatus_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Offer_Delete",
                p => new
                    {
                        Id = p.Int(),
                        offerStatus_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[Offers]
                      WHERE (([Id] = @Id) AND (([offerStatus_Id] = @offerStatus_Id) OR ([offerStatus_Id] IS NULL AND @offerStatus_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.OfferStatus_Insert",
                p => new
                    {
                        Status = p.String(),
                        isApproved = p.Boolean(),
                        isActive = p.Boolean(),
                        backcolor = p.String(),
                        forecolor = p.String(),
                    },
                body:
                    @"INSERT [dbo].[OfferStatus]([Status], [isApproved], [isActive], [backcolor], [forecolor])
                      VALUES (@Status, @isApproved, @isActive, @backcolor, @forecolor)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[OfferStatus]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[OfferStatus] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.OfferStatus_Update",
                p => new
                    {
                        Id = p.Int(),
                        Status = p.String(),
                        isApproved = p.Boolean(),
                        isActive = p.Boolean(),
                        backcolor = p.String(),
                        forecolor = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[OfferStatus]
                      SET [Status] = @Status, [isApproved] = @isApproved, [isActive] = @isActive, [backcolor] = @backcolor, [forecolor] = @forecolor
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.OfferStatus_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[OfferStatus]
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ProcurementProduct_Insert",
                p => new
                    {
                        unitPrice = p.Double(),
                        caption1 = p.String(),
                        value1 = p.Double(),
                        caption2 = p.String(),
                        value2 = p.Double(),
                        caption3 = p.String(),
                        value3 = p.Double(),
                        product_Id = p.Int(),
                        Offer_Id = p.Int(),
                        PurchaseOrder_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[ProcurementProducts]([unitPrice], [caption1], [value1], [caption2], [value2], [caption3], [value3], [product_Id], [Offer_Id], [PurchaseOrder_Id])
                      VALUES (@unitPrice, @caption1, @value1, @caption2, @value2, @caption3, @value3, @product_Id, @Offer_Id, @PurchaseOrder_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[ProcurementProducts]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[ProcurementProducts] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.ProcurementProduct_Update",
                p => new
                    {
                        Id = p.Int(),
                        unitPrice = p.Double(),
                        caption1 = p.String(),
                        value1 = p.Double(),
                        caption2 = p.String(),
                        value2 = p.Double(),
                        caption3 = p.String(),
                        value3 = p.Double(),
                        product_Id = p.Int(),
                        Offer_Id = p.Int(),
                        PurchaseOrder_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[ProcurementProducts]
                      SET [unitPrice] = @unitPrice, [caption1] = @caption1, [value1] = @value1, [caption2] = @caption2, [value2] = @value2, [caption3] = @caption3, [value3] = @value3, [product_Id] = @product_Id, [Offer_Id] = @Offer_Id, [PurchaseOrder_Id] = @PurchaseOrder_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.ProcurementProduct_Delete",
                p => new
                    {
                        Id = p.Int(),
                        Offer_Id = p.Int(),
                        PurchaseOrder_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[ProcurementProducts]
                      WHERE ((([Id] = @Id) AND (([Offer_Id] = @Offer_Id) OR ([Offer_Id] IS NULL AND @Offer_Id IS NULL))) AND (([PurchaseOrder_Id] = @PurchaseOrder_Id) OR ([PurchaseOrder_Id] IS NULL AND @PurchaseOrder_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.Vendor_Insert",
                p => new
                    {
                        billingAddres_Id = p.Int(),
                        company_Id = p.Int(),
                        contact_Id = p.Int(),
                        contactPerson_Id = p.Int(),
                        shippingAddress_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[tabVendor]([billingAddres_Id], [company_Id], [contact_Id], [contactPerson_Id], [shippingAddress_Id])
                      VALUES (@billingAddres_Id, @company_Id, @contact_Id, @contactPerson_Id, @shippingAddress_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[tabVendor]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[tabVendor] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.Vendor_Update",
                p => new
                    {
                        Id = p.Int(),
                        billingAddres_Id = p.Int(),
                        company_Id = p.Int(),
                        contact_Id = p.Int(),
                        contactPerson_Id = p.Int(),
                        shippingAddress_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[tabVendor]
                      SET [billingAddres_Id] = @billingAddres_Id, [company_Id] = @company_Id, [contact_Id] = @contact_Id, [contactPerson_Id] = @contactPerson_Id, [shippingAddress_Id] = @shippingAddress_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Vendor_Delete",
                p => new
                    {
                        Id = p.Int(),
                        billingAddres_Id = p.Int(),
                        company_Id = p.Int(),
                        contact_Id = p.Int(),
                        contactPerson_Id = p.Int(),
                        shippingAddress_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[tabVendor]
                      WHERE (((((([Id] = @Id) AND (([billingAddres_Id] = @billingAddres_Id) OR ([billingAddres_Id] IS NULL AND @billingAddres_Id IS NULL))) AND (([company_Id] = @company_Id) OR ([company_Id] IS NULL AND @company_Id IS NULL))) AND (([contact_Id] = @contact_Id) OR ([contact_Id] IS NULL AND @contact_Id IS NULL))) AND (([contactPerson_Id] = @contactPerson_Id) OR ([contactPerson_Id] IS NULL AND @contactPerson_Id IS NULL))) AND (([shippingAddress_Id] = @shippingAddress_Id) OR ([shippingAddress_Id] IS NULL AND @shippingAddress_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.PurchaseOrder_Insert",
                p => new
                    {
                       referenceNo = p.String(),
                        fileNo = p.String(),
                        purchaseOrderDate = p.DateTime(),
                        deliveryDate = p.DateTime(),
                        shipmentDate = p.DateTime(),
                        orderConfirmationDate = p.DateTime(),
                        billLaddingDate = p.DateTime(),
                        lCDate = p.DateTime(),
                        materialReciptDate = p.DateTime(),
                        targetYear = p.Int(),
                        lCnumber = p.String(),
                        exchngeRate = p.Single(),
                        commision = p.String(),
                        maker = p.String(),
                        origin = p.String(),
                        comments = p.String(),
                        deliveryTime = p.String(),
                        totalFOBValue = p.Double(),
                        totalCFRValue = p.Double(),
                        isPercentTax = p.Boolean(),
                        salesTax = p.Double(),
                        customerCompany_Id = p.Int(),
                        dept_Id = p.Int(),
                        user_Id = p.Int(),
                        allocation_Id = p.Int(),
                        vendor_Id = p.Int(),
                        principal_Id = p.Int(),
                        company_Id = p.Int(),
                        purchaseOrdertype = p.Int(),
                        offer_Id = p.Int(),
                        bid_Id = p.Int(),
                        currency_Id = p.Int(),
                        paymentterm_Id = p.Int(),
                        incoterm_Id = p.Int(),
                        TitleValue1Id = p.Int(),
                        TitleValue2Id = p.Int(),
                        purchaseOrderStatus_Id = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[PurchaseOrders]([referenceNo], [fileNo], [purchaseOrderDate], [deliveryDate], [shipmentDate], [orderConfirmationDate], [billLaddingDate], [lCDate], [materialReciptDate], [targetYear], [lCnumber], [exchngeRate], [commision], [maker], [origin], [comments], [deliveryTime], [totalFOBValue], [totalCFRValue], [isPercentTax], [salesTax], [customerCompany_Id], [dept_Id], [user_Id], [allocation_Id], [vendor_Id], [principal_Id], [company_Id], [purchaseOrdertype], [offer_Id], [bid_Id], [currency_Id], [paymentterm_Id], [incoterm_Id], [TitleValue1Id], [TitleValue2Id], [purchaseOrderStatus_Id])
                      VALUES (@referenceNo, @fileNo, @purchaseOrderDate, @deliveryDate, @shipmentDate, @orderConfirmationDate, @billLaddingDate, @lCDate, @materialReciptDate, @targetYear, @lCnumber, @exchngeRate, @commision, @maker, @origin, @comments, @deliveryTime, @totalFOBValue, @totalCFRValue, @isPercentTax, @salesTax, @customerCompany_Id, @dept_Id, @user_Id, @allocation_Id, @vendor_Id, @principal_Id, @company_Id, @purchaseOrdertype, @offer_Id, @bid_Id, @currency_Id, @paymentterm_Id, @incoterm_Id, @TitleValue1Id, @TitleValue2Id, @purchaseOrderStatus_Id)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[PurchaseOrders]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[PurchaseOrders] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PurchaseOrder_Update",
                p => new
                    {
                        Id = p.Int(),
                       referenceNo = p.String(),
                        fileNo = p.String(),
                        purchaseOrderDate = p.DateTime(),
                        deliveryDate = p.DateTime(),
                        shipmentDate = p.DateTime(),
                        orderConfirmationDate = p.DateTime(),
                        billLaddingDate = p.DateTime(),
                        lCDate = p.DateTime(),
                        materialReciptDate = p.DateTime(),
                        targetYear = p.Int(),
                        lCnumber = p.String(),
                        exchngeRate = p.Single(),
                        commision = p.String(),
                        maker = p.String(),
                        origin = p.String(),
                        comments = p.String(),
                        deliveryTime = p.String(),
                        totalFOBValue = p.Double(),
                        totalCFRValue = p.Double(),
                        isPercentTax = p.Boolean(),
                        salesTax = p.Double(),
                        customerCompany_Id = p.Int(),
                        dept_Id = p.Int(),
                        user_Id = p.Int(),
                        allocation_Id = p.Int(),
                        vendor_Id = p.Int(),
                        principal_Id = p.Int(),
                        company_Id = p.Int(),
                        purchaseOrdertype = p.Int(),
                        offer_Id = p.Int(),
                        bid_Id = p.Int(),
                        currency_Id = p.Int(),
                        paymentterm_Id = p.Int(),
                        incoterm_Id = p.Int(),
                        TitleValue1Id = p.Int(),
                        TitleValue2Id = p.Int(),
                        purchaseOrderStatus_Id = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[PurchaseOrders]
                      SET [referenceNo] = @referenceNo, [fileNo] = @fileNo, [purchaseOrderDate] = @purchaseOrderDate, [deliveryDate] = @deliveryDate, [shipmentDate] = @shipmentDate, [orderConfirmationDate] = @orderConfirmationDate, [billLaddingDate] = @billLaddingDate, [lCDate] = @lCDate, [materialReciptDate] = @materialReciptDate, [targetYear] = @targetYear, [lCnumber] = @lCnumber, [exchngeRate] = @exchngeRate, [commision] = @commision, [maker] = @maker, [origin] = @origin, [comments] = @comments, [deliveryTime] = @deliveryTime, [totalFOBValue] = @totalFOBValue, [totalCFRValue] = @totalCFRValue, [isPercentTax] = @isPercentTax, [salesTax] = @salesTax, [customerCompany_Id] = @customerCompany_Id, [dept_Id] = @dept_Id, [user_Id] = @user_Id, [allocation_Id] = @allocation_Id, [vendor_Id] = @vendor_Id, [principal_Id] = @principal_Id, [company_Id] = @company_Id, [purchaseOrdertype] = @purchaseOrdertype, [offer_Id] = @offer_Id, [bid_Id] = @bid_Id, [currency_Id] = @currency_Id, [paymentterm_Id] = @paymentterm_Id, [incoterm_Id] = @incoterm_Id, [TitleValue1Id] = @TitleValue1Id, [TitleValue2Id] = @TitleValue2Id, [purchaseOrderStatus_Id] = @purchaseOrderStatus_Id
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PurchaseOrder_Delete",
                p => new
                    {
                        Id = p.Int(),
                        purchaseOrderStatus_Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[PurchaseOrders]
                      WHERE (([Id] = @Id) AND (([purchaseOrderStatus_Id] = @purchaseOrderStatus_Id) OR ([purchaseOrderStatus_Id] IS NULL AND @purchaseOrderStatus_Id IS NULL)))"
            );
            
            CreateStoredProcedure(
                "dbo.PurchaseOrderStatus_Insert",
                p => new
                    {
                        Status = p.String(),
                        isApproved = p.Boolean(),
                        isActive = p.Boolean(),
                        backcolor = p.String(),
                        forecolor = p.String(),
                    },
                body:
                    @"INSERT [dbo].[SaleOrderStatus]([Status], [isApproved], [isActive], [backcolor], [forecolor])
                      VALUES (@Status, @isApproved, @isActive, @backcolor, @forecolor)
                      
                      DECLARE @Id int
                      SELECT @Id = [Id]
                      FROM [dbo].[SaleOrderStatus]
                      WHERE @@ROWCOUNT > 0 AND [Id] = scope_identity()
                      
                      SELECT t0.[Id]
                      FROM [dbo].[SaleOrderStatus] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[Id] = @Id"
            );
            
            CreateStoredProcedure(
                "dbo.PurchaseOrderStatus_Update",
                p => new
                    {
                        Id = p.Int(),
                        Status = p.String(),
                        isApproved = p.Boolean(),
                        isActive = p.Boolean(),
                        backcolor = p.String(),
                        forecolor = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[SaleOrderStatus]
                      SET [Status] = @Status, [isApproved] = @isApproved, [isActive] = @isActive, [backcolor] = @backcolor, [forecolor] = @forecolor
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.PurchaseOrderStatus_Delete",
                p => new
                    {
                        Id = p.Int(),
                    },
                body:
                    @"DELETE [dbo].[SaleOrderStatus]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.PurchaseOrderStatus_Delete");
            DropStoredProcedure("dbo.PurchaseOrderStatus_Update");
            DropStoredProcedure("dbo.PurchaseOrderStatus_Insert");
            DropStoredProcedure("dbo.PurchaseOrder_Delete");
            DropStoredProcedure("dbo.PurchaseOrder_Update");
            DropStoredProcedure("dbo.PurchaseOrder_Insert");
            DropStoredProcedure("dbo.Vendor_Delete");
            DropStoredProcedure("dbo.Vendor_Update");
            DropStoredProcedure("dbo.Vendor_Insert");
            DropStoredProcedure("dbo.ProcurementProduct_Delete");
            DropStoredProcedure("dbo.ProcurementProduct_Update");
            DropStoredProcedure("dbo.ProcurementProduct_Insert");
            DropStoredProcedure("dbo.OfferStatus_Delete");
            DropStoredProcedure("dbo.OfferStatus_Update");
            DropStoredProcedure("dbo.OfferStatus_Insert");
            DropStoredProcedure("dbo.Offer_Delete");
            DropStoredProcedure("dbo.Offer_Update");
            DropStoredProcedure("dbo.Offer_Insert");
            DropStoredProcedure("dbo.Product_Delete");
            DropStoredProcedure("dbo.Product_Update");
            DropStoredProcedure("dbo.Product_Insert");
            DropStoredProcedure("dbo.InquiryProduct_Delete");
            DropStoredProcedure("dbo.InquiryProduct_Update");
            DropStoredProcedure("dbo.InquiryProduct_Insert");
            DropStoredProcedure("dbo.InquiryStatus_Delete");
            DropStoredProcedure("dbo.InquiryStatus_Update");
            DropStoredProcedure("dbo.InquiryStatus_Insert");
            DropStoredProcedure("dbo.Inquiry_Delete");
            DropStoredProcedure("dbo.Inquiry_Update");
            DropStoredProcedure("dbo.Inquiry_Insert");
            DropStoredProcedure("dbo.User_Delete");
            DropStoredProcedure("dbo.User_Update");
            DropStoredProcedure("dbo.User_Insert");
            DropStoredProcedure("dbo.Employee_Delete");
            DropStoredProcedure("dbo.Employee_Update");
            DropStoredProcedure("dbo.Employee_Insert");
            DropStoredProcedure("dbo.CustomerCompany_Delete");
            DropStoredProcedure("dbo.CustomerCompany_Update");
            DropStoredProcedure("dbo.CustomerCompany_Insert");
            DropStoredProcedure("dbo.Department_Delete");
            DropStoredProcedure("dbo.Department_Update");
            DropStoredProcedure("dbo.Department_Insert");
            DropStoredProcedure("dbo.Currency_Delete");
            DropStoredProcedure("dbo.Currency_Update");
            DropStoredProcedure("dbo.Currency_Insert");
            DropStoredProcedure("dbo.Contact_Delete");
            DropStoredProcedure("dbo.Contact_Update");
            DropStoredProcedure("dbo.Contact_Insert");
            DropStoredProcedure("dbo.Address_Delete");
            DropStoredProcedure("dbo.Address_Update");
            DropStoredProcedure("dbo.Address_Insert");
        }
    }
}
