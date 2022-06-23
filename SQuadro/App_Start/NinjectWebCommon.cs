[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(SQuadro.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(SQuadro.App_Start.NinjectWebCommon), "Stop")]

namespace SQuadro.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using SQuadro.Models;
    using SQuadro.Controllers;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUsersHelper>().To<UsersHelper>();
            kernel.Bind<IListTemplate>().To<AreasList>().WhenInjectedInto<AreasController>();
            kernel.Bind<IListTemplate>().To<CategoriesList>().WhenInjectedInto<CategoriesController>();
            kernel.Bind<IListTemplate>().To<ContactTypesList>().WhenInjectedInto<ContactTypesController>();
            kernel.Bind<IListTemplate>().To<DocumentsList>().WhenInjectedInto<DocumentsController>();
            kernel.Bind<IListTemplate>().To<DocumentSetsList>().WhenInjectedInto<DocumentSetsController>();
            kernel.Bind<IListTemplate>().To<DocumentStatusesList>().WhenInjectedInto<DocumentStatusesController>();
            kernel.Bind<IListTemplate>().To<DocumentTypesList>().WhenInjectedInto<DocumentTypesController>();
            kernel.Bind<IListTemplate>().To<EmailSettingsList>().WhenInjectedInto<EmailSettingsController>();
            kernel.Bind<IListTemplate>().To<EmailTemplatesList>().WhenInjectedInto<EmailTemplatesController>();
            kernel.Bind<IListTemplate>().To<PartnersList>().WhenInjectedInto<PartnersController>();
            kernel.Bind<IListTemplate>().To<SubjectsList>().WhenInjectedInto<SubjectsController>();
            kernel.Bind<IListTemplate>().To<TagsList>().WhenInjectedInto<TagsController>();
            kernel.Bind<IListTemplate>().To<UserRolesList>().WhenInjectedInto<UserRolesController>();
            kernel.Bind<IListTemplate>().To<UsersList>().WhenInjectedInto<UsersController>();
            kernel.Bind<IListTemplate>().To<VesselsList>().WhenInjectedInto<VesselsController>();
        }        
    }
}
