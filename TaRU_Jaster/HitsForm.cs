using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MaterialSkin;
using MaterialSkin.Controls;


namespace TaRU_Jaster
{
    public partial class HitsForm : MaterialForm
    {
        public struct TargetHits
        {
            public int targetNo;
            public int overallHits;
            public int riseCount;
            public int hitFallCount;
        }

        private MaterialSkinManager _materialSkinManager;
        private List<TargetHits> _targetHits;


        public HitsForm()
        {
            InitializeComponent();

            // Set the window color scheme
            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.AddFormToManage(this);
            _materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            _materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

        }

        public void SetTargetHitsList(List<TargetHits> w_targetHits)
        {
            _targetHits = w_targetHits;
            foreach(TargetHits target in _targetHits)
            {
                ListViewItem item = new ListViewItem((target.targetNo).ToString());
                item.SubItems.Add(target.overallHits.ToString());
                item.SubItems.Add(target.riseCount.ToString());
                item.SubItems.Add(target.hitFallCount.ToString());
                item.SubItems.Add((target.hitFallCount * 100 / target.riseCount).ToString() + "%");
                materialListView1.Items.Add(item);
            }
        }

    }
}
