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

/* TaRu Logger */
using static TaRU_Jaster.Logger;

namespace TaRU_Jaster
{
    public partial class StatsForm : MaterialForm
    {
        public StatsForm()
        {
            InitializeComponent();

            _materialSkinManager = MaterialSkinManager.Instance;
            _materialSkinManager.AddFormToManage(this);
            _materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            _materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private MaterialSkinManager _materialSkinManager;
        private List<HardyExecutor.TargetStats> _targetStats;

        public void SetTargetStatsList(List<HardyExecutor.TargetStats> w_targetStats)
        {
            _targetStats = w_targetStats;
            for (int targetNo = 0; targetNo < _targetStats.Count; targetNo++)
            {
                // SUPER UGLY HACK TO ONLY GET RECENT UPDATES
                if(DateTime.Now.Subtract(_targetStats[targetNo].lastUpdate).TotalSeconds < 60)
                {
                    ListViewItem item = new ListViewItem((targetNo + 1).ToString());
                    item.SubItems.Add(_targetStats[targetNo].lightOn ? "Yes" : "No");
                    item.SubItems.Add(_targetStats[targetNo].hitsToFall.ToString());
                    item.SubItems.Add(_targetStats[targetNo].battery.ToString());
                    item.SubItems.Add(_targetStats[targetNo].sensitivity.ToString());
                    materialListView1.Items.Add(item);
                }
            }
        }
    }
}
