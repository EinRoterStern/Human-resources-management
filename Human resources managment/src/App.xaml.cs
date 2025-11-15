using Human_resources_managment.PostgresDataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Human_resources_managment
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static IServiceProvider Services { get; private set; } = null!;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //var config = new ConfigurationBuilder()
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.Build();

            //var services = new ServiceCollection();

            //var cs = config.GetConnectionString("DefaultConnection");

            //services.AddDbContext<DBContextHRM>(opt =>
            //opt.UseNpgsql(cs));

        }
    }

}
