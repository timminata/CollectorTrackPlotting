using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.IO;
using Newtonsoft.Json;
using GMap.NET.WindowsForms.Markers;

namespace TestMap
{
    public partial class Form1 : Form
    {
        GMapOverlay overlay = new GMapOverlay();

        public Form1()
        {
            InitializeComponent();
            gMapControl1.Overlays.Add(overlay);
        }

        private void gMapControl1_Load(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            gMapControl1.Position = new PointLatLng(-33, 18);
        }

        private void openCollectorFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader r = new StreamReader(openFileDialog1.FileName))
                {
                    string json = r.ReadToEnd();
                    dynamic track = JsonConvert.DeserializeObject(json);
                    List<PointLatLng> allPoints = new List<PointLatLng>();
                    int i = 0;
                    foreach(var point in track.points)
                    {
                        var pointLatLng = new PointLatLng(Convert.ToDouble(point.latitude), Convert.ToDouble(point.longitude));
                        GMarkerGoogle marker = new GMarkerGoogle(pointLatLng, GMarkerGoogleType.gray_small);
                        if (Convert.ToDouble(point.accuracy) > 20) marker = new GMarkerGoogle(pointLatLng, GMarkerGoogleType.red_small);
                        marker.ToolTipText = ""+i++;
                        overlay.Markers.Add(marker);
                        allPoints.Add(pointLatLng);
                    }
                    GMapRoute route = new GMapRoute(allPoints, "test");
                    overlay.Routes.Add(route);
                    gMapControl1.ZoomAndCenterRoute(route);
                }
            }
        }

        
    }
}
