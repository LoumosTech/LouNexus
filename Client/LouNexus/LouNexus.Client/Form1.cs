using LouNexus.Core.Interfaces.Core;
using LouNexus.Core.Models.Core;
using System.Threading.Tasks;

namespace LouNexus.Client
{
    public partial class Form1 : Form
    {
        private readonly IFactoryRepository _factoryRepository;
        public Form1()
        {
            InitializeComponent();
            _factoryRepository = Configuration.AppServices.factoryRepository;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
