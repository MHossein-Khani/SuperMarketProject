using SuperMarket.Entities;
using SuperMarket.Infrastructure.Test;
using SuperMarket.Persistance.EF;
using SuperMarket.Specs.Infrastructure;
using static SuperMarket.Specs.BDDHelper;

namespace SuperMarket.Specs.Categories
{
    [Scenario("تعریف دسته بندی")]
    [Feature("",
           AsA = "فروشنده ",
           IWantTo = "دسته بندی را تعریف کنم",
           InOrderTo = "کالا را تعریف کنم"
           )]
    public class AddCategory : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;

        public AddCategory(ConfigurationFixture configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
        }

        [Given("دسته بندی با عنوان 'لبنیات' در فهرست دسته بندی وجود دارد")]
        public void Given()
        {
            var category = new Category
            {
                Name = "لبنیات"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
        }

        [When("دسته بندی با عنوان 'لبنیات' را تعریف میکنیم")]
        public void When()
        {
            var dto = new AddCategoryDto
            {
                Name = "لبنیات"
            };
            var unitOfWork = new EFUnitOfWork(_dataContext);
            CategoryRipository
        }
    }

    public class AddCategoryDto
    {
        public string Name { get; set; }
    }
}
