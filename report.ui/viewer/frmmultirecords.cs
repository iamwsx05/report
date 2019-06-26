using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Controls;
using Common.Entity;
using weCare.Core.Entity;
using weCare.Core.Utils;
using Report.Entity;

namespace Report.Ui
{
    public partial class frmMultiRecords : frmBasePopup
    {
        public frmMultiRecords(List<EntityPatientInfo> _data)
        {
            InitializeComponent();
        }

        private void frmMultiRecords_Load(object sender, EventArgs e)
        {

        }

        private void frmMultiRecords_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
