
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/03/2014 20:36:58
-- Generated from EDMX file: D:\Development Projects\SQuadro\SQuadro\SQuadro.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [SQuadro];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CountryCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Companies] DROP CONSTRAINT [FK_CountryCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_ContactContactType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contacts] DROP CONSTRAINT [FK_ContactContactType];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyContact]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Contacts] DROP CONSTRAINT [FK_CompanyContact];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCompanyArea]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyAreas] DROP CONSTRAINT [FK_CompanyCompanyArea];
GO
IF OBJECT_ID(N'[dbo].[FK_AreaCompanyArea]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyAreas] DROP CONSTRAINT [FK_AreaCompanyArea];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCategoryCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyCategories] DROP CONSTRAINT [FK_CompanyCategoryCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryCategoryCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyCategories] DROP CONSTRAINT [FK_CategoryCategoryCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentTypeDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentTypeDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentStatusDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentStatusDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_RelatedObjectDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_RelatedObjectDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_UserOrganization]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserOrganization];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationArea]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Areas] DROP CONSTRAINT [FK_OrganizationArea];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationCompany]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Companies] DROP CONSTRAINT [FK_OrganizationCompany];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationContactType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContactTypes] DROP CONSTRAINT [FK_OrganizationContactType];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [FK_OrganizationCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationTag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tags] DROP CONSTRAINT [FK_OrganizationTag];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationDocumentStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentStatuses] DROP CONSTRAINT [FK_OrganizationDocumentStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationDocumentType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentTypes] DROP CONSTRAINT [FK_OrganizationDocumentType];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationRelatedObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RelatedObjects] DROP CONSTRAINT [FK_OrganizationRelatedObject];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationDocument]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_OrganizationDocument];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationEmailSenderTemplate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmailSettings] DROP CONSTRAINT [FK_OrganizationEmailSenderTemplate];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationDocumentSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentSets] DROP CONSTRAINT [FK_OrganizationDocumentSet];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentSetDocument_DocumentSet]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentSetDocument] DROP CONSTRAINT [FK_DocumentSetDocument_DocumentSet];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentSetDocument_Document]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentSetDocument] DROP CONSTRAINT [FK_DocumentSetDocument_Document];
GO
IF OBJECT_ID(N'[dbo].[FK_CreatedBy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Companies] DROP CONSTRAINT [FK_CreatedBy];
GO
IF OBJECT_ID(N'[dbo].[FK_UpdatedBy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Companies] DROP CONSTRAINT [FK_UpdatedBy];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentCreatedByUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentCreatedByUser];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentUpdatedByUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_DocumentUpdatedByUser];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationEmailTemplates]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmailTemplates] DROP CONSTRAINT [FK_OrganizationEmailTemplates];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationSubject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subjects] DROP CONSTRAINT [FK_OrganizationSubject];
GO
IF OBJECT_ID(N'[dbo].[FK_OrganizationUserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoles] DROP CONSTRAINT [FK_OrganizationUserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoleUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserRoleUser];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoleCategory_UserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleCategory] DROP CONSTRAINT [FK_UserRoleCategory_UserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoleCategory_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleCategory] DROP CONSTRAINT [FK_UserRoleCategory_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoleRelatedObject_UserRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleRelatedObject] DROP CONSTRAINT [FK_UserRoleRelatedObject_UserRole];
GO
IF OBJECT_ID(N'[dbo].[FK_UserRoleRelatedObject_RelatedObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserRoleRelatedObject] DROP CONSTRAINT [FK_UserRoleRelatedObject_RelatedObject];
GO
IF OBJECT_ID(N'[dbo].[FK_Vessel_inherits_RelatedObject]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RelatedObjects_Vessel] DROP CONSTRAINT [FK_Vessel_inherits_RelatedObject];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO
IF OBJECT_ID(N'[dbo].[Contacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contacts];
GO
IF OBJECT_ID(N'[dbo].[ContactTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactTypes];
GO
IF OBJECT_ID(N'[dbo].[Areas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Areas];
GO
IF OBJECT_ID(N'[dbo].[CompanyAreas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyAreas];
GO
IF OBJECT_ID(N'[dbo].[CompanyCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyCategories];
GO
IF OBJECT_ID(N'[dbo].[DocumentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentTypes];
GO
IF OBJECT_ID(N'[dbo].[DocumentStatuses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentStatuses];
GO
IF OBJECT_ID(N'[dbo].[Tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tags];
GO
IF OBJECT_ID(N'[dbo].[Documents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Documents];
GO
IF OBJECT_ID(N'[dbo].[Organizations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Organizations];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[RelatedObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RelatedObjects];
GO
IF OBJECT_ID(N'[dbo].[EmailSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailSettings];
GO
IF OBJECT_ID(N'[dbo].[DocumentSets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentSets];
GO
IF OBJECT_ID(N'[dbo].[EmailTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmailTemplates];
GO
IF OBJECT_ID(N'[dbo].[Subjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subjects];
GO
IF OBJECT_ID(N'[dbo].[UserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoles];
GO
IF OBJECT_ID(N'[dbo].[RelatedObjects_Vessel]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RelatedObjects_Vessel];
GO
IF OBJECT_ID(N'[dbo].[DocumentSetDocument]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentSetDocument];
GO
IF OBJECT_ID(N'[dbo].[UserRoleCategory]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoleCategory];
GO
IF OBJECT_ID(N'[dbo].[UserRoleRelatedObject]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserRoleRelatedObject];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NULL,
    [Comment] nvarchar(max)  NULL,
    [CountryID_Alpha2] nchar(2)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL,
    [CreatedOn] datetimeoffset  NOT NULL,
    [UpdatedOn] datetimeoffset  NULL,
    [CreatedByUserID] uniqueidentifier  NOT NULL,
    [UpdatedByUserID] uniqueidentifier  NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [ID_Alpha2] nchar(2)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Contacts'
CREATE TABLE [dbo].[Contacts] (
    [ID] uniqueidentifier  NOT NULL,
    [Data] nvarchar(max)  NOT NULL,
    [IsPrimary] bit  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [UserGroup] nvarchar(max)  NULL,
    [ContactTypeID] int  NOT NULL,
    [CompanyID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'ContactTypes'
CREATE TABLE [dbo].[ContactTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [DisplayPattern] nvarchar(128)  NULL,
    [OrganizationID] uniqueidentifier  NOT NULL,
    [SystemType] int  NULL
);
GO

-- Creating table 'Areas'
CREATE TABLE [dbo].[Areas] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'CompanyAreas'
CREATE TABLE [dbo].[CompanyAreas] (
    [ID] uniqueidentifier  NOT NULL,
    [CompanyID] uniqueidentifier  NOT NULL,
    [AreaID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'CompanyCategories'
CREATE TABLE [dbo].[CompanyCategories] (
    [ID] uniqueidentifier  NOT NULL,
    [CompanyID] uniqueidentifier  NOT NULL,
    [CategoryID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'DocumentTypes'
CREATE TABLE [dbo].[DocumentTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'DocumentStatuses'
CREATE TABLE [dbo].[DocumentStatuses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Tags'
CREATE TABLE [dbo].[Tags] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(32)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Documents'
CREATE TABLE [dbo].[Documents] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(512)  NOT NULL,
    [Number] nvarchar(256)  NULL,
    [FileName] nvarchar(512)  NOT NULL,
    [Date] datetime  NOT NULL,
    [ExpirationDate] datetime  NULL,
    [Comment] nvarchar(max)  NULL,
    [DocumentTypeID] int  NULL,
    [DocumentStatusID] int  NULL,
    [Tags] nvarchar(max)  NULL,
    [RelatedObjectID] uniqueidentifier  NULL,
    [OrganizationID] uniqueidentifier  NOT NULL,
    [CreatedOn] datetimeoffset  NOT NULL,
    [UpdatedOn] datetimeoffset  NULL,
    [CreatedByUserID] uniqueidentifier  NOT NULL,
    [UpdatedByUserID] uniqueidentifier  NULL,
    [File] varbinary(max)  NULL
);
GO

-- Creating table 'Organizations'
CREATE TABLE [dbo].[Organizations] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Timezone] varchar(256)  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [Email] nvarchar(256)  NOT NULL,
    [Role] int  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL,
    [ForceChangePassword] bit  NOT NULL,
    [UserRoleID] uniqueidentifier  NULL
);
GO

-- Creating table 'RelatedObjects'
CREATE TABLE [dbo].[RelatedObjects] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'EmailSettings'
CREATE TABLE [dbo].[EmailSettings] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL,
    [IsDefault] bit  NOT NULL,
    [Email] nvarchar(256)  NOT NULL,
    [SmtpServer] nvarchar(256)  NOT NULL,
    [SmtpPort] int  NOT NULL,
    [SmtpUser] nvarchar(256)  NOT NULL,
    [SmtpPassword] nvarchar(256)  NOT NULL,
    [EnableSsl] bit  NOT NULL
);
GO

-- Creating table 'DocumentSets'
CREATE TABLE [dbo].[DocumentSets] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(256)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'EmailTemplates'
CREATE TABLE [dbo].[EmailTemplates] (
    [ID] uniqueidentifier  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL,
    [Type] int  NOT NULL,
    [Body] nvarchar(max)  NULL,
    [Salutation] nvarchar(128)  NULL,
    [Signature] nvarchar(128)  NULL,
    [Subject] nvarchar(256)  NULL
);
GO

-- Creating table 'Subjects'
CREATE TABLE [dbo].[Subjects] (
    [ID] bigint IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(256)  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserRoles'
CREATE TABLE [dbo].[UserRoles] (
    [ID] uniqueidentifier  NOT NULL,
    [OrganizationID] uniqueidentifier  NOT NULL,
    [IsReadonly] bit  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'RelatedObjects_Vessel'
CREATE TABLE [dbo].[RelatedObjects_Vessel] (
    [ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'DocumentSetDocument'
CREATE TABLE [dbo].[DocumentSetDocument] (
    [DocumentSets_ID] uniqueidentifier  NOT NULL,
    [Documents_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserRoleCategory'
CREATE TABLE [dbo].[UserRoleCategory] (
    [UserRoles_ID] uniqueidentifier  NOT NULL,
    [Categories_ID] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'UserRoleRelatedObject'
CREATE TABLE [dbo].[UserRoleRelatedObject] (
    [UserRoles_ID] uniqueidentifier  NOT NULL,
    [RelatedObjects_ID] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID_Alpha2] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([ID_Alpha2] ASC);
GO

-- Creating primary key on [ID] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [PK_Contacts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ContactTypes'
ALTER TABLE [dbo].[ContactTypes]
ADD CONSTRAINT [PK_ContactTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Areas'
ALTER TABLE [dbo].[Areas]
ADD CONSTRAINT [PK_Areas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CompanyAreas'
ALTER TABLE [dbo].[CompanyAreas]
ADD CONSTRAINT [PK_CompanyAreas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CompanyCategories'
ALTER TABLE [dbo].[CompanyCategories]
ADD CONSTRAINT [PK_CompanyCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentTypes'
ALTER TABLE [dbo].[DocumentTypes]
ADD CONSTRAINT [PK_DocumentTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentStatuses'
ALTER TABLE [dbo].[DocumentStatuses]
ADD CONSTRAINT [PK_DocumentStatuses]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [PK_Tags]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [PK_Documents]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Organizations'
ALTER TABLE [dbo].[Organizations]
ADD CONSTRAINT [PK_Organizations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RelatedObjects'
ALTER TABLE [dbo].[RelatedObjects]
ADD CONSTRAINT [PK_RelatedObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EmailSettings'
ALTER TABLE [dbo].[EmailSettings]
ADD CONSTRAINT [PK_EmailSettings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentSets'
ALTER TABLE [dbo].[DocumentSets]
ADD CONSTRAINT [PK_DocumentSets]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EmailTemplates'
ALTER TABLE [dbo].[EmailTemplates]
ADD CONSTRAINT [PK_EmailTemplates]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Subjects'
ALTER TABLE [dbo].[Subjects]
ADD CONSTRAINT [PK_Subjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [PK_UserRoles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RelatedObjects_Vessel'
ALTER TABLE [dbo].[RelatedObjects_Vessel]
ADD CONSTRAINT [PK_RelatedObjects_Vessel]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [DocumentSets_ID], [Documents_ID] in table 'DocumentSetDocument'
ALTER TABLE [dbo].[DocumentSetDocument]
ADD CONSTRAINT [PK_DocumentSetDocument]
    PRIMARY KEY NONCLUSTERED ([DocumentSets_ID], [Documents_ID] ASC);
GO

-- Creating primary key on [UserRoles_ID], [Categories_ID] in table 'UserRoleCategory'
ALTER TABLE [dbo].[UserRoleCategory]
ADD CONSTRAINT [PK_UserRoleCategory]
    PRIMARY KEY NONCLUSTERED ([UserRoles_ID], [Categories_ID] ASC);
GO

-- Creating primary key on [UserRoles_ID], [RelatedObjects_ID] in table 'UserRoleRelatedObject'
ALTER TABLE [dbo].[UserRoleRelatedObject]
ADD CONSTRAINT [PK_UserRoleRelatedObject]
    PRIMARY KEY NONCLUSTERED ([UserRoles_ID], [RelatedObjects_ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CountryID_Alpha2] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_CountryCompany]
    FOREIGN KEY ([CountryID_Alpha2])
    REFERENCES [dbo].[Countries]
        ([ID_Alpha2])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CountryCompany'
CREATE INDEX [IX_FK_CountryCompany]
ON [dbo].[Companies]
    ([CountryID_Alpha2]);
GO

-- Creating foreign key on [ContactTypeID] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [FK_ContactContactType]
    FOREIGN KEY ([ContactTypeID])
    REFERENCES [dbo].[ContactTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ContactContactType'
CREATE INDEX [IX_FK_ContactContactType]
ON [dbo].[Contacts]
    ([ContactTypeID]);
GO

-- Creating foreign key on [CompanyID] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [FK_CompanyContact]
    FOREIGN KEY ([CompanyID])
    REFERENCES [dbo].[Companies]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyContact'
CREATE INDEX [IX_FK_CompanyContact]
ON [dbo].[Contacts]
    ([CompanyID]);
GO

-- Creating foreign key on [CompanyID] in table 'CompanyAreas'
ALTER TABLE [dbo].[CompanyAreas]
ADD CONSTRAINT [FK_CompanyCompanyArea]
    FOREIGN KEY ([CompanyID])
    REFERENCES [dbo].[Companies]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCompanyArea'
CREATE INDEX [IX_FK_CompanyCompanyArea]
ON [dbo].[CompanyAreas]
    ([CompanyID]);
GO

-- Creating foreign key on [AreaID] in table 'CompanyAreas'
ALTER TABLE [dbo].[CompanyAreas]
ADD CONSTRAINT [FK_AreaCompanyArea]
    FOREIGN KEY ([AreaID])
    REFERENCES [dbo].[Areas]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AreaCompanyArea'
CREATE INDEX [IX_FK_AreaCompanyArea]
ON [dbo].[CompanyAreas]
    ([AreaID]);
GO

-- Creating foreign key on [CompanyID] in table 'CompanyCategories'
ALTER TABLE [dbo].[CompanyCategories]
ADD CONSTRAINT [FK_CompanyCategoryCompany]
    FOREIGN KEY ([CompanyID])
    REFERENCES [dbo].[Companies]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCategoryCompany'
CREATE INDEX [IX_FK_CompanyCategoryCompany]
ON [dbo].[CompanyCategories]
    ([CompanyID]);
GO

-- Creating foreign key on [CategoryID] in table 'CompanyCategories'
ALTER TABLE [dbo].[CompanyCategories]
ADD CONSTRAINT [FK_CategoryCategoryCompany]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryCategoryCompany'
CREATE INDEX [IX_FK_CategoryCategoryCompany]
ON [dbo].[CompanyCategories]
    ([CategoryID]);
GO

-- Creating foreign key on [DocumentTypeID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_DocumentTypeDocument]
    FOREIGN KEY ([DocumentTypeID])
    REFERENCES [dbo].[DocumentTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentTypeDocument'
CREATE INDEX [IX_FK_DocumentTypeDocument]
ON [dbo].[Documents]
    ([DocumentTypeID]);
GO

-- Creating foreign key on [DocumentStatusID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_DocumentStatusDocument]
    FOREIGN KEY ([DocumentStatusID])
    REFERENCES [dbo].[DocumentStatuses]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentStatusDocument'
CREATE INDEX [IX_FK_DocumentStatusDocument]
ON [dbo].[Documents]
    ([DocumentStatusID]);
GO

-- Creating foreign key on [RelatedObjectID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_RelatedObjectDocument]
    FOREIGN KEY ([RelatedObjectID])
    REFERENCES [dbo].[RelatedObjects]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RelatedObjectDocument'
CREATE INDEX [IX_FK_RelatedObjectDocument]
ON [dbo].[Documents]
    ([RelatedObjectID]);
GO

-- Creating foreign key on [OrganizationID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserOrganization]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserOrganization'
CREATE INDEX [IX_FK_UserOrganization]
ON [dbo].[Users]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'Areas'
ALTER TABLE [dbo].[Areas]
ADD CONSTRAINT [FK_OrganizationArea]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationArea'
CREATE INDEX [IX_FK_OrganizationArea]
ON [dbo].[Areas]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_OrganizationCompany]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationCompany'
CREATE INDEX [IX_FK_OrganizationCompany]
ON [dbo].[Companies]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'ContactTypes'
ALTER TABLE [dbo].[ContactTypes]
ADD CONSTRAINT [FK_OrganizationContactType]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationContactType'
CREATE INDEX [IX_FK_OrganizationContactType]
ON [dbo].[ContactTypes]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [FK_OrganizationCategory]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationCategory'
CREATE INDEX [IX_FK_OrganizationCategory]
ON [dbo].[Categories]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'Tags'
ALTER TABLE [dbo].[Tags]
ADD CONSTRAINT [FK_OrganizationTag]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationTag'
CREATE INDEX [IX_FK_OrganizationTag]
ON [dbo].[Tags]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'DocumentStatuses'
ALTER TABLE [dbo].[DocumentStatuses]
ADD CONSTRAINT [FK_OrganizationDocumentStatus]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationDocumentStatus'
CREATE INDEX [IX_FK_OrganizationDocumentStatus]
ON [dbo].[DocumentStatuses]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'DocumentTypes'
ALTER TABLE [dbo].[DocumentTypes]
ADD CONSTRAINT [FK_OrganizationDocumentType]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationDocumentType'
CREATE INDEX [IX_FK_OrganizationDocumentType]
ON [dbo].[DocumentTypes]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'RelatedObjects'
ALTER TABLE [dbo].[RelatedObjects]
ADD CONSTRAINT [FK_OrganizationRelatedObject]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationRelatedObject'
CREATE INDEX [IX_FK_OrganizationRelatedObject]
ON [dbo].[RelatedObjects]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_OrganizationDocument]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationDocument'
CREATE INDEX [IX_FK_OrganizationDocument]
ON [dbo].[Documents]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'EmailSettings'
ALTER TABLE [dbo].[EmailSettings]
ADD CONSTRAINT [FK_OrganizationEmailSenderTemplate]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationEmailSenderTemplate'
CREATE INDEX [IX_FK_OrganizationEmailSenderTemplate]
ON [dbo].[EmailSettings]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'DocumentSets'
ALTER TABLE [dbo].[DocumentSets]
ADD CONSTRAINT [FK_OrganizationDocumentSet]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationDocumentSet'
CREATE INDEX [IX_FK_OrganizationDocumentSet]
ON [dbo].[DocumentSets]
    ([OrganizationID]);
GO

-- Creating foreign key on [DocumentSets_ID] in table 'DocumentSetDocument'
ALTER TABLE [dbo].[DocumentSetDocument]
ADD CONSTRAINT [FK_DocumentSetDocument_DocumentSet]
    FOREIGN KEY ([DocumentSets_ID])
    REFERENCES [dbo].[DocumentSets]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Documents_ID] in table 'DocumentSetDocument'
ALTER TABLE [dbo].[DocumentSetDocument]
ADD CONSTRAINT [FK_DocumentSetDocument_Document]
    FOREIGN KEY ([Documents_ID])
    REFERENCES [dbo].[Documents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentSetDocument_Document'
CREATE INDEX [IX_FK_DocumentSetDocument_Document]
ON [dbo].[DocumentSetDocument]
    ([Documents_ID]);
GO

-- Creating foreign key on [CreatedByUserID] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_CreatedBy]
    FOREIGN KEY ([CreatedByUserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CreatedBy'
CREATE INDEX [IX_FK_CreatedBy]
ON [dbo].[Companies]
    ([CreatedByUserID]);
GO

-- Creating foreign key on [UpdatedByUserID] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [FK_UpdatedBy]
    FOREIGN KEY ([UpdatedByUserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UpdatedBy'
CREATE INDEX [IX_FK_UpdatedBy]
ON [dbo].[Companies]
    ([UpdatedByUserID]);
GO

-- Creating foreign key on [CreatedByUserID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_DocumentCreatedByUser]
    FOREIGN KEY ([CreatedByUserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentCreatedByUser'
CREATE INDEX [IX_FK_DocumentCreatedByUser]
ON [dbo].[Documents]
    ([CreatedByUserID]);
GO

-- Creating foreign key on [UpdatedByUserID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_DocumentUpdatedByUser]
    FOREIGN KEY ([UpdatedByUserID])
    REFERENCES [dbo].[Users]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentUpdatedByUser'
CREATE INDEX [IX_FK_DocumentUpdatedByUser]
ON [dbo].[Documents]
    ([UpdatedByUserID]);
GO

-- Creating foreign key on [OrganizationID] in table 'EmailTemplates'
ALTER TABLE [dbo].[EmailTemplates]
ADD CONSTRAINT [FK_OrganizationEmailTemplates]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationEmailTemplates'
CREATE INDEX [IX_FK_OrganizationEmailTemplates]
ON [dbo].[EmailTemplates]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'Subjects'
ALTER TABLE [dbo].[Subjects]
ADD CONSTRAINT [FK_OrganizationSubject]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationSubject'
CREATE INDEX [IX_FK_OrganizationSubject]
ON [dbo].[Subjects]
    ([OrganizationID]);
GO

-- Creating foreign key on [OrganizationID] in table 'UserRoles'
ALTER TABLE [dbo].[UserRoles]
ADD CONSTRAINT [FK_OrganizationUserRole]
    FOREIGN KEY ([OrganizationID])
    REFERENCES [dbo].[Organizations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_OrganizationUserRole'
CREATE INDEX [IX_FK_OrganizationUserRole]
ON [dbo].[UserRoles]
    ([OrganizationID]);
GO

-- Creating foreign key on [UserRoleID] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserRoleUser]
    FOREIGN KEY ([UserRoleID])
    REFERENCES [dbo].[UserRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleUser'
CREATE INDEX [IX_FK_UserRoleUser]
ON [dbo].[Users]
    ([UserRoleID]);
GO

-- Creating foreign key on [UserRoles_ID] in table 'UserRoleCategory'
ALTER TABLE [dbo].[UserRoleCategory]
ADD CONSTRAINT [FK_UserRoleCategory_UserRole]
    FOREIGN KEY ([UserRoles_ID])
    REFERENCES [dbo].[UserRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Categories_ID] in table 'UserRoleCategory'
ALTER TABLE [dbo].[UserRoleCategory]
ADD CONSTRAINT [FK_UserRoleCategory_Category]
    FOREIGN KEY ([Categories_ID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleCategory_Category'
CREATE INDEX [IX_FK_UserRoleCategory_Category]
ON [dbo].[UserRoleCategory]
    ([Categories_ID]);
GO

-- Creating foreign key on [UserRoles_ID] in table 'UserRoleRelatedObject'
ALTER TABLE [dbo].[UserRoleRelatedObject]
ADD CONSTRAINT [FK_UserRoleRelatedObject_UserRole]
    FOREIGN KEY ([UserRoles_ID])
    REFERENCES [dbo].[UserRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [RelatedObjects_ID] in table 'UserRoleRelatedObject'
ALTER TABLE [dbo].[UserRoleRelatedObject]
ADD CONSTRAINT [FK_UserRoleRelatedObject_RelatedObject]
    FOREIGN KEY ([RelatedObjects_ID])
    REFERENCES [dbo].[RelatedObjects]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRoleRelatedObject_RelatedObject'
CREATE INDEX [IX_FK_UserRoleRelatedObject_RelatedObject]
ON [dbo].[UserRoleRelatedObject]
    ([RelatedObjects_ID]);
GO

-- Creating foreign key on [ID] in table 'RelatedObjects_Vessel'
ALTER TABLE [dbo].[RelatedObjects_Vessel]
ADD CONSTRAINT [FK_Vessel_inherits_RelatedObject]
    FOREIGN KEY ([ID])
    REFERENCES [dbo].[RelatedObjects]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------