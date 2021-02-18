using Microsoft.Extensions.Configuration;

namespace App
{
    public class App
    {
        private readonly IConfiguration _config;
        
        public App(IConfiguration configuration)
        {
            _config = configuration;
        }

        public void Run()
        {
            //do things
        }
    }
}
