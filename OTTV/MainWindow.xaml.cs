using OTTV.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utility.IO;
using Utility.MVVM;

namespace OTTV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Mvv mvv;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = mvv = new Mvv();
           
            Loaded += delegate
            {
                var txts = File.ReadAllLines(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ExampleottvMBE1.txt"));
                foreach (var txt in txts)
                {
                    string[] its = txt.Split('\t');
                    if (string.IsNullOrWhiteSpace(its[0])) continue;

                    var item = new Item();
                    item.Name = its[1];
                    item.alpha = double.Parse(its[2]);
                    item.AWi = double.Parse(its[3]);
                    item.Ugi = double.Parse(its[4]);
                    item.Tdeqwi = double.Parse(its[5]);
                    item.Agi = double.Parse(its[6]);
                    item.SF = double.Parse(its[7]);
                    item.SC = double.Parse(its[8]);
                    item.CF = double.Parse(its[9]);
                    item.SGR = double.Parse(its[10]);
                    item.Ao = double.Parse(its[11]);

                    mvv.Glassmodels.Add(item);
                }

                Start();

                SizeChanged += delegate
                {
                    updateGraph(mvv.Glassmodel.First());
                };
            };
        }

        private void Start()
        {
            //Elevation          α	    AWi 	Ugi	  Tdeqwi  Agi	    SF	  SC	CF	    SGR	    Ao
            //E1 curant case	0.3	    3666.2	6.707	27	  5202.441	191	0.7482	1.05	0.27	10868.64

            var useritem = new Item();
            useritem.Name = "E1";
            useritem.alpha = .3;
            useritem.AWi = 3666.2;
            useritem.Ugi = 6.707;
            useritem.Tdeqwi = 27;
            useritem.Agi = 5202.441;
            useritem.SF = 191;
            useritem.SC = 0.7482;
            useritem.CF = 1.05;
            useritem.SGR = 0.27;
            useritem.Ao = 10868.64;
            
            mvv.Glassmodel.Add(useritem);

            updateGraph(useritem);
        }

        void updateGraph(Item useritem)
        {
            var sitems = mvv.Glassmodels.Where(o=>o.Name.ToLower().StartsWith(useritem.Name.ToLower()));

           sitems.ForEach(o =>
            {
                o.MBE = (useritem.Ottv - o.Ottv) / useritem.Ottv;
            });

            mvv.ImgSource = null;

            var ch = new ZedGraph.GraphPane(); // zedGraphControl1.GraphPane;

            ch.Title.Text = $"OTTV Analysis";
            ch.XAxis.Title.Text = "Glass Types Proposal";
            ch.YAxis.Title.Text = "MBE impact";

            ch.Y2Axis.IsVisible = false;
            ch.X2Axis.IsVisible = false;

            ch.XAxis.Scale.MinAuto = false;
            ch.XAxis.Scale.Max = sitems.Count() + 1;
            ch.XAxis.Scale.MajorStepAuto = false;
            ch.XAxis.Scale.MajorStep = 1;
            ch.XAxis.Scale.Min = 0;
            ch.XAxis.Scale.MinorStepAuto = false;
            ch.XAxis.Scale.MinorStep = 0.1;
            ch.XAxis.MinorTic.Size = 0;
            ch.XAxis.MajorTic.Size = 0;

            ch.YAxis.Scale.Format = "00.00";
            ch.YAxis.MinorTic.Size = 0;
            ch.YAxis.MajorGrid.Color = System.Drawing.Color.DimGray;
            ch.YAxis.MajorGrid.PenWidth = 1;
            ch.YAxis.MajorGrid.DashOn = 6;
            ch.YAxis.MajorGrid.IsVisible = true;

            var bar = ch.AddCurve("MBE Value", null, sitems.Select(o => o.MBE).Cast<double>().ToArray(), System.Drawing.Color.DarkBlue);
            //bar.c.Fill.Type = ZedGraph.FillType.Solid;
            bar.Line.Width = 5f;
            bar.Line.IsSmooth = false;

            ch.XAxis.Scale.TextLabels = sitems.Select(o => o.Name).ToArray();
            ch.XAxis.Type = ZedGraph.AxisType.Text;

            ch.XAxis.Scale.FontSpec.Angle = 90;
             
            ch.AxisChange();

            mvv.ImgSource = Utility.IO.ImageIO.ConvertFromImage(ch.GetImage((int)Width, (int)Height, 150, true));


        }

        List<string> allowed = new List<string>()
        {
            //  α        AWi     Ugi   Tdeqwi  Agi       SF    SC    CF      SGR     Ao
            nameof(Item.Ottv),
            nameof(Item.Name),
            nameof(Item.alpha),
            nameof(Item.AWi),
            nameof(Item.Ugi),
            nameof(Item.Tdeqwi),
            nameof(Item.Agi),
            nameof(Item.SF),
            nameof(Item.SC),
            nameof(Item.CF),
            nameof(Item.SGR),
            nameof(Item.Ao),
            nameof(Item.Rvalue),
            nameof(Item.SGR),
            nameof(Item.MBE)
        };
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var clname = e.Column.Header.ToString();
            if (!allowed.Contains(clname)) e.Cancel = true;
            if (clname == nameof(Item.Ottv)) e.Column.IsReadOnly = true;
            if (clname == nameof(Item.Rvalue)) e.Column.IsReadOnly = true;
            if (clname == nameof(Item.MBE)) e.Column.IsReadOnly = true;
        }

        private void DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            updateGraph(mvv.Glassmodel.First());
        }
    }

    public class Mvv : BaseVModel
    {

        #region Glassmodels

        private ObservableRangeCollection<Item> _Glassmodels;

        public ObservableRangeCollection<Item> Glassmodels
        {
            get
            {
                if (_Glassmodels == null) _Glassmodels = new ObservableRangeCollection<Item>();
                return _Glassmodels;
            }
            set { SetProperty(ref _Glassmodels, value); }

        }
        #endregion


        #region ImgSource

        private ImageSource _ImgSource;

        public ImageSource ImgSource
        {
            get
            {
                return _ImgSource;
            }
            set { SetProperty(ref _ImgSource, value); }

        }
        #endregion


        #region Glassmodel

        private ObservableRangeCollection<Item> _Glassmodel;

        public ObservableRangeCollection<Item> Glassmodel
        {
            get
            {
                if (_Glassmodel == null) _Glassmodel = new ObservableRangeCollection<Item>();
                return _Glassmodel;
            }
            set { SetProperty(ref _Glassmodel, value); }

        }
        #endregion

    }
}
