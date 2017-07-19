using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQLProfilerHelper
{
    public partial class Form1 : Form
    {
        SpExecuteSqlConversionService _spExecuteSqlConversionService;

        public Form1()
        {
            InitializeComponent();
            _spExecuteSqlConversionService = new SpExecuteSqlConversionService();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void convertExecuteSQLToSQLToolStripMenuItem_Click(object sender, EventArgs e)
        {

            var idat = Clipboard.GetDataObject();
            var text = idat.GetData(DataFormats.Text) as string;

            _spExecuteSqlConversionService.SPExecuteSQLInput = text;

            try
            {
                _spExecuteSqlConversionService.Convert();
            }
            catch (InvalidInputSpExecuteSqlTextException ex)
            {
                DisplayErrorMessage(ex.Message);
            }

            Debug.WriteLine("-----");
            Debug.WriteLine(_spExecuteSqlConversionService.SQLOutput);

            Clipboard.SetText(_spExecuteSqlConversionService.SQLOutput);

            //////////////////////////////////////////////////////////

            void DisplayErrorMessage(string message)
            {
                MessageBox.Show(
                   message,
                   "Conversion failed",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Exclamation,
                   MessageBoxDefaultButton.Button1
                );
            }

        }
    }
}
